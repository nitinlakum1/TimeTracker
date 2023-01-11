using System.Management;
using System.ServiceProcess;

namespace UpdateService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ManagementObject service = new ManagementObject(new ManagementPath("Win32_Service.Name='TTService'"));
            var version = service["Description"].ToString();
        }

        protected override void OnStop()
        {

        }
    }
}
