﻿using TimeTracker_Model.User;

namespace TimeTracker_Repository
{
    public interface IUserRepo
    {
        Task<LoginDetailsModel> ValidateUser(LoginModel model);

        Task<(List<UserModel>, int)> GetUserList(UserFilterModel model);

        Task<UserModel> GetUserById(int id);

        Task<bool> AddUser(AddEditUserModel model);

        Task<bool> UpdateUser(AddEditUserModel model);

        Task<bool> DeleteUser(int id);
    }
}