using TimeTracker.Helper;
using TimeTracker_Model;

namespace TimeTracker.Models.SystemLog
{
    public class MonthlyReportViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime ClosingTime { get; set; }
        public int WorkingHours { get; set; }
    }
}
