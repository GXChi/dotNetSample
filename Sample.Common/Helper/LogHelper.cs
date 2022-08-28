using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Common.Helper
{
    public class LogHelper
    {
        public static void WriteLog(string filePathParam, string fileNameParam, string Infos)
        {
            FileStream fileStream = null;
            try
            {
                string text = "";
                string text2 = DateTime.Now.Date.Year + "-" + DateTime.Now.Date.Month + "-" + DateTime.Now.Date.Day;
                text = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + filePathParam + "\\" + text2 + "\\" + fileNameParam + ".log";
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + filePathParam + "\\" + text2 + "\\"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + filePathParam + "\\" + text2 + "\\");
                }

                fileStream = ((!File.Exists(text)) ? new FileStream(text, FileMode.Create, FileAccess.Write) : new FileStream(text, FileMode.Open, FileAccess.Write));
                string text3 = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "  " + Infos + "\r\n\r\n";
                byte[] array = new byte[text3.Length * 2];
                array = Encoding.UTF8.GetBytes(text3);
                fileStream.Position = fileStream.Length;
                fileStream.Write(array, 0, array.Length);
                fileStream.Close();
            }
            catch (Exception)
            {
                fileStream?.Close();
            }
        }
    }
}
