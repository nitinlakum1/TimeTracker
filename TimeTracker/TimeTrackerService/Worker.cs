using Microsoft.Win32;
using System.Diagnostics;

namespace TimeTrackerService
{
    public class Worker : BackgroundService
    {
        #region Declaration
        private readonly ILogger<Worker> _logger; 
        #endregion

        #region Constructor
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        } 
        #endregion

        #region Events
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!IsProcessOpen("ttservice"))
                {

                }
                // SetLog(string.Format("RUN: {0}", IsProcessOpen("skype")));
                await Task.Delay(1000, stoppingToken);
            }
        }

        static void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            SetLog("START2");
            try
            {
                if (e.Reason == SessionSwitchReason.SessionLock)
                {
                    //I left my desk
                    SetLog("I left my desk");
                }
                else if (e.Reason == SessionSwitchReason.SessionUnlock)
                {
                    //I returned to my desk
                    SetLog("I returned to my desk");
                }
            }
            catch (Exception ex)
            {
                SetLog(ex.Message);
            }
        } 
        #endregion

        #region Private Methods
        private bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.ToLower().Contains(name))
                {
                    return true;
                }
            }
            return false;
        }

        private static void SetLog(string message)
        {
            string path = "d:\\service\\data.txt";
            // This text is added only once to the file.
            if (File.Exists(path))
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine($"{DateTime.Now.ToLongTimeString()} | {message}");
                }
            }
            else
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine($"{DateTime.Now.ToLongTimeString()} | {message}");
                }
            }
        } 
        #endregion
    }
}