namespace TimeTracker_Model.SystemLog
{
    public class MonthlyReportModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime ClosingTime { get; set; }
        public int WorkingHours { get; set; }
    }
}