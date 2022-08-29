using Sample.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogHelper.WriteLog("ServiceSample", "LogInfo", "服务启动");
        }

        protected override void OnStop()
        {
            LogHelper.WriteLog("ServiceSample", "LogInfo", "服务停止");
        }
    }
}
