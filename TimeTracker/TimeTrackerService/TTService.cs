using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
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
            bool serverOnline = await systemLogData.IsServerConnected();

            if (serverOnline)
            {
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
            }
            else
            {
                message = "Server is offline";
            }

            string[] text = File.ReadAllLines(@"C:\Program Files\WCT\wifiname.txt");
            if (text != null && text.Length > 0)
            {
                writeLog = text[0].Split(',').Contains(GetConnectedWifi());
            }

            if (writeLog)
            {
                var logTime = DateTime.Now;

                if (serverOnline)
                {
                    //Sync log text file with database.
                    await SyncTextFileInDB();

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

        private static async Task SyncTextFileInDB()
        {
            try
            {
                Data.SystemLogData systemLogData = new Data.SystemLogData();
                var log = await systemLogData.GetSystemLog(GetMacAddress());

                if (log != null)
                {
                    List<string> filePath = null;
                    var length = (DateTime.Now.Date - log.LogTime.Date).TotalDays;

                    filePath.Add(string.Format(@"{0:dd_MM_yy}.txt", log.LogTime));
                    if (length > 0)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            filePath.Add(string.Format(@"{0:dd_MM_yy}.txt", log.LogTime.AddDays(i + 1)));
                        }
                    }

                    foreach (var file in filePath)
                    {
                        var data = File.ReadAllLines(@"C:\Program Files\WCT\" + file);
                        if (data != null)
                        {
                            foreach (var item in data)
                            {
                                var logTime = Convert.ToDateTime(item.Split('|')[0].Trim());
                                var module = Convert.ToInt32(item.Split('|')[1].Trim());
                                var message = item.Split('|')[2].Trim();

                                if (logTime >= log.LogTime)
                                {
                                    string name = Enum.GetName(typeof(LogTypes), module);
                                    AddSystemLogModel model = new AddSystemLogModel()
                                    {
                                        MacAddress = GetMacAddress(),
                                        LogType = (LogTypes)module,
                                        Description = message,
                                        LogTime = logTime
                                    };
                                    await systemLogData.AddSystemLog(model);
                                }
                            }
                        }
                    }
                }
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
