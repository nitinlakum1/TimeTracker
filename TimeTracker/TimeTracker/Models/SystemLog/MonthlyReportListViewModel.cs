namespace TimeTracker.Models.SystemLog
{
    public class MonthlyReportListViewModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime ClosingTime { get; set; }
        public string TotalTime { get; set; }
    }
}