/*
 * Create Windows Service
 *  sc.exe create TTService binpath="G:\Projects\WCT\TimeTracker\PublishTimeTrackerService\TimeTrackerService.exe"
 * Manage Service Lifetime
 *  sc.exe start TTService | sc.exe stop TTService | sc.exe delete TTService
 */

using Microsoft.Win32;

namespace TimeTrackerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            SetLog("START1");
            _logger = logger;
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            SetLog("START2");
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }
        }

        static async void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
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
    }
}