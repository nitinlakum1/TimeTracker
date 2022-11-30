using TimeTracker_Model.Salary;
using TimeTracker_Model.User;

namespace TimeTracker_Repository.UserRepo
{
    public interface ISalaryRepo
    {

        Task<(List<AddEditSalaryModel>, int)> GetSalary(SalaryFilterModel model);

        //Task<UserModel> GetUserById(int id);

        //Task<bool> AddUser(AddEditUserModel model);

        //Task<bool> UpdateUser(AddEditUserModel model);

        //Task<bool> DeleteUser(int id);
    }
}