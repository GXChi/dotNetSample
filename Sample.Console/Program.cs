using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;


namespace Sample.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var timers = new System.Timers.Timer();
            //timers += new ElapsedEventHandler(SayHellow);
            //timers.Interval = 20000;
            //timers.Enabled = true;
            //timers.AutoReset = true;


           
        }

        private static void SayHellow(object sender, ElapsedEventArgs e)
        {
            System.Console.WriteLine("1111 \r\n 1111");
        }
       
    }
}
