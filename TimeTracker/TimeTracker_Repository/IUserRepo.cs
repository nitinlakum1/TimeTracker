using TimeTracker_Model.User;

namespace TimeTracker_Repository
{
    public interface IUserRepo
    {
        Task<LoginDetailsModel> ValidateUser(LoginModel model);
    }
}