using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Sample.Common.Helper
{
    public class AESHelper
    {

        /// <summary>
        /// AES加密 (128-ECB加密模式)
        /// </summary>
        /// <param name="toEncrypt">内容</param>
        /// <param name="secretKey">秘钥</param>
        /// <returns></returns>
        public static string AESEncrypt(string toEncrypt, string secretKey)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] keyArray = Convert.FromBase64String(secretKey);
            //byte[] outputb = Convert.FromBase64String("Q3xNHuj9JJu1EGQnJnzIDA==");

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES解密(128-ECB加密模式)
        /// </summary>
        /// <param name="toDecrypt">密文</param>
        /// <param name="secretKey">秘钥(Base64String)</param>
        /// <returns></returns>
        public static string AESDecrypt(string toDecrypt, string secretKey)
        {
            try
            {
                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
                byte[] keyArray = Convert.FromBase64String(secretKey); //128bit

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray; //获取或设置对称算法的密钥
                rDel.Mode = CipherMode.ECB; //获取或设置对称算法的运算模式，必须设置为ECB  
                rDel.Padding = PaddingMode.PKCS7; //获取或设置对称算法中使用的填充模式，必须设置为PKCS7  
                ICryptoTransform cTransform = rDel.CreateDecryptor(); //用当前的 Key 属性和初始化向量 (IV) 建立对称解密器对象

                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                return null;
            }
        }





        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="Data">被加密的明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>密文</returns>
        public static Byte[] AESEncrypt(Byte[] Data, String Key, String Vector)
        {
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            Byte[] bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);

            Byte[] Cryptograph = null; // 加密后的密文

            Rijndael Aes = Rijndael.Create();
            try
            {
                // 开辟一块内存流
                using (MemoryStream Memory = new MemoryStream())
                {
                    // 把内存流对象包装成加密流对象
                    using (CryptoStream Encryptor = new CryptoStream(Memory,
                     Aes.CreateEncryptor(bKey, bVector),
                     CryptoStreamMode.Write))
                    {
                        // 明文数据写入加密流
                        Encryptor.Write(Data, 0, Data.Length);
                        Encryptor.FlushFinalBlock();

                        Cryptograph = Memory.ToArray();
                    }
                }
            }
            catch
            {
                Cryptograph = null;
            }

            return Cryptograph;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="Data">被解密的密文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>明文</returns>
        public static Byte[] AESDecrypt(Byte[] Data, String Key, String Vector)
        {
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            Byte[] bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);

            Byte[] original = null; // 解密后的明文

            Rijndael Aes = Rijndael.Create();
            try
            {
                // 开辟一块内存流，存储密文
                using (MemoryStream Memory = new MemoryStream(Data))
                {
                    // 把内存流对象包装成加密流对象
                    using (CryptoStream Decryptor = new CryptoStream(Memory,
                    Aes.CreateDecryptor(bKey, bVector),
                    CryptoStreamMode.Read))
                    {
                        // 明文存储区
                        using (MemoryStream originalMemory = new MemoryStream())
                        {
                            Byte[] Buffer = new Byte[1024];
                            Int32 readBytes = 0;
                            while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                originalMemory.Write(Buffer, 0, readBytes);
                            }

                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                original = null;
            }

            return original;
        }



        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesEncrypt(string toEncrypt, string key, string iv)
        {
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);//注意编码格式(utf8编码 UTF8Encoding)
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            //rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateEncryptor();//加密
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesDecrypt(string toDecrypt, string key, string iv)
        {
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);//注意编码格式(utf8编码 UTF8Encoding)
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            //rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateDecryptor();//解密
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="clearTxt"></param>
        /// <returns></returns>
        public static string AesEncrypt(string clearTxt, string secretKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

            using (RijndaelManaged cipher = new RijndaelManaged())
            {
                cipher.Mode = CipherMode.CBC;
                cipher.Padding = PaddingMode.PKCS7;
                cipher.KeySize = 128;
                cipher.BlockSize = 128;
                cipher.Key = keyBytes;
                cipher.IV = keyBytes;

                byte[] valueBytes = Encoding.UTF8.GetBytes(clearTxt);

                byte[] encrypted;
                using (ICryptoTransform encryptor = cipher.CreateEncryptor())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream writer = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encrypted = ms.ToArray();

                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < encrypted.Length; i++)
                                sb.Append(Convert.ToString(encrypted[i], 16).PadLeft(2, '0'));
                            return sb.ToString().ToUpperInvariant();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public static string AesDecypt(string encrypted, string secretKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

            using (RijndaelManaged cipher = new RijndaelManaged())
            {
                cipher.Mode = CipherMode.CBC;
                cipher.Padding = PaddingMode.PKCS7;
                cipher.KeySize = 128;
                cipher.BlockSize = 128;
                cipher.Key = keyBytes;
                cipher.IV = keyBytes;

                List<byte> lstBytes = new List<byte>();
                for (int i = 0; i < encrypted.Length; i += 2)
                    lstBytes.Add(Convert.ToByte(encrypted.Substring(i, 2), 16));

                using (ICryptoTransform decryptor = cipher.CreateDecryptor())
                {
                    using (MemoryStream msDecrypt = new MemoryStream(lstBytes.ToArray()))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

    }
}
