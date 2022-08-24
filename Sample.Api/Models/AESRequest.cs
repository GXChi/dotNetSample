using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Web.Models
{
    public class AESRequest
    {  
        public string secretKey { get; set; }
        public string privateKey { get; set; }
        public string publicKey { get; set; }
        public HeartBeat data { get; set; }
        public string vector { get; set; }

        public string data1 { get; set; }
    }

    public class HeartBeat
    {
        /// <summary>
        /// 停车场 ID
        /// </summary>
        public string park_id { get; set; }
        /// <summary>
        /// 泊位总数
        /// </summary>
        public int total_parking_number { get; set; }
        /// <summary>
        /// 空余泊位数
        /// </summary>
        public int remain_parking_number { get; set; }
        /// <summary>
        /// 开放泊位数，是指扣减掉内部保留车位数之后
        /// 剩余可以开放给社会公众的泊位数。默认情况下取泊位总数。
        /// </summary>
        public int open_parking_number { get; set; }
        /// <summary>
        /// 本地系统时间
        /// </summary>
        public string event_time { get; set; }
    }
}