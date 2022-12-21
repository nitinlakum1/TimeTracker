using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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

    #region Roles
    public enum ResourceStatus : int
    {
        [Display(Name = "Call Not Attend")]
        [Description("Call Not Attend")]
        CallNotAttend = 1,

        [Display(Name = "Not Interested")]
        [Description("Not Interested")]
        NotInterested = 2,

        [Display(Name = "In Process")]
        [Description("In Process")]
        InProcess = 3,

        [Display(Name = "Pending")]
        [Description("Pending")]
        Pending = 8,

        [Display(Name = "Interview")]
        [Description("Interview")]
        Interview = 4,

        [Display(Name = "Selected")]
        [Description("Selected")]
        Selected = 5,

        [Display(Name = "Rejected")]
        [Description("Rejected")]
        Rejected = 6,

        [Display(Name = "Confirmed")]
        [Description("Confirmed")]
        Confirmed = 7,
    }
    #endregion
}