using System;
using static TimeTrackerService.Enum;

namespace TimeTrackerService.DataModel
{
    public class AddSystemLogModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public LogTypes LogType { get; set; }
        public string Description { get; set; }
        public DateTime LogTime { get; set; }
    }
}