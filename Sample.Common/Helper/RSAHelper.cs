using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Common.Helper
{
    public class RSAHelper
    {
        public static string SignatureFormatter(string privatekey, string signStr)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(privatekey) || string.IsNullOrWhiteSpace(signStr))
                {
                    return "私钥和字符串不能为空";
                }

                //SHA256withRSA
                //string fnstr = signStr;//待签名字符串
                //1。转换私钥字符串为RSACryptoServiceProvider对象
                RSACryptoServiceProvider rsaP = LoadPrivateKey(privatekey, "PKCS8");
                byte[] data = Encoding.UTF8.GetBytes(signStr);//待签名字符串转成byte数组，UTF8
                byte[] byteSign = rsaP.SignData(data, "SHA256");//对应JAVA的RSAwithSHA256
                string sign = Convert.ToBase64String(byteSign);//签名byte数组转为BASE64字符串
                return sign;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string SignatureDeformatter(string publicKey, string singStr, string sign)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(publicKey) || string.IsNullOrWhiteSpace(singStr) || string.IsNullOrWhiteSpace(sign))
                {
                    return "公钥，签名字符串，签名 不能为空";
                }
                byte[] signature = Convert.FromBase64String(sign);//签名值转为byte数组
                //SHA256withRSA
                //string fnstr = singStr;
                //1。转换私钥字符串为RSACryptoServiceProvider对象
                RSACryptoServiceProvider rsaP = LoadPublicKey(publicKey);
                byte[] data = Encoding.UTF8.GetBytes(singStr);//待签名字符串转成byte数组，UTF8
                bool validSign = rsaP.VerifyData(data, "SHA256", signature);//对应JAVA的RSAwithSHA256

                if (validSign)
                    return "验证签名通过：" + DateTime.Now.ToString();

                else
                    return "验证签名 不通过：" + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region 加载私钥
        /// <summary>
        /// 转换私钥字符串为RSACryptoServiceProvider
        /// </summary>
        /// <param name="privateKeyStr">私钥字符串</param>
        /// <param name="keyFormat">PKCS8,PKCS1</param>
        /// <param name="signType">RSA 私钥长度1024 ,RSA2 私钥长度2048</param>
        /// <returns></returns>
        public static RSACryptoServiceProvider LoadPrivateKey(string privateKeyStr, string keyFormat)
        {
            string signType = "RSA";
            if (privateKeyStr.Length > 1024)
            {
                signType = "RSA2";
            }
            //PKCS8,PKCS1
            if (keyFormat == "PKCS1")
            {
                return LoadPrivateKeyPKCS1(privateKeyStr, signType);
            }
            else
            {
                return LoadPrivateKeyPKCS8(privateKeyStr);
            }
        }

        /// <summary>
        /// PKCS1 格式私钥转 RSACryptoServiceProvider 对象
        /// </summary>
        /// <param name="strKey">pcsk1 私钥的文本内容</param>
        /// <param name="signType">RSA 私钥长度1024 ,RSA2 私钥长度2048 </param>
        /// <returns></returns>
        public static RSACryptoServiceProvider LoadPrivateKeyPKCS1(string privateKeyPemPkcs1, string signType)
        {
            try
            {
                privateKeyPemPkcs1 = privateKeyPemPkcs1.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\r", "").Replace("\n", "").Trim();
                privateKeyPemPkcs1 = privateKeyPemPkcs1.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "").Replace("\r", "").Replace("\n", "").Trim();

                byte[] data = null;
                //读取带

                data = Convert.FromBase64String(privateKeyPemPkcs1);


                RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(data, signType);
                return rsa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey, string signType)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);


                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                int bitLen = 1024;
                if ("RSA2".Equals(signType))
                {
                    bitLen = 2048;
                }

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen, CspParameters);
                //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception ex)
            {
                throw ex;
                // return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)        //expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();    // data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {    //remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);        //last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        /// <summary>
        /// PKCS8 文本转RSACryptoServiceProvider 对象
        /// </summary>
        /// <param name="privateKeyPemPkcs8"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider LoadPrivateKeyPKCS8(string privateKeyPemPkcs8)
        {

            try
            {
                //PKCS8是“BEGIN PRIVATE KEY”
                privateKeyPemPkcs8 = privateKeyPemPkcs8.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\r", "").Replace("\n", "").Trim();
                privateKeyPemPkcs8 = privateKeyPemPkcs8.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "").Replace("\r", "").Replace("\n", "").Trim();

                //pkcs8 文本先转为 .NET XML 私钥字符串
                string privateKeyXml = RSAPrivateKeyJava2DotNet(privateKeyPemPkcs8);

                RSACryptoServiceProvider publicRsa = new RSACryptoServiceProvider();
                publicRsa.FromXmlString(privateKeyXml);
                return publicRsa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// PKCS8 私钥文本 转 .NET XML 私钥文本
        /// </summary>
        /// <param name="privateKeyPemPkcs8"></param>
        /// <returns></returns>
        public static string RSAPrivateKeyJava2DotNet(string privateKeyPemPkcs8)
        {
            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKeyPemPkcs8));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
            Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        #endregion


        /// <summary>
        /// 加载公钥证书
        /// </summary>
        /// <param name="publicKeyCert">公钥证书文本内容</param>
        /// <returns></returns>
        public static RSACryptoServiceProvider LoadPublicCert(string publicKeyCert)
        {

            publicKeyCert = publicKeyCert.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").Replace("\r", "").Replace("\n", "").Trim();

            byte[] bytesCerContent = Convert.FromBase64String(publicKeyCert);
            X509Certificate2 x509 = new X509Certificate2(bytesCerContent);
            RSACryptoServiceProvider rsaPub = (RSACryptoServiceProvider)x509.PublicKey.Key;
            return rsaPub;

        }


        /// <summary>
        /// pem 公钥文本 转  .NET RSACryptoServiceProvider。
        /// </summary>
        /// <param name="publicKeyPem"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider LoadPublicKey(string publicKeyPem)
        {

            publicKeyPem = publicKeyPem.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\r", "").Replace("\n", "").Trim();

            //pem 公钥文本 转  .NET XML 公钥文本。
            string publicKeyXml = RSAPublicKeyJava2DotNet(publicKeyPem);

            RSACryptoServiceProvider publicRsa = new RSACryptoServiceProvider();
            publicRsa.FromXmlString(publicKeyXml);
            return publicRsa;


        }

        /// <summary>
        /// pem 公钥文本 转  .NET XML 公钥文本。
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string RSAPublicKeyJava2DotNet(string publicKey)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }

    }

    public class RSACryption
    {
        #region RSA 加密解密

        #region RSA 的密钥产生
        /// <summary>
        /// RSA产生密钥
        /// </summary>
        /// <param name="xmlKeys">私钥</param>
        /// <param name="xmlPublicKey">公钥</param>
        public static void RSAKey(out string xmlKeys, out string xmlPublicKey)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                xmlKeys = rsa.ToXmlString(true);
                xmlPublicKey = rsa.ToXmlString(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region RSA加密函数
        //############################################################################## 
        //RSA 方式加密 
        //KEY必须是XML的形式,返回的是字符串 
        //该加密方式有长度限制的！
        //############################################################################## 

        /// <summary>
        /// RSA的加密函数
        /// </summary>
        /// <param name="xmlPublicKey">公钥</param>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns></returns>
        public static string RSAEncrypt(string xmlPublicKey, string encryptString)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] CypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                PlainTextBArray = (new UnicodeEncoding()).GetBytes(encryptString);
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// RSA的加密函数 
        /// </summary>
        /// <param name="xmlPublicKey">公钥</param>
        /// <param name="EncryptString">待加密的字节数组</param>
        /// <returns></returns>
        public static string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
        {
            try
            {
                byte[] CypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                CypherTextBArray = rsa.Encrypt(EncryptString, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region RSA的解密函数        
        /// <summary>
        /// RSA的解密函数
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="decryptString">待解密的字符串</param>
        /// <returns></returns>
        public static string RSADecrypt(string xmlPrivateKey, string decryptString)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] DypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                PlainTextBArray = Convert.FromBase64String(decryptString);
                DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
                Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// RSA的解密函数 
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="DecryptString">待解密的字节数组</param>
        /// <returns></returns>
        public static string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
        {
            try
            {
                byte[] DypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                DypherTextBArray = rsa.Decrypt(DecryptString, false);
                Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region RSA数字签名

        #region 获取Hash描述表
        /// <summary>
        /// 获取Hash描述
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="encoding">Encoding（UTF-8，GB2312）</param>
        /// <param name="hashName">hash名称(MD5,SHA256)</param>
        /// <returns>Base64Hash描述</returns>
        public static string GetHash(string data, string hashName = "MD5", string encoding = "UTF-8")
        {
            var buffer = Encoding.GetEncoding(encoding).GetBytes(data);
            HashAlgorithm hashAlgorithm = HashAlgorithm.Create(hashName);
            var hashData = hashAlgorithm.ComputeHash(buffer);
            return Convert.ToBase64String(hashData);
        }

        /// <summary>
        /// 获取SHA256Hash描述表
        /// </summary>
        /// <param name="strSource">待签名的字符串</param>
        /// <param name="strHashData">Hash描述</param>
        /// <returns></returns>
        public static string GetSHA256Hash(string data, string encoding = "UTF-8")
        {
            try
            {
                byte[] bt = Encoding.GetEncoding(encoding).GetBytes(data);
                var sha256 = new SHA256CryptoServiceProvider();
                byte[] rgbHash = sha256.ComputeHash(bt);
                return Convert.ToBase64String(rgbHash);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取MD5Hash描述表
        /// </summary>
        /// <param name="strSource">待签名的字符串</param>
        /// <param name="strHashData">Hash描述</param>
        /// <returns></returns>
        public static string GetMD5Hash(string data, string encoding = "UTF-8")
        {
            try
            {
                var buffer = Encoding.GetEncoding(encoding).GetBytes(data);
                HashAlgorithm hashAlgorithm = HashAlgorithm.Create("MD5");
                var hashData = hashAlgorithm.ComputeHash(buffer);
                return Convert.ToBase64String(hashData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取Hash描述表
        /// </summary>
        /// <param name="objFile">待签名的文件</param>
        /// <param name="hashName">hash名称(MD5,SHA256)</param>
        /// <returns></returns>
        public static string GetMD5Hash(System.IO.FileStream objFile, string hashName = "MD5")
        {
            try
            {
                //从文件中取得Hash描述 
                byte[] HashData;
                HashAlgorithm hashAlgorithm = System.Security.Cryptography.HashAlgorithm.Create(hashName);
                HashData = hashAlgorithm.ComputeHash(objFile);
                objFile.Close();
                return Convert.ToBase64String(HashData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region RSA签名

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="HashbyteSignature">待签名Hash描述</param>
        /// <param name="EncryptedSignatureData">签名后的结果</param>
        /// <returns></returns>
        public static string CreateSignature(string xmlPrivateKey, string HashData, string hashName = "MD5")
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(xmlPrivateKey);
                RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
                RSAFormatter.SetHashAlgorithm(hashName);
                //执行签名 
                var dataBase64 = Convert.FromBase64String(HashData);
                var rgbHash = RSAFormatter.CreateSignature(dataBase64);
                return Convert.ToBase64String(rgbHash);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="strKeyPrivate">私钥</param>
        /// <param name="HashbyteSignature">待签名Hash描述</param>
        /// <param name="EncryptedSignatureData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureFormatter(string strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="strKeyPrivate">私钥</param>
        /// <param name="HashbyteSignature">待签名Hash描述</param>
        /// <param name="m_strEncryptedSignatureData">签名后的结果</param>
        /// <returns></returns>
        public bool SignatureFormatter(string strKeyPrivate, byte[] HashbyteSignature, ref string strEncryptedSignatureData)
        {
            try
            {
                byte[] EncryptedSignatureData;
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
                strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="strKeyPrivate">私钥</param>
        /// <param name="strHashbyteSignature">待签名Hash描述</param>
        /// <param name="EncryptedSignatureData">签名后的结果</param>
        /// <returns></returns>
        public bool SignatureFormatter(string strKeyPrivate, string strHashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            try
            {
                byte[] HashbyteSignature;

                HashbyteSignature = Convert.FromBase64String(strHashbyteSignature);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="strKeyPrivate">私钥</param>
        /// <param name="strHashbyteSignature">待签名Hash描述</param>
        /// <param name="strEncryptedSignatureData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureFormatter(string strKeyPrivate, string strHashbyteSignature, ref string strEncryptedSignatureData, string HashType = "MD5")
        {
            try
            {
                byte[] HashbyteSignature;
                byte[] EncryptedSignatureData;
                HashbyteSignature = Convert.FromBase64String(strHashbyteSignature);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5 
                RSAFormatter.SetHashAlgorithm(HashType);
                //执行签名 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
                strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region RSA 签名验证
        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="strKeyPublic">公钥</param>
        /// <param name="HashbyteDeformatter">Hash描述</param>
        /// <param name="DeformatterData">签名后的结果</param>
        /// <returns></returns>
        public bool SignatureDeformatter(string strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5 
                RSADeformatter.SetHashAlgorithm("MD5");
                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="strKeyPublic">公钥</param>
        /// <param name="strHashbyteDeformatter">Hash描述</param>
        /// <param name="DeformatterData">签名后的结果</param>
        /// <returns></returns>
        public bool SignatureDeformatter(string strKeyPublic, string strHashbyteDeformatter, byte[] DeformatterData)
        {
            try
            {
                byte[] HashbyteDeformatter;
                HashbyteDeformatter = Convert.FromBase64String(strHashbyteDeformatter);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5 
                RSADeformatter.SetHashAlgorithm("MD5");
                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="strKeyPublic">公钥</param>
        /// <param name="HashbyteDeformatter">Hash描述</param>
        /// <param name="strDeformatterData">签名后的结果</param>
        /// <returns></returns>
        public bool SignatureDeformatter(string strKeyPublic, byte[] HashbyteDeformatter, string strDeformatterData)
        {
            try
            {
                byte[] DeformatterData;
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5 
                RSADeformatter.SetHashAlgorithm("MD5");
                DeformatterData = Convert.FromBase64String(strDeformatterData);
                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="strKeyPublic">公钥</param>
        /// <param name="strHashbyteDeformatter">Hash描述</param>
        /// <param name="strDeformatterData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureDeformatter(string strKeyPublic, string strHashbyteDeformatter, string strDeformatterData)
        {
            try
            {
                byte[] DeformatterData;
                byte[] HashbyteDeformatter;
                HashbyteDeformatter = Convert.FromBase64String(strHashbyteDeformatter);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5 
                RSADeformatter.SetHashAlgorithm("MD5");
                DeformatterData = Convert.FromBase64String(strDeformatterData);
                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region 排序
        public static string Sort(Dictionary<string, string> dictionary)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(dictionary);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;
        }
        #endregion
    }
}
