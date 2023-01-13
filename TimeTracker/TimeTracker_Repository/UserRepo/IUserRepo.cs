using TimeTracker_Model.User;

namespace TimeTracker_Repository.UserRepo
{
    public interface IUserRepo
    {
        Task<LoginDetailsModel> ValidateUser(LoginModel model);

        Task<(List<UserModel>, int)> GetUserList(UserFilterModel model);

        Task<UserModel> GetUserById(int id);

        Task<bool> AddUser(AddEditUserModel model);

        Task<bool> UpdateUser(AddEditUserModel model);

        Task<bool> DeleteUser(int id);

        Task<List<UserModel>> GetUserLookup();

        Task<bool> DeleteProfilePic(int id);

        Task<DateTime> GetJoiningDate(int id);

        Task<bool> UpdateKey(string email, string key);

        Task<string> GetKey(string email);

        Task<bool> CreatePassword(CreatePasswordModel model);

        Task<bool> ValidateEmail(string email, int userId);

        Task<bool> ValidateContactNo(string contactNo, int userId);

        Task<bool> ValidateEmailForgotPass(string email);

        Task<bool> ValidateUser(string username);

        Task<bool> ValidatePassword(string password, string username);
    }
}