using Sample.Common.Helper;
using Sample.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Sample.Web.Controllers
{
    /// <summary>
    /// AES
    /// </summary>
    public class AESController : ApiController
    {

        private readonly string vector = "0000000000000000";
        
        /// <summary>
        /// aes加密
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AES/AesEncrypt")]
        [HttpPost]
        public dynamic AesEncrypt([FromBody] AESRequest model)
        {
            //byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(model.data);
            //var result2 = AESHelper.AesEncrypt(model.data, model.secretKey, model.vector);
            //var result1 = AESHelper.AesEncrypt(model.data, model.secretKey);
            var ECBresult = AESHelper.AESEncrypt(model.data1, model.secretKey, vector);           
            var CBCresult1 = AESHelper.AesEncrypt(model.data1, model.secretKey, vector);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(model.data1);
            var result2 = AESHelper.AESEncrypt(toEncryptArray, model.secretKey, vector);           
            return new { ECBresult, CBCresult1, result2 };
        }

        /// <summary>
        /// aes解密
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AES/AesDecyptCBC")]
        [HttpPost]
        public dynamic AesDecyptCBC([FromBody] AESRequest model)
        {
            var result1 = AESHelper.AesDecrypt(model.data1, model.secretKey, vector);
         
            return new { result1};
        }

        /// <summary>
        /// aes解密
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AES/AesDecyptECB")]
        [HttpPost]
        public dynamic AesDecyptECB([FromBody] AESRequest model)
        {
            var result = AESHelper.AESDecrypt(model.data1, model.secretKey, vector);
            return new { result };
        }

        /// <summary>
        /// aes解密
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AES/AesDecypt2")]
        [HttpPost]
        public dynamic AesDecypt2([FromBody] AESRequest model)
        {
            var dataByte = Convert.FromBase64String(model.data1);
            var result = AESHelper.AESDecrypt(dataByte, model.secretKey, vector);
            return new { result };
        }


    }
}