using System;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Collections;
using System.Configuration;
using Sample.WinForm.Models;

namespace Sample.WinForm
{
    public partial class FormSample : Form
    {
        public FormSample()
        {
            InitializeComponent();
        }

        private static string serviceFilePath = ConfigurationManager.AppSettings["serviceFilePath"];
        private static string serviceName = ConfigurationManager.AppSettings["serviceName"];
        private ServiceInfo serviceInfo = GetServiceInfo();
        #region
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //安装服务
        private void Install_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceInfo.serviceName))
            {
                MessageBox.Show($"{serviceInfo.serviceName}服务已经安装");
                //this.UninstallService(serviceName);
            }
            else
            {
                this.InstallService(serviceInfo.serviceFilePath);
                MessageBox.Show($"{serviceInfo.serviceName}服务安装成功");
            }

        }
        //启动服务
        private void Start_Click(object sender, EventArgs e)
        {

            if (this.IsServiceExisted(serviceInfo.serviceName))
            {
                this.ServiceStart(serviceInfo.serviceName);
                MessageBox.Show($"{serviceInfo.serviceName}服务启动成功");
            }
            else
            {
                MessageBox.Show($"{serviceInfo.serviceName}服务不存在，请先安装服务");
            }
        }
        //停止服务
        private void Stop_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceInfo.serviceName))
            {
                this.ServiceStop(serviceInfo.serviceName);
                MessageBox.Show($"{serviceInfo.serviceName}服务停止成功");
            }
            else
            {
                MessageBox.Show($"{serviceInfo.serviceName}服务不存在");
            }

        }
        //卸载服务
        private void Unstall_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceInfo.serviceName))
            {
                this.ServiceStop(serviceInfo.serviceName);
                this.UninstallService(serviceInfo.serviceFilePath);
                MessageBox.Show($"{serviceInfo.serviceName}服务卸载成功");
            }
            else
            {
                MessageBox.Show($"{serviceInfo.serviceName}服务不存在");
            }
        }
        #endregion

        #region
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
        #endregion

        public static ServiceInfo GetServiceInfo()
        {
            ServiceInfo service = new ServiceInfo();
            service.serviceFilePath = $"{Application.StartupPath}\\Sample.Service.exe";
            service.serviceName = "SampleService";
            if (!string.IsNullOrEmpty(serviceFilePath))
            {
                service.serviceFilePath = serviceFilePath;
            }
            if (!string.IsNullOrEmpty(serviceName))
            {
                service.serviceName = serviceName;
            }
            return service;
        }

    }
}
