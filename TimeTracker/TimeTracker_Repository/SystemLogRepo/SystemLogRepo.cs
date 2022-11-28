using AutoMapper;
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

        public async Task<(List<SystemLogModel>, int)> GetSystemLog(SystemLogFilterModel model)
        {
            var (userList, totalRecord) = await _systemLogData.GetSystemLog(model);

            var systemLogs = _mapper.Map<List<SystemLogModel>>(userList);

            return (systemLogs, totalRecord);
        }

        public async Task<List<SystemLogModel>> GetTodaysSystemLog(int userId)
        {
            var result = await _systemLogData.GetTodaysSystemLog(userId);

            return _mapper.Map<List<SystemLogModel>>(result);
        }
        #endregion
    }
}