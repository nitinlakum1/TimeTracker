namespace TimeTracker_Model.SystemLog
{
    public class SystemLogModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public DateTime LogTime { get; set; }
        public string Description { get; set; }
        public LogTypes LogType { get; set; }
    }
}