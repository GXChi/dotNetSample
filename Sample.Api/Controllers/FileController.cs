using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sample.Web.Controllers
{
    public class FileController : ApiController
    {
        [Route("File/UploadFileBase64")]
        [HttpPost]
        public dynamic UploadFileBase64(string base64Str)
        {
            string message;
            if (string.IsNullOrEmpty(base64Str))
            {
                message = "base64数据不能为空！";
                return Json(new { code = 1, msg = message });
            }
            string path;
            try
            {
                string serverRootPath = System.AppDomain.CurrentDomain.BaseDirectory;    //程序根目录
                string filePath = $"Upload\\Image\\{DateTime.Now.ToString("yyyyMMdd")}"; //文件路径

                string fileFullPath = $"{serverRootPath}{filePath}"; //文件保存路径              
                if (!Directory.Exists(fileFullPath))
                {
                    Directory.CreateDirectory(fileFullPath);
                }

                string suffix = ""; //后缀
                string base64 = base64Str;
                if (base64Str.IndexOf(",") > -1)
                {
                    suffix = base64Str.Split(',')[0].Split(';')[0].Split('/')[1];
                    base64 = base64Str.Trim().Substring(base64Str.IndexOf(",") + 1); //将‘，’以前的多余字符串删除
                }
                if (base64Str.Split(',').Length > 1)
                {
                    suffix = base64Str.Split(',')[0].Split(';')[0].Split('/')[1];
                    base64 = base64Str.Split(',')[1];
                }

                string fileName = $"{Guid.NewGuid().ToString()}.{suffix}";       //文件名称

                WriteFile(base64, $"{fileFullPath}\\{fileName}");
                path = $"https://{Request.Headers.Host}\\{filePath}\\{fileName}"; //返回图片地址
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return Json(new { code = 1, msg = message });
            }

            var resultData = new
            {
                code = 0,
                msg = "文件保存成功！",
                path = path
                //datumId = "", 保存附件到数据库的主键id
            };
            return Json(resultData);
         
        }

        [Route("File/UploadImageBase64")]
        [HttpPost]
        public dynamic UploadImageBase64(string base64Str)
        {
            string message;
            if (string.IsNullOrEmpty(base64Str))
            {
                message = "base64数据不能为空！";
                return Json(new { code = 1, msg = message });
            }

            string path;
            string thumbnailPath;
            try
            {
                var base64 = base64Str.Split(',')[1];
                Image image = byteToImage(Convert.FromBase64String(base64));
                var suffix = GetImageSuffix(image); //后缀

                var serverRootPath = System.AppDomain.CurrentDomain.BaseDirectory;       //程序根目录

                string filePath = $"Upload\\Image\\{DateTime.Now.ToString("yyyyMMdd")}"; //文件路径
                string fileFullPath = $"{serverRootPath}{filePath}"; //文件保存路径              
                if (!Directory.Exists(fileFullPath))
                {
                    Directory.CreateDirectory(fileFullPath);
                }

                string fileName = Guid.NewGuid().ToString() + suffix;                 //文件名称
                image.Save(fileFullPath + "\\" + fileName); //保存图片

                string thumbnailFilePath = $"Upload\\ThumbnailImage\\{DateTime.Now.ToString("yyyyMMdd")}"; //文件路径
                var thumbnailFullPath = $"{serverRootPath}{thumbnailFilePath}";
                if (!Directory.Exists(thumbnailFullPath))
                {
                    Directory.CreateDirectory(thumbnailFullPath);
                }
                SaveReducedImage(image, 0.5, thumbnailFullPath + "\\" + fileName); //保存缩略图；

                path = $"https://{Request.Headers.Host}\\{filePath}\\{fileName}"; //返回图片地址
                thumbnailPath = $"https://{Request.Headers.Host}\\{thumbnailFilePath}\\{fileName}";
            }
            catch(Exception ex)
            {
                return Json(new { code = 1, msg = ex.ToString() });
            }
            return Json(new { code = 0, msg = "图片保存成功！", path = path, thumbnailPath = thumbnailPath });
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="base64bt"></param>
        /// <param name="percent">缩略图的宽度百分比 如：需要百分之80，就填0.8</param>     
        /// <param name="targetFilePath">缩略图保存的全文件名，(带路径)，参数格式：D:Images ilename.jpg</param>
        private void SaveReducedImage(Image resourceImage, double percent, string targetFilePath)
        {            
            Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            int imageWidth = Convert.ToInt32(resourceImage.Width * percent);
            int imageHeight = Convert.ToInt32(resourceImage.Width * percent);
            var ReducedImage = resourceImage.GetThumbnailImage(imageWidth, imageHeight, callb, IntPtr.Zero);
            ReducedImage.Save(targetFilePath, ImageFormat.Jpeg);
            ReducedImage.Dispose();
        }


        /// <summary>
        /// 文件转base64
        /// </summary>
        /// <returns>base64字符串</returns>
        public string FileToBase64String()
        {
            FileStream fsForRead = new FileStream("D:\\upFile\\aaaaaaaaaaa.png", FileMode.Open);//文件路径
            string base64Str = "";
            try
            {
                //读写指针移到距开头10个字节处
                fsForRead.Seek(0, SeekOrigin.Begin);
                byte[] bs = new byte[fsForRead.Length];
                int log = Convert.ToInt32(fsForRead.Length);
                //从文件中读取10个字节放到数组bs中
                fsForRead.Read(bs, 0, log);
                base64Str = Convert.ToBase64String(bs);
                return base64Str;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.ReadLine();
                return base64Str;
            }
            finally
            {
                fsForRead.Close();
            }
        }


        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="base64Str">base64字符串</param>
        /// <param name="fullPathName">完整路径和文件名</param>
        private void WriteFile(string base64Str, string fullPathName)
        {
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64Str));
            var streamArray = stream.ToArray();

            FileStream fileStream = new FileStream(fullPathName, FileMode.OpenOrCreate, FileAccess.Write);
            fileStream.Write(streamArray, 0, streamArray.Length);
            fileStream.Close();
        }

        #region 保存图片
        /// <summary>
        /// 写入图片
        /// </summary>
        /// <param name="base64Str">base64字符串</param>
        /// <param name="fullPathName">完整路径和文件名</param>
        private void WriteImage(string base64Str, string fullPathName)
        {
            var image = byteToImage(Convert.FromBase64String(base64Str));
            image.Save(fullPathName);
        }

        /// <summary>
        /// 字节数组转换成图片
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private Image byteToImage(byte[] buffer)
        {
            //buffer = Convert.FromBase64String(base64Str);
            MemoryStream stream = new MemoryStream(buffer);
            stream.Position = 0;
            Image image = Image.FromStream(stream);
            stream.Close();
            return image;
        }

        /// <summary>
        /// 获取图片后缀名
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns></returns>
        private string GetImageSuffix(Image image)
        {
            string suffix = ".jpg";
            var rawFormatGuid = image.RawFormat.Guid;
            if (rawFormatGuid == ImageFormat.Png.Guid)
            {
                suffix = ".png";
            }
            else if (rawFormatGuid == ImageFormat.Jpeg.Guid)
            {
                suffix = ".jpeg";
            }
            else if (rawFormatGuid == ImageFormat.Bmp.Guid)
            {
                suffix = ".bmp";
            }
            else if (rawFormatGuid == ImageFormat.Gif.Guid)
            {
                suffix = ".gif";
            }
            else if (rawFormatGuid == ImageFormat.Icon.Guid)
            {
                suffix = ".icon";
            }
            else if (rawFormatGuid == ImageFormat.Emf.Guid)
            {
                suffix = ".emf";
            }
            else if (rawFormatGuid == ImageFormat.Wmf.Guid)
            {
                suffix = ".wmf";
            }
            else if (rawFormatGuid == ImageFormat.Tiff.Guid)
            {
                suffix = ".tiff";
            }
            else if (rawFormatGuid == ImageFormat.Exif.Guid)
            {
                suffix = ".exif";
            }

            return suffix;

        }
        #endregion
    }
}