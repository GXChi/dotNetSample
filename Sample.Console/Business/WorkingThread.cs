using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Console.Business
{
    /// <summary>
    /// 工作线程，
    /// </summary>
    public class WorkingThread
    {
        /// <summary>
        /// //泊位处理线程,对泊位相应的设备故障检测，订单更新，告警处理，工单派送
        /// </summary>
        private Thread berthStatusHandle;
        /// <summary>
        /// //进行中的订单处理线程,对进行中的订单进行状态判断，短信发送
        /// </summary>
        private Thread runningOrderHandle;
        /// <summary>
        /// 充值交易处理,调用第三方充值接口，查询充值交易结果
        /// </summary>
        private Thread rechargeHandle;
        /// <summary>
        /// 退款申请处理线程，调用第三方支付接口，查询充值交易，申请退款
        /// </summary>
        private Thread applyRefundHandle;
        /// <summary>
        /// 退款申请查询线程，调用第三方支付接口，查询充值交易，申请退款
        /// </summary>
        private Thread rechargeRefundHandle;
        //private Thread pdaUserLoginCheck;//PDA登录检验线程
        private Timer pdaUserLoginDetectTime;//PDA登录检验线程
        ///唯传数据接收服务
        //private IDeviceDataService winextDataService;

        private Timer pdaUserLevelMonitor;//用户等级监测

        /// <summary>
        /// 违章欠费对账，对已支付未结束的欠费单进行处理      add by zdl 2019.12.04
        /// </summary>
        private Timer pdaChargeVerifyMonitor;

        /// <summary>
        /// 网关掉线通知线程        add by zdl 2020.04.15
        /// </summary>
        private Thread gatewayHandle;

        /// <summary>
        /// 运行系统监控服务
        /// </summary>
        public void Running()
        {
            System.Console.WriteLine("停车系统服务正在启动。。。。。。");
            MemoryCache.IsRunning = true;
            berthStatusHandle = new Thread(BerthHandle);
            berthStatusHandle.Start();
            //休息一下，省得线程在同一时间启动请求数据，数据库压力好大 
            Task.Delay(1).Wait();
            System.Console.WriteLine("泊位状态监控线程启动成功");
            runningOrderHandle = new Thread(RunningOrderHandle);
            runningOrderHandle.Start();
            Task.Delay(1).Wait();
            System.Console.WriteLine("订单状态监控线程启动成功");
            rechargeHandle = new Thread(RechargeHandle);
            rechargeHandle.Start();
            Task.Delay(1).Wait();
            System.Console.WriteLine("充值订单状态监控线程启动成功");
            applyRefundHandle = new Thread(ApplyRefundHandle);
            applyRefundHandle.Start();
            Task.Delay(1).Wait();

            gatewayHandle = new Thread(GatewayHandle);
            gatewayHandle.Start();
            Task.Delay(1).Wait();
            System.Console.WriteLine("网关掉线通知线程启动成功");

            //System.Console.WriteLine("退款订单状态监控线程启动成功");
            //rechargeRefundHandle = new Thread(RechargeRefundHandle);
            //rechargeRefundHandle.Start();
            //Task.Delay(1).Wait();
            //System.Console.WriteLine("退款订单状态查询线程启动成功");

            //Pda 用户在线检测时间间隔
            int interval = 60000;
            //Pda 用户在线检测定时器
            pdaUserLoginDetectTime = new Timer(PdaUserLoginDetect, null, 0, interval);
            System.Console.WriteLine("PDA登录状态监控线程启动成功");
            //winextDataService = new WinextDataService();
            //winextDataService.Start();

            System.Console.WriteLine("停车系统服务启动成功");
            //用户等级检测时间间隔
            int intervalLevel = 86400000;//60000*60*24;//一天
            //用户等级检测定时器
            pdaUserLevelMonitor = new Timer(UserLevelMonitor, null, 0, intervalLevel);
            DateTime now = DateTime.Now;
            DateTime now1 = now.Date.AddHours(1);
            int dueInterval = 0;
            if (now >= now1)
            {
                dueInterval = (int)now1.AddDays(1).Subtract(now).TotalMilliseconds;
            }
            else
            {
                dueInterval = (int)now1.Subtract(now).TotalMilliseconds;
            }
            //先执行等级判断，过后把用户等级监控时间更改在凌晨
            Task.Delay(1000 * 10).Wait();
            pdaUserLevelMonitor.Change(dueInterval, intervalLevel);
            System.Console.WriteLine("用户等级监控线程启动成功");

            //欠费对账时间间隔
            int intervalCharge = 86400000;
            //违章欠费对账计时器
            pdaChargeVerifyMonitor = new Timer(ChargeVerifyMonitor, null, 0, intervalCharge);
            DateTime date1 = DateTime.Now;
            DateTime date2 = date1.Date.AddHours(1);
            int dueInterval1 = 0;
            if (date1 >= date2)
            {
                dueInterval1 = (int)date2.AddDays(1).Subtract(date1).TotalMilliseconds;
            }
            else
            {
                dueInterval1 = (int)date2.Subtract(date1).TotalMilliseconds;
            }
            pdaChargeVerifyMonitor.Change(dueInterval1, intervalCharge);

        }

        /// <summary>
        /// 充值结果查询线程，调用第三方充值接口，查询充值交易结果
        /// </summary>
        /// <param name="obj"></param>
        private void RechargeHandle(object obj)
        {
            RechargeHandle rechargeHandle = new RechargeHandle();
            while (MemoryCache.IsRunning)
            {
                rechargeHandle.Handle();
                Task.Delay(1000 * 3).Wait();
            }
        }

        /// <summary>
        /// 退款申请处理线程，调用第三方支付接口，查询充值交易，申请退款
        /// </summary>
        /// <param name="obj"></param>
        private void ApplyRefundHandle(object obj)
        {
            ApplyRefund rechargeRefund = new ApplyRefund();
            while (MemoryCache.IsRunning)
            {
                rechargeRefund.Handle();
                Task.Delay(1000 * 10).Wait();
            }
        }
        /// <summary>
        /// 退款查询处理线程，调用第三方支付接口，查询充值交易，申请退款
        /// </summary>
        /// <param name="obj"></param>
        private void RechargeRefundHandle(object obj)
        {
            RechargeRefund rechargeRefund = new RechargeRefund();
            while (MemoryCache.IsRunning)
            {
                rechargeRefund.QureyRechargeRefund();
                Task.Delay(1000 * 10).Wait();
            }
        }

        /// <summary>
        /// 停止系统监控服务
        /// </summary>
        public void Stop()
        {
            ///系统运行状态设置为false
            MemoryCache.IsRunning = false;
            ///停止泊位监控线程
            if (berthStatusHandle != null)
            {
                berthStatusHandle.Abort();
            }
            ///停止订单监控线程
            if (runningOrderHandle != null)
            {
                runningOrderHandle.Abort();
            }
            ///停止交易监控线程
            if (rechargeHandle != null)
            {
                rechargeHandle.Abort();
            }
            ///停止提现申请监控线程
            if (applyRefundHandle != null)
            {
                applyRefundHandle.Abort();
            }
            ///停止退款查询监控线程
            if (rechargeRefundHandle != null)
            {
                rechargeRefundHandle.Abort();
            }

            ///停止网关掉线通知线程
            if (gatewayHandle != null)
            {
                gatewayHandle.Abort();
            }
            ///停止PDA登录状态保持定时器线程
            pdaUserLoginDetectTime.Dispose();
            ///停止唯传数据服务
            //if (winextDataService!=null)
            //{
            //    winextDataService.Stop();
            //} 
            pdaUserLevelMonitor.Dispose();
            System.Console.WriteLine("停车系统服务停止成功");
        }

        /// <summary>
        /// 订单处理线程
        /// 订单处理 续费提醒，超时提醒，超时告警，消息推送
        /// </summary>
        private void RunningOrderHandle()
        {
            OrderHandle orderHandle = new OrderHandle();
            while (MemoryCache.IsRunning)
            {
                orderHandle.Handle();
                Task.Delay(1000 * 10).Wait();
            }
        }

        /// <summary>
        /// 泊位状态处理线程
        /// 对泊位相应的设备故障检测，订单更新，告警处理，工单派送
        /// </summary>
        private void BerthHandle()
        {
            BerthHandle berthHandle = new BerthHandle();
            while (MemoryCache.IsRunning)//循环判断泊位状态
            {
                berthHandle.Handle();
                Task.Delay(5 * 1000).Wait();
            }
        }

        //private void BerthStatusHandle()
        //{
        //    BerthStatusHandle berthStatusHandle = new BerthStatusHandle();
        //    while (MemoryCache.IsRunning)//循环判断泊位状态
        //    {
        //        berthStatusHandle.Handle();
        //        Task.Delay(1 * 1000).Wait();
        //    }
        //}

        /// <summary>
        /// 网关掉线通知线程        add by zdl 2020.04.15
        /// </summary>
        private void GatewayHandle()
        {
            GatewayHandle gatewayHandle = new GatewayHandle();
            while (MemoryCache.IsRunning)//循环判断泊位状态
            {
                gatewayHandle.Handle();
                //每半小时查询一次
                Task.Delay(30 * 60 * 1000).Wait();
            }
        }

        /// <summary>
        /// Pda 用户在线检测方法
        /// </summary>
        /// <param name="o"></param>
        private void PdaUserLoginDetect(object o)
        {
            //System.Console.WriteLine("Pda 用户在线检测线程启动");
            UserLoginDetect UserLoginDetect = new UserLoginDetect();
            UserLoginDetect.Detect();
            //System.Console.WriteLine("Pda 用户在线检测线程结束");
        }

        /// <summary>
        /// 用户等级检测方法
        /// </summary>
        /// <param name="o"></param>
        private void UserLevelMonitor(object o)
        {
            System.Console.WriteLine("用户等级监控线程启动");
            UserLevelMonitor userLevelMonitor = new UserLevelMonitor();
            userLevelMonitor.Monitor();
            System.Console.WriteLine("用户等级监控线程结束");
        }


        private void ChargeVerifyMonitor(object o)
        {
            System.Console.WriteLine("欠费对账监控线程启动");
            ChargeVerifyMonitor chargeVerifyMonitor = new ChargeVerifyMonitor();
            chargeVerifyMonitor.Monitor();
            System.Console.WriteLine("欠费对账监控线程结束");
        }
    }
}
