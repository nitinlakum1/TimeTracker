using System.ComponentModel;
using System.Configuration.Install;

namespace TimeTrackerService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void SystemTime_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
