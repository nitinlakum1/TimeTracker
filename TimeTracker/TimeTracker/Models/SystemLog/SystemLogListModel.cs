using TimeTracker.Helper;
using TimeTracker_Model;

namespace TimeTracker.Models.SystemLog
{
    public class SystemLogListModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public DateTime LogTime { get; set; }
        public string Description { get; set; }
        public LogTypes LogType { get; set; }
        public string LogTypeName
        {
            get
            {
                return CommonHelper.GetEnumDescription(LogType);
            }
        }
    }
}
