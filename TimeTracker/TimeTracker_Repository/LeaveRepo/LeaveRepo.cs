using AutoMapper;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model.Leave;
using TimeTracker_Model.Resources;
using TimeTracker_Model.Salary;

namespace TimeTracker_Repository.LeaveRepo
{
    public class LeaveRepo : ILeaveRepo
    {
        #region Declaration
        private readonly LeaveData _leaveData;
        private readonly IMapper _mapper;
        #endregion

        #region Const
        public LeaveRepo(LeaveData leaveData, IMapper mapper)
        {
            _leaveData = leaveData;
            _mapper = mapper;
        }
        #endregion

        #region Methods

        public async Task<(List<LeaveModel>, int)> GetLeave(LeaveFilterModel model)
        {
            var (result, totalRecord) = await _leaveData.GetLeave(model);

            var leaveList = _mapper.Map<List<LeaveModel>>(result);
            return (leaveList, totalRecord);
        }

        public async Task<bool> AddLeave(AddLeaveModel model)
        {
            var result = _mapper.Map<Leaves>(model);
            return await _leaveData.AddLeave(result);
        }

        public async Task<bool> ChangeStatus(int id, int btnId)
        {
            return await _leaveData.ChangeStatus(id, btnId);
        }

        public async Task<List<LeaveModel>> GetLeaveDetail(int id)
        {
            var result = await _leaveData.GetLeaveDetail(id);

            var leaveList = _mapper.Map<List<LeaveModel>>(result);
            return leaveList;
        }

        public async Task<int> LeaveCount(int? id)
        {
            return await _leaveData.LeaveCount(id);
        }
        #endregion
    }
}