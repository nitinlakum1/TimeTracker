using AutoMapper;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model.SystemLog;

namespace TimeTracker_Repository.SystemLogRepo
{
    public class SystemLogRepo : ISystemLogRepo
    {
        #region Declaration
        private readonly SystemLogData _systemLogData;
        private readonly IMapper _mapper;
        #endregion

        #region Const
        public SystemLogRepo(SystemLogData systemLogData, IMapper mapper)
        {
            _systemLogData = systemLogData;
            _mapper = mapper;
        }
        #endregion

        #region Methods

        public async Task<List<SystemLogModel>> GetSystemLog(SystemLogFilterModel model)
        {
            var result = await _systemLogData.GetSystemLog(model);

            return _mapper.Map<List<SystemLogModel>>(result);
        }

        public async Task<List<SystemLogModel>> GetTodaysSystemLog(int userId)
        {
            var result = await _systemLogData.GetTodaysSystemLog(userId);

            return _mapper.Map<List<SystemLogModel>>(result);
        }

        public async Task<List<SystemLogModel>> GetMonthlyReport(SystemLogFilterModel model)
        {
            var list= await _systemLogData.GetMonthlyReport(model);

            var result = _mapper.Map<List<SystemLogModel>>(list);

            return result;
        }

        public async Task<bool> AddLog(SystemLogAdddModel model)
        {
            var result = _mapper.Map<SystemLogs>(model);
            return await _systemLogData.AddLog(result);
        }

        public async Task<bool> DeleteLog(int id)
        {
            return await _systemLogData.DeleteLog(id);
        }
        #endregion
    }
}