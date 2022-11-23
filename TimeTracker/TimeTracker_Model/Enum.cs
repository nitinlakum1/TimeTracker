using System.ComponentModel;

namespace TimeTracker_Model
{
    #region LogTypes
    public enum LogTypes : int
    {
        [Description("Service Start")]
        ServiceStart = 1,

        [Description("System Log On")]
        SystemLogOn = 2,

        [Description("System Log Off")]
        SystemLogOff = 3,

        [Description("System Lock")]
        SystemLock = 4,

        [Description("System Unlock")]
        SystemUnlock = 5,

        [Description("System Shutdown")]
        SystemShutdown = 6,

        [Description("Service Stopped")]
        ServiceStopped = 7,
    }
    #endregion
}