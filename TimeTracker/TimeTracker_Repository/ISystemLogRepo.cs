﻿using TimeTracker_Model.SystemLog;

namespace TimeTracker_Repository
{
    public interface ISystemLogRepo
    {
        Task<(List<SystemLogModel>, int)> GetSystemLog(SystemLogFilterModel model);

        Task<List<SystemLogModel>> GetTodaysSystemLog(int userId);
    }
}