using TimeTracker_Model.Leave;
using TimeTracker_Model.Resources;

namespace TimeTracker_Repository.LeaveRepo
{
    public interface ILeaveRepo
    {

        #region Signature
        Task<(List<LeaveModel>, int)> GetLeave(LeaveFilterModel model);

        Task<bool> AddLeave(AddLeaveModel model);

        Task<bool> ChangeStatus(int id, int btnId);

        Task<List<LeaveModel>> GetLeaveDetail(int id);
        #endregion

    }
}