using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Console.Business
{
    public class ResponseModel
    { 
        public int Code { get; set; }
        public string Msg { get; set; }
        public string Data { get; set; }
    }
    public class ThreadHelper
    {
        //线程安全队列
        private static ConcurrentQueue<ResponseModel> queue = new ConcurrentQueue<ResponseModel>();
        public static void MultitDataProcessing()
        
        {
            var startTime = DateTime.Now;
            System.Console.WriteLine($"开始，时间：{startTime}；");
            int threadCount = 100; //10个线程
            for (int i = 0; i < threadCount; i++)
            {
                string fileName = $"task{i}.txt";
                Task.Factory.StartNew(() =>
                {
                    var sb = new StringBuilder();
                    int j = 0;

                    while (queue.TryDequeue(out ResponseModel model))
                    {
                        if (model != null)
                            sb.AppendLine($"==>Code={model.Code},Msg={model.Msg},Data={model.Data}");
                        //System.Console.WriteLine($"每100条输出一次控制台，并暂停100毫秒, 第{model.Code}次文件：{fileName}");
                        if (j % 100 == 0 || (queue.Count.Equals(0) && j < 100))
                        {
                            System.Console.WriteLine($"每100条输出一次控制台，并暂停100毫秒, 第{i}次文件：{fileName}");
                            System.Console.WriteLine(sb.ToString());
                            sb = new StringBuilder();
                            Thread.Sleep(100);
                        }
                    }
                    j++;
                });
                System.Console.WriteLine($"结束：{DateTime.Now}；用时：{DateTime.Now - startTime}");
            }
        }

        public static void SetData()
        {
            var startTime = DateTime.Now;
            System.Console.WriteLine($"开始数据设置，时间：{startTime}；");
            for (int i = 0; i < 10000000; i++)
            {
                var model = new ResponseModel { Code = i, Msg = $"{i + 1}次循环", Data = $"产生随机数：{new Random().Next(1000, 10000000)}" };
                queue.Enqueue(model);
                //Thread.Sleep(1);
            }
            System.Console.WriteLine($"10000000条数据设置完毕！时间：{ DateTime.Now}；用时：{DateTime.Now - startTime}");
        }
    }
}
