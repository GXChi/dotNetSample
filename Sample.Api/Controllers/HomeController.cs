using Sample.Common.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Http;

namespace Sample.Web.Controllers
{
    public class HomeController : ApiController
    {
        [Route("CarPark/PushHeartbeat")]
        [HttpPost]
        public dynamic PushHeartbeat(SectionHeartbeat data)
        {
            LogHelper.WriteLog("CarPark", "PushHeartbeat", JsonHelper.JsonSerializer<SectionHeartbeat>(data) + "\r\n Authorization:" + Request.Headers.Authorization);
            return new { code = "0", msg = "请求成功" };
        }

        [Route("CarPark/PushInParkRecord")]
        [HttpPost]
        public dynamic PushInParkRecord(InParkRecord data)
        {
            LogHelper.WriteLog("CarPark", "PushInParkRecord", JsonHelper.JsonSerializer<InParkRecord>(data) + "\r\n Authorization:" + Request.Headers.Authorization);
            return new { code = "0", msg = "请求成功" };
        }

        [Route("CarPark/PushOutParkRecord")]
        [HttpPost]
        public dynamic PushOutParkRecord(OutParkRecord data)
        {
            LogHelper.WriteLog("CarPark", "PushOutParkRecord", JsonHelper.JsonSerializer<OutParkRecord>(data) + "\r\n Authorization:" + Request.Headers.Authorization);
            return new { code = "0", msg = "请求成功" };
        }

    }

    public class SectionHeartbeat
    {
        /// <summary>
        /// 路段id
        /// </summary>
        public string SectionId { get; set; }
        /// <summary>
        /// 路段名称
        /// </summary>
        public string SectionName { get; set; }
        /// <summary>
        /// 总泊位数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 已使用泊位数
        /// </summary>
        //public int UseCount { get; set; }
        /// <summary>
        /// 剩余泊位数
        /// </summary>
        public int RemainderCount { get; set; }
    }

    public class InParkRecord
    {
        /// <summary>
        ///  关联id（和出场，图片推送relationId保持一致）
        /// </summary>
        public string relationId { get; set; }
        /// <summary>
        /// 路段id
        /// </summary>
        public string sectionId { get; set; }
        /// <summary>
        /// 泊位号
        /// </summary>
        public string berthCode { get; set; }
        /// <summary>
        /// 入场时间
        /// </summary>
        public string startParkingTime { get; set; }
    }

    public class OutParkRecord
    {
        /// <summary>
        /// 关联id（和进场relationId保持一致）
        /// </summary>
        public string relationId { get; set; }
        /// <summary>
        /// 路段id
        /// </summary>
        public string sectionId { get; set; }
        /// <summary>
        /// 泊位号
        /// </summary>
        public string berthCode { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string plateNumber { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string plateColor { get; set; }
        /// <summary>
        /// 车牌类型
        /// </summary>
        public string vehicleType { get; set; }
        /// <summary>
        /// 离开时间
        /// </summary>
        public string endParkingTime { get; set; }
    }
}
