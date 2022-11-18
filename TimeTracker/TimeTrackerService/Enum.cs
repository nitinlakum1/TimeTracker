namespace TimeTrackerService
{
    public class Enum
    {
        #region LogTypes
        public enum LogTypes : int
        {
            Other = 0,
            SystemLogOn = 1,
            SystemLock = 2,
            SystemUnlock = 3,
            SystemLogOff = 4,
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
