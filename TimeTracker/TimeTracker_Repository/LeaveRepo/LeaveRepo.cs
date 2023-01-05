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

        public async Task<List<LeaveModel>> GetLeave(LeaveFilterModel model)
        {
            var result = await _leaveData.GetLeave(model);
            return _mapper.Map<List<LeaveModel>>(result);
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

        public async Task<decimal> LeaveCount(int? id, DateTime startDate, DateTime endDate)
        {
            return await _leaveData.LeaveCount(id, startDate, endDate);
        }

        public async Task<decimal> MonthlyLeaveCount(int id, string month)
        {
            var firstDayMonth = DateTime.Parse(month);
            var lastDayMonth = firstDayMonth.AddMonths(1).AddDays(-1);

            return await _leaveData.LeaveCount(id, firstDayMonth, lastDayMonth);
        }

        public async Task<decimal> UsedLeaveCountSalary(int id, string month)
        {
            var startFinancialYearDate
                = new DateTime(DateTime.Now.Month > 3 ? DateTime.Now.Year : DateTime.Now.Year - 1, 4, 1);

            var selectedMonth = DateTime.Parse(month);
            var lastDayOfSalaryPreMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1).AddDays(-1);

            return await _leaveData.LeaveCount(id, startFinancialYearDate, lastDayOfSalaryPreMonth);
        }
        #endregion
    }
}