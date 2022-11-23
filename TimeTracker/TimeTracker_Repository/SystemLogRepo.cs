using AutoMapper;
using TimeTracker_Data.Modules;
using TimeTracker_Model.SystemLog;

namespace TimeTracker_Repository
{
    public class SystemLogRepo : ISystemLogRepo
    {
        #region Declaration
        private readonly SystemLogData _SystemLogData;
        private readonly IMapper _mapper;
        #endregion

        #region Const
        public SystemLogRepo(SystemLogData SystemLogData, IMapper mapper)
        {
            _SystemLogData = SystemLogData;
            _mapper = mapper;
        }
        #endregion

        #region Methods

        public async Task<(List<SystemLogModel>, int)> GetSystemLog(SystemLogFilterModel model)
        {
            var (userList, totalRecord) = await _SystemLogData.GetSystemLog(model);

            var systemLogs = _mapper.Map<List<SystemLogModel>>(userList);

            return (systemLogs, totalRecord);
        }

        public async Task<List<SystemLogModel>> GetTodaysSystemLog(int userId)
        {
            var result = await _SystemLogData.GetTodaysSystemLog(userId);

            return _mapper.Map<List<SystemLogModel>>(result);
        }
        #endregion
    }
}