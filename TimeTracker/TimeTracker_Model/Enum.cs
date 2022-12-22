using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TimeTracker_Model
{
    #region LogTypes
    public enum LogTypes : int
    {
        [Display(Name = "Service Start")]
        [Description("Service Start")]
        ServiceStart = 1,

        [Display(Name = "System Log On")]
        [Description("System Log On")]
        SystemLogOn = 2,

        [Display(Name = "System Log Off")]
        [Description("System Log Off")]
        SystemLogOff = 3,

        [Display(Name = "System Lock")]
        [Description("System Lock")]
        SystemLock = 4,

        [Display(Name = "System Unlock")]
        [Description("System Unlock")]
        SystemUnlock = 5,

        [Display(Name = "System Shutdown")]
        [Description("System Shutdown")]
        SystemShutdown = 6,

        [Display(Name = "Service Stopped")]
        [Description("Service Stopped")]
        ServiceStopped = 7,
    }
    #endregion

    #region Roles
    public enum Roles : int
    {
        [Description("Admin")]
        Admin = 1,

        [Description("Employee")]
        Employee = 2,

        [Description("HR")]
        HR = 3,
    }
    #endregion

    #region ResourceStatus
    public enum ResourceStatus : int
    {
        [Display(Name = "Call Not Attend")]
        [Description("Call Not Attend")]
        CallNotAttend = 1,

        [Display(Name = "Not Interested")]
        [Description("Not Interested")]
        NotInterested = 2,

        [Display(Name = "Follow Back")]
        [Description("Follow Back")]
        FollowBack = 3,

        [Display(Name = "In Process")]
        [Description("In Process")]
        InProcess = 4,

        [Display(Name = "Schedule Interview")]
        [Description("ScheduleInterview")]
        ScheduleInterview = 5,

        [Display(Name = "Reschedule")]
        [Description("Reschedule")]
        Reschedule = 6,

        [Display(Name = "Not Suitable")]
        [Description("Not Suitable")]
        NotSuitable = 7,

        [Display(Name = "Selected")]
        [Description("Selected")]
        Selected = 8,

        [Display(Name = "Rejected")]
        [Description("Rejected")]
        Rejected = 9,

        [Display(Name = "Confirmed")]
        [Description("Confirmed")]
        Confirmed = 10,
    }
    #endregion
}