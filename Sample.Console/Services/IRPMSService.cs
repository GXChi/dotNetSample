using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Console.Services
{
    public partial class IRPMSService : ServiceBase
    {
        public IRPMSService()
        {
            InitializeComponent();
        }
        /// <summary>
        /// work thread 
        /// </summary>
        private WorkingThread workingThread;
        protected override void OnStart(string[] args)
        {
            ///initialization  data
            InitializationConfig.Initialization();
            /// running work thread
            workingThread = new WorkingThread();
            workingThread.Running();
        }

        protected override void OnStop()
        {
            /// stop work thread
            workingThread.Stop();
        }
    }
}
