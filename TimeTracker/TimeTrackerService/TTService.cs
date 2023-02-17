using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        static bool servesStart = false;
        static bool callRunning = false;
        #endregion

        #region Const
        public TTService()
        {
            InitializeComponent();
            systemLogData = new Data.SystemLogData();
        }
        #endregion

        #region Events
        protected override async void OnStart(string[] args)
        {
            string wifiName = "";
            while (string.IsNullOrWhiteSpace(wifiName))
            {
                wifiName = await GetConnectedWifi();
            }
            await SetLog("Service is Start", LogTypes.ServiceStart);
            servesStart = true;
        }

        protected override async void OnSessionChange(SessionChangeDescription changeDescription)
        {
            while (callRunning) { }
            callRunning = true;
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    while (!servesStart) { }
                    await SetLog("System Log On", LogTypes.SystemLogOn);
                    break;
                case SessionChangeReason.SessionLogoff:
                    await SetSystemLogOff("System Log Off", LogTypes.SystemLogOff);
                    break;
                case SessionChangeReason.SessionLock:
                    await SetLog("System Locked", LogTypes.SystemLock);
                    break;
                case SessionChangeReason.SessionUnlock:
                    await SetLog("System Unlocked", LogTypes.SystemUnlock);
                    break;
                default:
                    break;
            }
            callRunning = false;
        }

        protected override async void OnShutdown()
        {
            await SetLog("System Shutdown", LogTypes.SystemShutdown);
        }

        protected override async void OnStop()
        {
            await SetLog("Service is Stopped", LogTypes.ServiceStopped);
        }
        #endregion

        #region Private Method
        //private static async Task SetLog(string message, LogTypes logType)
        //{
        //    try
        //    {
        //        bool serverOnline = await systemLogData.IsServerConnected();
        //        if (serverOnline)
        //        {
        //            //Get settings from database then write in setting.txt file.
        //            await WriteSetting();
        //        }

        //        bool writeLog = await ValidateSettings();

        //        if (writeLog)
        //        {
        //            var logTime = DateTime.Now;
        //            var lastLog = GetTodaysLog();
        //            var addLog = true;
        //            if (lastLog != null)
        //            {
        //                if(lastLog.LogType == LogTypes.SystemLogOn
        //                    || lastLog.LogType == LogTypes.SystemUnlock
        //                    || lastLog.LogType == LogTypes.ServiceStart)
        //                {
        //                    addLog = logType == LogTypes.SystemLock || logType == LogTypes.SystemLogOff;
        //                }

        //                if (lastLog.LogType == LogTypes.SystemLock)
        //                {
        //                    addLog = logType == LogTypes.SystemUnlock || logType == LogTypes.SystemLogOn;
        //                }
        //            }

        //            if (addLog)
        //            {
        //                string wifiName = await GetConnectedWifi();
        //                if (serverOnline)
        //                {
        //                    //Sync log text file with database.
        //                    await SyncTextFileInDB();
        //                    try
        //                    {
        //                        var macAddress = GetMacAddress();

        //                        AddSystemLogModel model = new AddSystemLogModel()
        //                        {
        //                            MacAddress = macAddress,
        //                            LogType = logType,
        //                            Description = message,
        //                            LogTime = logTime,
        //                            CreatedOn = DateTime.Now,
        //                            WiFiName = wifiName
        //                        };
        //                        await systemLogData.AddSystemLog(model);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        WriteTextFile("AddSystemLog", ex.Message, GetErrorLineNumber(ex));
        //                    }
        //                }

        //                string path = @"C:\Program Files\WCT\";
        //                if (!Directory.Exists(path))
        //                {
        //                    Directory.CreateDirectory(path);
        //                }
        //                path = Path.Combine(path, string.Format(@"{0:dd-MM-yy}.txt", logTime));

        //                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        //                StreamWriter sWriter = new StreamWriter(fs);
        //                sWriter.BaseStream.Seek(0, SeekOrigin.End);
        //                sWriter.WriteLine($"{logTime:dd-MM-yy hh:mm:ss tt} | {(int)logType} | {message} | {wifiName}");
        //                sWriter.Flush();
        //                sWriter.Close();
        //            }
        //        }
        //        else
        //        {
        //            WriteTextFile("SetLog", "Settings are not configured.", 0);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteTextFile("SetLog", ex.Message, GetErrorLineNumber(ex));
        //    }
        //}

        private static async Task SetLog(string message, LogTypes logType)
        {
            try
            {
                bool serverOnline = await systemLogData.IsServerConnected();
                if (serverOnline)
                {
                    //Get settings from database then write in setting.txt file.
                    await WriteSetting();
                }

                bool writeLog = await ValidateSettings();

                if (writeLog)
                {
                    var logTime = DateTime.Now;
                    var lastLog = GetTodaysLog();
                    var addLog = true;
                    if (lastLog != null)
                    {
                        if (lastLog.LogType == LogTypes.SystemLogOn
                            || lastLog.LogType == LogTypes.SystemUnlock
                            || lastLog.LogType == LogTypes.ServiceStart)
                        {
                            addLog = logType == LogTypes.SystemLock || logType == LogTypes.SystemLogOff;
                        }

                        if (lastLog.LogType == LogTypes.SystemLock || lastLog.LogType == LogTypes.SystemLogOff)
                        {
                            addLog = logType == LogTypes.SystemUnlock || logType == LogTypes.SystemLogOn || logType == LogTypes.ServiceStart;
                        }
                    }

                    if (addLog)
                    {
                        string wifiName = await GetConnectedWifi();
                        string path = @"C:\Program Files\WCT\";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        path = Path.Combine(path, string.Format(@"{0:dd-MM-yy}.txt", logTime));

                        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter sWriter = new StreamWriter(fs);
                        sWriter.BaseStream.Seek(0, SeekOrigin.End);
                        sWriter.WriteLine($"{logTime:dd-MM-yy hh:mm:ss tt} | {(int)logType} | {message} | {wifiName}");
                        sWriter.Flush();
                        sWriter.Close();

                        //Sync log text file with database.
                        await SyncTextFileInDB();
                    }
                }
                else
                {
                    WriteTextFile("SetLog", "Settings are not configured.", 0);
                }
            }
            catch (Exception ex)
            {
                WriteTextFile("SetLog", ex.Message, GetErrorLineNumber(ex));
            }
        }

        private static async Task SetSystemLogOff(string message, LogTypes logType)
        {
            try
            {
                bool writeLog = await ValidateSettings();

                if (writeLog)
                {
                    var logTime = DateTime.Now;
                    string path = @"C:\Program Files\WCT\";
                    string wifiName = await GetConnectedWifi();
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, string.Format(@"{0:dd-MM-yy}.txt", logTime));

                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter sWriter = new StreamWriter(fs);
                    sWriter.BaseStream.Seek(0, SeekOrigin.End);
                    sWriter.WriteLine($"{logTime:dd-MM-yy hh:mm:ss tt} | {(int)logType} | {message} | {wifiName}");
                    sWriter.Flush();
                    sWriter.Close();
                }
                else
                {
                    WriteTextFile("SetSystemLogOff", "Settings are not configured.", 0);
                }
            }
            catch (Exception ex)
            {
                WriteTextFile("SetSystemLogOff", ex.Message, GetErrorLineNumber(ex));
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
                WriteTextFile("WriteSetting", ex.Message, GetErrorLineNumber(ex));
            }
        }

        private static async Task SyncTextFileInDB()
        {
            try
            {
                var log = await systemLogData.GetSystemLog(GetMacAddress());

                if (log != null)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\Program Files\WCT\");
                    FileInfo[] Files = directoryInfo.GetFiles("*.txt"); //Getting Text files

                    foreach (FileInfo file in Files)
                    {
                        var fileLogTime = DateTime.ParseExact(file.Name.Split('.')[0], "dd-MM-yy", null);

                        if (File.Exists(@"C:\Program Files\WCT\" + file.Name)
                            && log.LogTime.Date > fileLogTime)
                        {
                            //Delete extra unwanted text files.
                            File.Delete(@"C:\Program Files\WCT\" + file.Name);
                        }
                    }

                    List<string> filePath = new List<string>();
                    var length = (DateTime.Now.Date - log.LogTime.Date).TotalDays;

                    filePath.Add(string.Format(@"{0:dd-MM-yy}.txt", log.LogTime));
                    if (length > 0)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            filePath.Add(string.Format(@"{0:dd-MM-yy}.txt", log.LogTime.AddDays(i + 1)));
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
                                    var module = Convert.ToInt32(item.Split('|')[1].Trim());
                                    var message = item.Split('|')[2].Trim();
                                    var wifiName = item.Split('|')[3].Trim();

                                    if (logTime > log.LogTime)
                                    {
                                        string name = Enum.GetName(typeof(LogTypes), module);
                                        AddSystemLogModel model = new AddSystemLogModel()
                                        {
                                            MacAddress = GetMacAddress(),
                                            LogType = (LogTypes)module,
                                            Description = message,
                                            LogTime = logTime,
                                            CreatedOn = DateTime.Now,
                                            WiFiName = wifiName
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
                WriteTextFile("SyncTextFileInDB", ex.Message, GetErrorLineNumber(ex));
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
                path = Path.Combine(path, string.Format(@"Error-{0:dd-MM-yy}.txt", logTime));

                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sWriter = new StreamWriter(fs);
                sWriter.BaseStream.Seek(0, SeekOrigin.End);
                sWriter.WriteLine($"{logTime:dd-MM-yy hh:mm:ss:fff tt} | Line Number: {lineNumber} | {methodName} | {message}");
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
                WriteTextFile("GetMacAddress", ex.Message, GetErrorLineNumber(ex));
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

        private static async Task<string> GetConnectedWifi()
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "netsh.exe";
                p.StartInfo.Arguments = "wlan show interfaces";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();

                string s = await p.StandardOutput.ReadToEndAsync();
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

        private static async Task<bool> ValidateSettings()
        {
            bool result = false;
            try
            {
                #region Get setting from settings.json
                if (File.Exists(@"C:\Program Files\WCT\settings.json"))
                {
                    List<SettingModel> settings = null;
                    using (StreamReader r = new StreamReader(@"C:\Program Files\WCT\settings.json"))
                    {
                        string json = await r.ReadToEndAsync();
                        settings = JsonConvert.DeserializeObject<List<SettingModel>>(json);
                    }

                    if (settings != null && settings.Any())
                    {
                        //string wifiName = settings
                        //    .FirstOrDefault(a => a.Key.Equals(AppSettings.SYSTEM_WIFI_NAME))
                        //    .Value;
                        //bool wifiIsConnected = wifiName.Split(',').Contains(await GetConnectedWifi());
                        bool wifiIsConnected = true;

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

                        result = wifiIsConnected
                            && curentTime >= startTiming
                            && curentTime <= endTiming
                            && !DateTime.Now.ToString("dddd").Equals("Saturday")
                            && !DateTime.Now.ToString("dddd").Equals("Sunday");

                        var companyHolidays = settings
                            .FirstOrDefault(a => a.Key.Equals(AppSettings.COMPANY_HOLIDAYS))
                            .Value;

                        if (!string.IsNullOrWhiteSpace(companyHolidays) && result)
                        {
                            result = !companyHolidays.Split(',').Contains(DateTime.Now.ToString("dd-MM-yyyy"));
                        }
                    }

                    //TODO: Validate Employee leave.
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteTextFile("ValidateSettings", ex.Message, GetErrorLineNumber(ex));
            }
            return result;
        }

        private static AddSystemLogModel GetTodaysLog()
        {
            try
            {
                string path = Path.Combine(@"C:\Program Files\WCT\", string.Format(@"{0:dd-MM-yy}.txt", DateTime.Now));
                if (File.Exists(path))
                {
                    var data = File.ReadAllLines(path);
                    if (data != null && data.Length > 0)
                    {
                        string lastLog = data[data.Length - 1];
                        if (!string.IsNullOrWhiteSpace(lastLog))
                        {
                            var logTime = DateTime.ParseExact(lastLog.Split('|')[0].Trim(), "dd-MM-yy hh:mm:ss tt", null);
                            var module = Convert.ToInt32(lastLog.Split('|')[1].Trim());
                            var message = lastLog.Split('|')[2].Trim();

                            string name = Enum.GetName(typeof(LogTypes), module);
                            AddSystemLogModel model = new AddSystemLogModel()
                            {
                                LogType = (LogTypes)module,
                                Description = message,
                                LogTime = logTime
                            };
                            return model;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteTextFile("GetTodaysLog", ex.Message, GetErrorLineNumber(ex));
            }
            return null;
        }
        #endregion
    }
}
