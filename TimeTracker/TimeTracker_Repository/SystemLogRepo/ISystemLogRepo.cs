using TimeTracker_Model.SystemLog;

namespace TimeTracker_Repository.SystemLogRepo
{
    public interface ISystemLogRepo
    {
        Task<List<SystemLogModel>> GetSystemLog(SystemLogFilterModel model);

        Task<List<SystemLogModel>> GetTodaysSystemLog(int userId);

        Task<List<SystemLogModel>> GetMonthlyReport(SystemLogFilterModel model);

        Task<bool> AddLog(SystemLogAdddModel model);

        Task<bool> DeleteLog(int id);

        Task<TimeSpan> GetLastTime(int userId, DateTime logDate);
    }
}