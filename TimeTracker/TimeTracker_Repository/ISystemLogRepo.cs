﻿using TimeTracker_Model.SystemLog;
using TimeTracker_Model.User;

namespace TimeTracker_Repository
{
    public interface ISystemLogRepo
    {
        Task<(List<SystemLogModel>, int)> GetSystemLog(SystemLogFilterModel model);
    }
}