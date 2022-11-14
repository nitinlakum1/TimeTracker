using TimeTracker_Model.User;

namespace TimeTracker_Repository
{
    public interface IUserRepo
    {
        Task<LoginDetailsModel> ValidateUser(LoginModel model);

        Task<(List<UserListModel>, int)> GetUserList(UserFilterModel model);

        Task<UserListModel> GetUserById(int id);

        Task<bool> DeleteUser(int id);
    }
}