using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Threading.Tasks;
using TimeTrackerService.DataModel;
using static TimeTrackerService.Enums;

namespace TimeTrackerService
{
    public partial class TTService : ServiceBase
    {
        #region Declaration
        private static Data.SystemLogData systemLogData;
        #endregion

        #region Const
        public TTService()
        {
            InitializeComponent();
            systemLogData = new Data.SystemLogData();
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
            try
            {
                bool writeLog = false;
                bool serverOnline = await systemLogData.IsServerConnected();
                if (serverOnline)
                {
                    //Get settings from database then write in setting.txt file.
                    await WriteSetting();
                }

                #region Get setting from settings.json
                if (File.Exists(@"C:\Program Files\WCT\settings.json"))
                {
                    List<SettingModel> settings = null;
                    using (StreamReader r = new StreamReader(@"C:\Program Files\WCT\settings.json"))
                    {
                        string json = r.ReadToEnd();
                        settings = JsonConvert.DeserializeObject<List<SettingModel>>(json);
                    }

                    if (settings != null && settings.Any())
                    {
                        string wifiName = settings
                            .FirstOrDefault(a => a.Key.Equals(AppSettings.SYSTEM_WIFI_NAME))
                            .Value;
                        writeLog = wifiName.Split(',').Contains(GetConnectedWifi());
                    }
                }
                #endregion

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
                            WriteTextFile("AddSystemLog", ex.Message);
                        }
                    }

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
                else
                {
                    WriteTextFile(nameof(SetLog), "Wi-Fi is not configured.");
                }
            }
            catch (Exception ex)
            {
                WriteTextFile(nameof(SetLog), ex.Message);
            }
        }

        private static async Task WriteSetting()
        {
            try
            {
                var settings = await systemLogData.GetSettings();
                if (settings != null && settings.Any())
                {
                    var wsSettings = settings
                        .Where(a => a.Key.Equals(AppSettings.SYSTEM_WIFI_NAME)
                               || a.Key.Equals(AppSettings.SYSTEM_LOG_END_TIMING)
                               || a.Key.Equals(AppSettings.SYSTEM_LOG_START_TIMING))
                        .ToList();

                    string path = @"C:\Program Files\WCT\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, "settings.json");

                    if (File.Exists(path))
                    { File.Delete(path); }

                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter sWriter = new StreamWriter(fs);
                    sWriter.BaseStream.Seek(0, SeekOrigin.End);
                    sWriter.WriteLine(JsonConvert.SerializeObject(wsSettings));
                    sWriter.Flush();
                    sWriter.Close();
                }
            }
            catch (Exception ex)
            {
                WriteTextFile(nameof(WriteSetting), ex.Message);
            }
        }

        private static async Task SyncTextFileInDB()
        {
            try
            {
                var log = await systemLogData.GetSystemLog(GetMacAddress());

                if (log != null)
                {
                    List<string> filePath = new List<string>();
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
                        if (File.Exists(@"C:\Program Files\WCT\" + file))
                        {
                            var data = File.ReadAllLines(@"C:\Program Files\WCT\" + file);
                            if (data != null)
                            {
                                foreach (var item in data)
                                {
                                    var logTime = DateTime.ParseExact(item.Split('|')[0].Trim(), "dd-MM-yy hh:mm:ss tt", null);
                                    //var logTime = Convert.ToDateTime(item.Split('|')[0].Trim());
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
            }
            catch (Exception ex)
            {
                WriteTextFile(nameof(SyncTextFileInDB), ex.Message);
            }
        }

        private static void WriteTextFile(string methodName, string message)
        {
            try
            {
                var logTime = DateTime.Now;
                string path = @"C:\Program Files\WCT\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, string.Format(@"Error_{0:dd_MM_yy}.txt", logTime));

                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sWriter = new StreamWriter(fs);
                sWriter.BaseStream.Seek(0, SeekOrigin.End);
                sWriter.WriteLine($"{logTime:dd-MM-yy hh:mm:ss tt} | {methodName} | {message}");
                sWriter.Flush();
                sWriter.Close();
            }
            catch { }
        }

        private static string GetMacAddress()
        {
            try
            {
                return NetworkInterface.GetAllNetworkInterfaces()
                            .Where(a => a.NetworkInterfaceType.ToString() == NetworkInterfaceType.Ethernet.ToString()
                                   && a.GetType().Name == "SystemNetworkInterface")
                            .Select(a => a.GetPhysicalAddress().ToString())
                            .FirstOrDefault();
            }
            catch (Exception ex)
            {
                WriteTextFile(nameof(GetMacAddress), ex.Message);
                return "";
            }
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
            catch (Exception ex)
            {
                WriteTextFile(nameof(GetConnectedWifi), ex.Message);
                return "";
            }
        }
        #endregion
    }
}
