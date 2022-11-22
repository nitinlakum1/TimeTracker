using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using TimeTrackerService.DataModel;
using static TimeTrackerService.Enums;

namespace TimeTrackerService
{
    public partial class TTService : ServiceBase
    {
        #region Declaration
        #endregion

        #region Const
        public TTService()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        protected override void OnStart(string[] args)
        {
            SetLog("Service is Start", LogTypes.Other);
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    SetLog("System Log On", LogTypes.SystemLogOn);
                    break;
                case SessionChangeReason.SessionLogoff:
                    SetLog("System Log Off", LogTypes.SystemLogOff);
                    break;
                //case SessionChangeReason.RemoteConnect:
                //    SetLog("System Remote Connect");
                //    break;
                //case SessionChangeReason.RemoteDisconnect:
                //    SetLog("System Remote Disconnect");
                //    break;
                case SessionChangeReason.SessionLock:
                    SetLog("System Locked", LogTypes.SystemLock);
                    break;
                case SessionChangeReason.SessionUnlock:
                    SetLog("System Unlocked", LogTypes.SystemUnlock);
                    break;
                default:
                    break;
            }
        }

        protected override void OnShutdown()
        {
            SetLog("System Shutdown", LogTypes.Other);
        }

        protected override void OnStop()
        {
            SetLog("Service is Stopped", LogTypes.Other);
        }
        #endregion

        #region Private Method

        private static async void SetLog(string message, LogTypes module)
        {
            bool writeLog = false;
            Data.SystemLogData systemLogData = new Data.SystemLogData();

            try
            {
                var settings = await systemLogData.GetSettings();
                if (settings != null && settings.Any())
                {
                    string wifiName = settings.FirstOrDefault(a => a.Key.Equals(AppSettings.SYSTEM_WIFI_NAME)).Value.Trim();

                    string path = @"C:\Program Files\WCT\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, "wifiname.txt");

                    if (File.Exists(path))
                    { File.Delete(path); }

                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter sWriter = new StreamWriter(fs);
                    sWriter.BaseStream.Seek(0, SeekOrigin.End);
                    sWriter.WriteLine(wifiName);
                    sWriter.Flush();
                    sWriter.Close();
                }
            }
            catch { }

            string[] text = File.ReadAllLines(@"C:\Program Files\WCT\wifiname.txt");
            if(text != null && text.Length > 0)
            {
                writeLog = text[0].Split(',').Contains(GetConnectedWifi());
            }

            if (writeLog)
            {
                var logTime = DateTime.Now;
                try
                {
                    var macAddress = GetMacAddress();

                    AddSystemLogModel model = new AddSystemLogModel()
                    {
                        MacAddress = macAddress,
                        LogType = module,
                        Description = message,
                        LogTime = logTime
                    };
                    await systemLogData.AddSystemLog(model);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

                try
                {
                    string path = @"C:\Program Files\WCT\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, string.Format(@"{0:dd_MM_yy}.txt", logTime));

                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter sWriter = new StreamWriter(fs);
                    sWriter.BaseStream.Seek(0, SeekOrigin.End);
                    sWriter.WriteLine($"{logTime:dd-MM-yy hh:mm:ss tt} | {(int)module} | {message}");
                    sWriter.Flush();
                    sWriter.Close();
                }
                catch { }
            }
        }

        private static string SyncTextFileInDB()
        {
            try
            {


                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                    .Where(a => a.NetworkInterfaceType.ToString() == NetworkInterfaceType.Ethernet.ToString()
                           && a.GetType().Name == "SystemNetworkInterface")
                    .Select(a => a.GetPhysicalAddress().ToString())
                    .FirstOrDefault();
        }

        private static string GetConnectedWifi()
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "netsh.exe";
                p.StartInfo.Arguments = "wlan show interfaces";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();

                string s = p.StandardOutput.ReadToEnd();
                string s1 = s.Substring(s.IndexOf("SSID"));
                s1 = s1.Substring(s1.IndexOf(":"));
                s1 = s1.Substring(2, s1.IndexOf("\n")).Trim();

                p.WaitForExit();

                return s1;
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}
