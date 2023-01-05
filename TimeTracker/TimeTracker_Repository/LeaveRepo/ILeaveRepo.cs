using TimeTracker_Model.Leave;
using TimeTracker_Model.Resources;

namespace TimeTracker_Repository.LeaveRepo
{
    public interface ILeaveRepo
    {

        #region Signature
        Task<List<LeaveModel>> GetLeave(LeaveFilterModel model);

        Task<bool> AddLeave(AddLeaveModel model);

        Task<bool> ChangeStatus(int id, int btnId);

        Task<List<LeaveModel>> GetLeaveDetail(int id);

        Task<decimal> LeaveCount(int? id, DateTime startDate, DateTime endDate);

        Task<decimal> MonthlyLeaveCount(int id, string month);

        Task<decimal> UsedLeaveCountSalary(int id, string month);
        #endregion

    }
}