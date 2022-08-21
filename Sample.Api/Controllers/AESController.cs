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
        /// <summary>
        /// aes加密
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AES/AesEncrypt")]
        [HttpPost]
        public dynamic AesEncrypt([FromBody] AESRequest model)
        {
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(model.data);
            var result2 = AESHelper.AesEncrypt(model.data, model.secretKey, model.vector);
            var result1 = AESHelper.AesEncrypt(model.data, model.secretKey);
            return new { result1, result2 };
        }

        /// <summary>
        /// aes加密
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AES/AesDecypt")]
        [HttpPost]
        public dynamic AesDecypt([FromBody] AESRequest model)
        {
            var dataByte = Convert.FromBase64String(model.data);
            var result1 = AESHelper.AesDecypt(model.data, model.secretKey);
            var result2 = AESHelper.AesDecrypt(model.data, model.secretKey, model.vector);
            return new { result1, result2 };
        }

    }
}