using System;
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
                    //Delete Install_Start.bat file from the 'TimeTrackerService' folder.
                    if (File.Exists(@"C:\TimeTrackerService\Install_Start.bat"))
                    {
                        File.Delete(@"C:\TimeTrackerService\Install_Start.bat");
                    }
                }
                catch { }

                try
                {
                    string version = "1.0.1";
                    try
                    {
                        ManagementObject managementObject = new ManagementObject(new ManagementPath("Win32_Service.Name='TTService'"));

                        version = managementObject["Description"].ToString();
                    }
                    catch
                    {
                        version = "1.0.1";
                        WriteTextFile("While getting Description", "No", 0);
                    }

                    var updateService = await systemLogData.GetUpdateService("TTService");

                    if (updateService != null
                        && updateService.Version != version)
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
                        ProcessStartInfo procStartInfo = new ProcessStartInfo(@"C:\TimeTrackerService\Install_Start.bat")
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

                        //Delete TimeTrackerService.zip file from the 'TimeTrackerService' folder.
                        if (File.Exists(@"C:\TimeTrackerService\TimeTrackerService.zip"))
                        {
                            File.Delete(@"C:\TimeTrackerService\TimeTrackerService.zip");
                        }
                    }

                    //5 Minutes
                    Thread.Sleep(1000 * 60 * 5);
                }
                catch (Exception ex)
                {
                    WriteTextFile("OnStart", ex.Message, 0);
                }
            }
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

        private static void WriteTextFile(string methodName, string message, int lineNumber)
        {
            try
            {
                var logTime = DateTime.Now;
                string path = @"C:\Program Files\WCT\TTUpdate\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, string.Format(@"Error_{0:dd_MM_yy}.txt", logTime));

                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sWriter = new StreamWriter(fs);
                sWriter.BaseStream.Seek(0, SeekOrigin.End);
                sWriter.WriteLine($"{logTime:dd-MM-yy hh:mm:ss:fff tt} | Line Number: {lineNumber} | {methodName} | {message}");
                sWriter.Flush();
                sWriter.Close();
            }
            catch { }
        }
        #endregion
    }
}
