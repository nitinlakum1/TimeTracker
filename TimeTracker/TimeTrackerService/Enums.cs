namespace TimeTrackerService
{
    public class Enums
    {
        #region LogTypes
        public enum LogTypes : int
        {
            ServiceStart = 1,
            SystemLogOn = 2,
            SystemLogOff = 3,
            SystemLock = 4,
            SystemUnlock = 5,
            SystemShutdown = 6,
            ServiceStopped = 7,
        }
        #endregion

        #region SPExceptions
        public enum SPExceptions : int
        {
            Success = 0,
            CustomError = 1001,
        }
        #endregion
    }
}
