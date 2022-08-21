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
        public dynamic RSAEncrypt([FromBody]RSARequest model)
        {
            return RSACryption.RSAEncrypt(model.publicKey, model.data);
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
            return RSACryption.RSADecrypt(model.privateKey, model.data);
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
            string dataHash = "";
            RSACryption.GetHash(model.data, ref dataHash);

            string result = "";
            RSACryption.SignatureFormatter(model.privateKey, dataHash, ref result);
            return new { model.data, result };
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
            RSACryption.GetHash(model.data, ref dataHash);
            if (RSACryption.SignatureDeformatter(model.publicKey, dataHash, model.sign))
                return "验签成功！";
            return "验签失败！";
        }
        #endregion
    }
}