using TimeTracker_Model;

namespace TimeTracker.Models.SystemLog
{
    public class SystemLogListModel
    {
        public int Id { get; set; }
        public DateTime LogTime { get; set; }
        public string Description { get; set; }
        public LogTypes LogType { get; set; }
    }
}
