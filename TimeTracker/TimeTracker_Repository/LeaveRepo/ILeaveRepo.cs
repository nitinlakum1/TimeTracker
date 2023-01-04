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

        Task<int> LeaveCount(int? id);

        Task<int> MonthlyLeaveCount(int id, string month);

        Task<int> UsedLeaveCountSalary(int id, string month);
        #endregion

    }
}