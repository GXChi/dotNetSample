using Sample.Common.Helper;
using Sample.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Sample.Web.Controllers
{
    /// <summary>
    /// RSA
    /// </summary>
    public class RSAController : ApiController
    {
        #region 密钥转换
        /// <summary>
        /// 密钥pem格式转换成xml格式
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="publickey"></param>
        /// <returns></returns>
        [Route("RSA/RSAPemToXml")]
        [HttpPost]
        public dynamic RSAPemToXml([FromBody] RSARequest model)
        {
            string xmlPrivateKey = RSAHelper.RSAPrivateKeyJava2DotNet(model.privateKey);
            string xmlPublicKey = RSAHelper.RSAPublicKeyJava2DotNet(model.publicKey);
            return new { xmlPrivateKey, xmlPublicKey };
        }

        #endregion

        #region RSACryption
        /// <summary>
        /// 创建私钥和公钥（xml格式）
        /// </summary>
        /// <returns></returns>
        [Route("RSA/CreateRSAKey")]
        [HttpPost]
        public dynamic CreateRSAKey()
        {
            string privateKeyXml;
            string publicKeyXml;
            RSACryption.RSAKey(out privateKeyXml, out publicKeyXml);
            return new { privateKeyXml, publicKeyXml };
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">待加密的数据</param>
        /// <param name="publicKeyXml">公钥</param>
        /// <returns></returns>
        [Route("RSA/RSAEncrypt")]
        [HttpPost]
        public dynamic RSAEncrypt([FromBody] RSARequest model)
        {
            return RSACryption.RSAEncrypt(model.publicKey, JsonHelper.JsonSerializer(model.data));
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">待解密的数据</param>
        /// <param name="privateKeyXml">私钥</param>
        /// <returns></returns>
        [Route("RSA/RSADecrypt")]
        [HttpPost]
        public dynamic RSADecrypt([FromBody] RSARequest model)
        {
            return RSACryption.RSADecrypt(model.privateKey, JsonHelper.JsonSerializer(model.data));
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="privateKeyXml">私钥</param>
        /// <param name="data">待签名的数据</param>
        /// <returns>签名</returns>
        [Route("RSA/SignatureFormatter")]
        [HttpPost]
        public dynamic SignatureFormatter([FromBody] RSARequest model)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("app_id", "1111");
            dictionary.Add("timestamp", "2222");
            dictionary.Add("data", "ve+DUAOwfJekg5TPhyzSqQZVYQ67QnnyLlc+qS0umOJ2/vPe63vts7VogKaQtxy7G3c5CfaEtLJ/QhNEUAXgsOQ6UM7s+se8H/Y2lcZgKWSADGQzIsVoEf+VclR20OJAxFIaQzMBjSP6lSyj5/LaEAJLSFHtIyUKDQ4sUdFtTNI=");
            //排序拼接验签数据
            var signData = RSACryption.Sort(dictionary);


            string dataHash = "";
            RSACryption.GetHash(signData, ref dataHash);

            string sign = "";
            RSACryption.SignatureFormatter(model.privateKey, dataHash, ref sign);
            //var resdata = JsonHelper.JsonSerializer(model.data);
            return new { signData, sign };
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="publicKeyXml">公钥</param>
        /// <param name="data">待验签的数据</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        [Route("RSA/SignatureDeformatter")]
        [HttpPost]
        public dynamic SignatureDeformatter([FromBody] RSARequest model)
        {
            string dataHash = "";
            RSACryption.GetHash(JsonHelper.JsonSerializer(model.data), ref dataHash);
            if (RSACryption.SignatureDeformatter(model.publicKey, dataHash, model.sign))
                return "验签成功！";
            return "验签失败！";
        }
        #endregion
    }
}