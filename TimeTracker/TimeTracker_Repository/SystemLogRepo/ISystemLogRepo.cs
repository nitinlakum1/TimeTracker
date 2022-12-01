using TimeTracker_Model.SystemLog;
using TimeTracker_Model.User;

namespace TimeTracker_Repository.SystemLogRepo
{
    public interface ISystemLogRepo
    {
        Task<(List<SystemLogModel>, int)> GetSystemLog(SystemLogFilterModel model);

        Task<List<SystemLogModel>> GetTodaysSystemLog(int userId);

        Task<List<SystemLogModel>> GetMonthlyReport(SystemLogFilterModel model);

        Task<bool> AddLog(SystemLogAdddModel model);
    }
}