namespace TimeTracker.Models.SystemLog
{
    public class SystemLogListModel
    {
        public int Id { get; set; }
        public DateTime LogTime { get; set; }
        public string Description { get; set; }
    }
}
