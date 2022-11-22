using AutoMapper;
using System.ComponentModel.DataAnnotations;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model.SystemLog;
using TimeTracker_Model.User;

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

        #endregion
    }
}