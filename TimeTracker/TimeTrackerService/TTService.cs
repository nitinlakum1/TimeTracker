using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Threading;
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
            Thread.Sleep(5000);
            SetLog("Service is Start", LogTypes.ServiceStart);
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    SetLog("System Log On", LogTypes.SystemLogOn);
                    break;
                case SessionChangeReason.SessionLogoff:
                    SystemLogOff("System Log Off", LogTypes.SystemLogOff);
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
            SetLog("System Shutdown", LogTypes.SystemShutdown);
        }

        protected override void OnStop()
        {
            SetLog("Service is Stopped", LogTypes.ServiceStopped);
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

                    //Validate all settings. (Wifi Connection | Start and End Timing | Working Day | Holiday)
                    if (settings != null && settings.Any())
                    {
                        string wifiName = settings
                            .FirstOrDefault(a => a.Key.Equals(AppSettings.SYSTEM_WIFI_NAME))
                            .Value;
                        bool wifiIsConnected = wifiName.Split(',').Contains(GetConnectedWifi());

                        var start = settings
                            .FirstOrDefault(a => a.Key.Equals(AppSettings.SYSTEM_LOG_START_TIMING))
                            .Value; //HH:mm | 23:59

                        var end = settings
                            .FirstOrDefault(a => a.Key.Equals(AppSettings.SYSTEM_LOG_END_TIMING))
                            .Value; //HH:mm | 23:59

                        var startTiming = new TimeSpan(
                            Convert.ToInt32(start.Split(':')[0]),
                            Convert.ToInt32(start.Split(':')[1]), 0);

                        var endTiming = new TimeSpan(
                            Convert.ToInt32(end.Split(':')[0]),
                            Convert.ToInt32(end.Split(':')[1]), 0);

                        TimeSpan curentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);

                        var companyHolidays = settings
                            .FirstOrDefault(a => a.Key.Equals(AppSettings.COMPANY_HOLIDAYS))
                            .Value; //HH:mm | 23:59

                        writeLog = wifiIsConnected
                            && DateTime.Now.Hour >= startTiming.Hours
                            && DateTime.Now.Minute >= startTiming.Minutes
                            && DateTime.Now.Hour <= endTiming.Hours
                            && DateTime.Now.Minute <= endTiming.Minutes
                            && !DateTime.Now.ToString("dddd").Equals("Saturday")
                            && !DateTime.Now.ToString("dddd").Equals("Sunday");

                        if (!string.IsNullOrWhiteSpace(companyHolidays) && !writeLog)
                        {
                            writeLog = !companyHolidays.Split(',').Contains(DateTime.Now.ToString("dd-MM-yyyy"));
                        }
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
                            WriteTextFile("AddSystemLog", ex.Message, GetErrorLineNumber(ex));
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
                    WriteTextFile(nameof(SetLog), "Settings are not configured.", 0);
                }
            }
            catch (Exception ex)
            {
                WriteTextFile(nameof(SetLog), ex.Message, GetErrorLineNumber(ex));
            }
        }

        private static async void SystemLogOff(string message, LogTypes module)
        {
            try
            {
                bool writeLog = false;
                #region Get setting from settings.json
                if (File.Exists(@"C:\Program Files\WCT\settings.json"))
                {
                    List<SettingModel> settings = null;
                    using (StreamReader r = new StreamReader(@"C:\Program Files\WCT\settings.json"))
                    {
                        string json = r.ReadToEnd();
                        settings = JsonConvert.DeserializeObject<List<SettingModel>>(json);
                    }

                    //Validate all settings. (Wifi Connection | Start and End Timing | Working Day | Holiday)
                    if (settings != null && settings.Any())
                    {
                        string wifiName = settings
                            .FirstOrDefault(a => a.Key.Equals(AppSettings.SYSTEM_WIFI_NAME))
                            .Value;
                        bool wifiIsConnected = wifiName.Split(',').Contains(GetConnectedWifi());

                        var start = settings
                            .FirstOrDefault(a => a.Key.Equals(AppSettings.SYSTEM_LOG_START_TIMING))
                            .Value; //HH:mm | 23:59

                        var end = settings
                            .FirstOrDefault(a => a.Key.Equals(AppSettings.SYSTEM_LOG_END_TIMING))
                            .Value; //HH:mm | 23:59

                        var startTiming = new TimeSpan(
                            Convert.ToInt32(start.Split(':')[0]),
                            Convert.ToInt32(start.Split(':')[1]), 0);

                        var endTiming = new TimeSpan(
                            Convert.ToInt32(end.Split(':')[0]),
                            Convert.ToInt32(end.Split(':')[1]), 0);

                        TimeSpan curentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);

                        var companyHolidays = settings
                            .FirstOrDefault(a => a.Key.Equals(AppSettings.COMPANY_HOLIDAYS))
                            .Value; //HH:mm | 23:59

                        writeLog = wifiIsConnected
                            && DateTime.Now.Hour >= startTiming.Hours
                            && DateTime.Now.Minute >= startTiming.Minutes
                            && DateTime.Now.Hour <= endTiming.Hours
                            && DateTime.Now.Minute <= endTiming.Minutes
                            && !DateTime.Now.ToString("dddd").Equals("Saturday")
                            && !DateTime.Now.ToString("dddd").Equals("Sunday");

                        if (!string.IsNullOrWhiteSpace(companyHolidays) && !writeLog)
                        {
                            writeLog = !companyHolidays.Split(',').Contains(DateTime.Now.ToString("dd-MM-yyyy"));
                        }
                    }
                }
                #endregion

                if (writeLog)
                {
                    var logTime = DateTime.Now;
                    string path = @"C:\Program Files\WCT\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, string.Format(@"{0:dd_MM_yy}.txt", logTime));

                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter sWriter = new StreamWriter(fs);
                    sWriter.BaseStream.Seek(0, SeekOrigin.End);
                    await sWriter.WriteLineAsync($"{logTime:dd-MM-yy hh:mm:ss tt} | {(int)module} | {message}");
                    sWriter.Flush();
                    sWriter.Close();
                }
                else
                {
                    WriteTextFile(nameof(SystemLogOff), "Settings are not configured.", 0);
                }
            }
            catch (Exception ex)
            {
                WriteTextFile(nameof(SystemLogOff), ex.Message, GetErrorLineNumber(ex));
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
                               || a.Key.Equals(AppSettings.COMPANY_HOLIDAYS)
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
                WriteTextFile(nameof(WriteSetting), ex.Message, GetErrorLineNumber(ex));
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
                WriteTextFile(nameof(SyncTextFileInDB), ex.Message, GetErrorLineNumber(ex));
            }
        }

        private static void WriteTextFile(string methodName, string message, int lineNumber)
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
                sWriter.WriteLine($"{logTime:dd-MM-yy hh:mm:ss tt} | Line Number: {lineNumber} | {methodName} | {message}");
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
                WriteTextFile(nameof(GetMacAddress), ex.Message, GetErrorLineNumber(ex));
                return "";
            }
        }

        private static int GetErrorLineNumber(Exception exception)
        {
            try
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(exception, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                return frame.GetFileLineNumber();
            }
            catch
            {
                return 0;
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
                WriteTextFile(nameof(GetConnectedWifi), ex.Message, GetErrorLineNumber(ex));
                return "";
            }
        }
        #endregion
    }
}
