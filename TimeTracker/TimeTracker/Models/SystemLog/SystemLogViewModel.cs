using TimeTracker_Model;

namespace TimeTracker.Models.SystemLog
{
    public class SystemLogViewModel
    {
        public string UserId { get; set; }
        public DateTime LogTime { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public LogTypes LogType { get; set; }
    }
}
