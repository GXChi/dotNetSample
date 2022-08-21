using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Common.Helper
{
    public class RSA2Helper
    {
        #region RSA2Encrypt 加密
        private const string PrivateKey = "";
        private static AsymmetricKeyParameter GetPrivateKeyParameter(string s)
        {
            s = s.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            byte[] privateInfoByte = Convert.FromBase64String(s);
            AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(privateInfoByte);
            return priKey;
        }

        private static string EncryptByPrivateKey(string s, string key)
        {
            //非对称加密算法，加解密用
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());
            engine.Init(true, GetPrivateKeyParameter(key));
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(s);
            var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
            return Convert.ToBase64String(ResultData);
        }

        /// <summary>
        /// 生成激活码
        /// </summary>
        /// <param name="machCode">机器码</param>
        /// <returns></returns>
        public static string CreateUseCode(string machCode)
        {
            try
            {
                if (string.IsNullOrEmpty(machCode))
                {
                    return "请输入正确的机器码!";
                }
                return EncryptByPrivateKey(machCode, PrivateKey/*RSAKey.CloudMusicPrivateKey*/); // RSAKey.CloudMusicPrivateKey 为私钥
            }
            catch
            {
                return "生成失败。";
            }
        }

        /// <summary>
        /// 将字符串进行base64转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ToBase64String(string str)
        {
            return Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(str));
        }
        #endregion

        #region RSA2Decrypt 解密

        private const string PublicKey = "";
        private static AsymmetricKeyParameter GetPublicKeyParameter(string publicKdy)
        {
            publicKdy = publicKdy.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            byte[] publicInfoByte = Convert.FromBase64String(publicKdy);
            Asn1Object pubKeyObj = Asn1Object.FromByteArray(publicInfoByte);//这里也可以从流中读取，从本地导入   
            AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(publicInfoByte);
            return pubKey;
        }

        private static string DecryptByPublicKey(string s, string publicKdy)
        {
            s = s.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());
            //解密  
            try
            {
                engine.Init(false, GetPublicKeyParameter(publicKdy));
                byte[] byteData = Convert.FromBase64String(s);
                var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return System.Text.Encoding.UTF8.GetString(ResultData);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 验证激活码
        /// </summary>
        /// <param name="machCode">机器码</param>
        /// <param name="useCode">激活码</param>
        /// <returns></returns>
        public static bool Verifty(string machCode, string useCode)
        {
            try
            {
                if (string.IsNullOrEmpty(machCode))
                {
                    return false;
                }
                string enCode = DecryptByPublicKey(useCode, PublicKey);
                if (string.IsNullOrEmpty(enCode))
                {
                    return false;
                }
                if (machCode.Equals(enCode))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将字符串进行md5加密
        /// </summary>
        /// <param name="str">返回16位字符串</param>
        /// <returns></returns>
        public static string GetMD5Lenth16(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
            t2 = t2.Replace("-", "");
            md5.Dispose();
            return t2;
        }
        #endregion

        #region RSA2签名

        #endregion 

        #region RSA2验签

        #endregion 
    }
}