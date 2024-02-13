using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace VeriketApplicationTest
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.serviceInstaller1.AfterInstall += ServiceInstaller_AfterInstall;
        }

        private void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(_serviceName))
            {
                sc.Start();
            }
        }
    }
}
