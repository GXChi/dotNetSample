using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample.Service
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string serviceFilePath = $"{Application.StartupPath}\\Sample.Service.exe";
        string serviceName = "SampleService";

        private void Install_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName))
            {
                MessageBox.Show("SampleService服务已经安装");
                //this.UninstallService(serviceName);
            }
            else
            {
                this.InstallService(serviceFilePath);
                MessageBox.Show("SampleService服务安装成功");
            }
            
        }

        private void Start_Click(object sender, EventArgs e)
        {

            if (this.IsServiceExisted(serviceName))
            {
                this.ServiceStart(serviceName);
                MessageBox.Show("SampleService服务启动成功");
            }
            else
            {
                MessageBox.Show("SampleService服务不存在，请先安装服务");
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName))
            {
                this.ServiceStop(serviceName);
                MessageBox.Show("SampleService服务停止成功");
            }
            else
            {
                MessageBox.Show("SampleService服务不存在");
            }
            
        }

        private void Unstall_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName))
            {
                this.ServiceStop(serviceName);
                this.UninstallService(serviceFilePath);
                MessageBox.Show("SampleService服务卸载成功");
            }
            else
            {
                MessageBox.Show("SampleService服务不存在");
            }
        }

        //安装服务
        private void InstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                IDictionary savedState = new Hashtable();
                installer.Install(savedState);
                installer.Commit(savedState);
            }
        }

        //卸载服务
        private void UninstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                IDictionary savedState = new Hashtable();
                installer.Uninstall(savedState);
            }
        }

        //启动服务
        private void ServiceStart(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Stopped)
                {
                    control.Start();
                }
            }
        }


        //停止服务
        private void ServiceStop(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Running)
                {
                    control.Stop();
                }
            }
        }


        //判断服务是否存在
        private bool IsServiceExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController sc in services)
            {
                if (sc.ServiceName.ToLower() == serviceName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
