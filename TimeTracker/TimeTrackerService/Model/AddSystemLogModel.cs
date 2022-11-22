using System;
using static TimeTrackerService.Enums;

namespace TimeTrackerService.DataModel
{
    public class AddSystemLogModel
    {
        public int Id { get; set; }
        public string MacAddress { get; set; }
        public LogTypes LogType { get; set; }
        public string Description { get; set; }
        public DateTime LogTime { get; set; }
    }
}