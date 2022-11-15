using System;
using System.IO;
using System.ServiceProcess;

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
            SetLog("Service is Start");
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    SetLog("System Log On");
                    break;
                case SessionChangeReason.SessionLogoff:
                    SetLog("System Log Off");
                    break;
                case SessionChangeReason.RemoteConnect:
                    SetLog("System Remote Connect");
                    break;
                case SessionChangeReason.RemoteDisconnect:
                    SetLog("System Remote Disconnect");
                    break;
                case SessionChangeReason.SessionLock:
                    SetLog("System Locked");
                    break;
                case SessionChangeReason.SessionUnlock:
                    SetLog("System Unlocked");
                    break;
                default:
                    break;
            }
        }

        protected override void OnShutdown()
        {
            SetLog("System Shutdown");
        }

        protected override void OnStop()
        {
            SetLog("Service is Stopped");
        }
        #endregion

        #region Private Method
        private static void SetLog(string message)
        {
            string path = @"C:\Program Files\WCT\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, string.Format(@"{0:dd_MM_yy}.txt", DateTime.Now.Date));

            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sWriter = new StreamWriter(fs);
            sWriter.BaseStream.Seek(0, SeekOrigin.End);
            sWriter.WriteLine($"{DateTime.Now:dd-MM-yy hh:mm:ss tt} | {message}");
            sWriter.Flush();
            sWriter.Close();
        }
        #endregion
    }
}
