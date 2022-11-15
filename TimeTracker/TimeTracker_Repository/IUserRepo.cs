using TimeTracker_Model.User;

namespace TimeTracker_Repository
{
    public interface IUserRepo
    {
        Task<LoginDetailsModel> ValidateUser(LoginModel model);

        Task<(List<UserListModel>, int)> GetUserList(UserFilterModel model);

        Task<UserListModel> GetUserById(int id);

        Task<bool> AddUser(AddEditUserModel model);

        Task<bool> UpdateUser(AddEditUserModel model);

        Task<bool> DeleteUser(int id);
    }
}