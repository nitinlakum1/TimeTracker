using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Management;
using System.Net;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace UpdateService
{
    public partial class Service1 : ServiceBase
    {
        private static Data.UpdateServiceData systemLogData;

        public Service1()
        {
            InitializeComponent();
            systemLogData = new Data.UpdateServiceData();
        }

        #region Events
        protected async override void OnStart(string[] args)
        {
            while (true)
            {
                try
                {
                    string version = "";
                    try
                    {
                        ManagementObject managementObject = new ManagementObject(new ManagementPath("Win32_Service.Name='TTService'"));

                        version = managementObject["Description"].ToString();
                    }
                    catch { }

                    var updateService = await systemLogData.GetUpdateService("TTService");

                    if (string.IsNullOrWhiteSpace(version)
                        || (updateService != null
                            && !updateService.Version.Equals(version)))
                    {
                        //Stop TTService
                        await ExecuteCommand("sc stop \"TTService\"");

                        //Delete TTService
                        await ExecuteCommand("sc delete \"TTService\"");

                        //Delete all files from the 'TimeTrackerService' folder.
                        if (Directory.Exists(@"C:\TimeTrackerService"))
                        {
                            DirectoryInfo di = new DirectoryInfo(@"C:\TimeTrackerService\");
                            foreach (FileInfo file in di.GetFiles())
                            {
                                file.Delete();
                            }
                        }

                        //Download the TTService from the server.
                        WebClient webClient = new WebClient();
                        {
                            webClient.DownloadFile("http://103.252.109.188:85/TimeTrackerService/TimeTrackerService.zip", @"C:\TimeTrackerService\TimeTrackerService.zip");
                        }

                        string zipPath = @"C:\TimeTrackerService\TimeTrackerService.zip";
                        string extractPath = @"C:\TimeTrackerService\";
                        ZipFile.ExtractToDirectory(zipPath, extractPath);

                        //Install TTService
                        ProcessStartInfo procStartInfo = new ProcessStartInfo(@"C:\TimeTrackerService\Install.bat")
                        {
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            Verb = "runas"
                        };

                        Process proc = new Process
                        {
                            StartInfo = procStartInfo,
                        };

                        proc.Start();

                        //Start TTService
                        await ExecuteCommand("sc start \"TTService\"");
                    }
                    Thread.Sleep(5000);
                }
                catch { }
            }

            //while (true)
            //{
            //    string version = "";
            //    try
            //    {
            //        ManagementObject managementObject = new ManagementObject(new ManagementPath("Win32_Service.Name='TTService'"));

            //        version = managementObject["Description"].ToString();
            //    }
            //    catch { }

            //    var updateService = await systemLogData.GetUpdateService("TTService");

            //    if (string.IsNullOrWhiteSpace(version)
            //        || (updateService != null
            //            && !updateService.Version.Equals(version)))
            //    {
            //        try
            //        {
            //            //Stop TTService
            //            await ExecuteCommand("sc stop \"TTService\"");
            //            await ExecuteCommand("sc delete \"TTService\"");
            //            //ServiceController serviceController = new ServiceController("TTService");
            //            //if (serviceController.Status.Equals(ServiceControllerStatus.Running))
            //            //{
            //            //    serviceController.Stop();
            //            //}
            //            //TODO: Delete TTService

            //            //Delete all files from the 'TimeTrackerService' folder.
            //            if (Directory.Exists(@"C:\TimeTrackerService"))
            //            {
            //                DirectoryInfo di = new DirectoryInfo(@"C:\TimeTrackerService\");
            //                foreach (FileInfo file in di.GetFiles())
            //                {
            //                    file.Delete();
            //                }
            //            }

            //            //Download the TTService from the server.
            //            WebClient webClient = new WebClient();
            //            {
            //                webClient.DownloadFile("http://103.252.109.188:85/TimeTrackerService/TimeTrackerService.zip", @"C:\TimeTrackerService\TimeTrackerService.zip");
            //            }

            //            ZipFile.ExtractToDirectory(@"C:\TimeTrackerService\TimeTrackerService.zip", @"C:\TimeTrackerService\");

            //            //Start TTService
            //            //serviceController.Start();
            //            await ExecuteCommand("\"C:\\Windows\\Microsoft.NET\\Framework\v4.0.30319\\installutil.exe\" \"C:\\TimeTrackerService\\TimeTrackerService.exe\"");
            //            await ExecuteCommand("sc start \"TTService\"");
            //        }
            //        catch { }

            //        try
            //        {

            //        }
            //        catch { }
            //    }
            //    Thread.Sleep(5000);
            //}
        }

        protected override void OnStop()
        {

        }
        #endregion

        #region PrivateMethod
        private static async Task ExecuteCommand(object command)
        {
            try
            {
                await Task.Run(() =>
                {
                    // create the ProcessStartInfo using "cmd" as the program to be run,
                    // and "/c " as the parameters.
                    // Incidentally, /c tells cmd that we want it to execute the command that follows,
                    // and then exit.
                    ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command)
                    {
                        // The following commands are needed to redirect the standard output.
                        // This means that it will be redirected to the Process.StandardOutput StreamReader.
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        // Do not create the black window.
                        CreateNoWindow = true,
                        Verb = "runas"
                    };

                    // Now we create a process, assign its ProcessStartInfo and start it
                    Process proc = new Process
                    {
                        StartInfo = procStartInfo,
                    };

                    proc.Start();
                });
            }
            catch { }
        }
        #endregion
    }
}
