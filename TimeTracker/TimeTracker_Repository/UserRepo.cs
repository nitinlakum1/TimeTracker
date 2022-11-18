using AutoMapper;
using System.ComponentModel.DataAnnotations;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model.User;

namespace TimeTracker_Repository
{
    public class UserRepo : IUserRepo
    {
        #region Declaration
        private readonly UserData _userData;
        private readonly IMapper _mapper;
        #endregion

        #region Const
        public UserRepo(UserData userData, IMapper mapper)
        {
            _userData = userData;
            _mapper = mapper;
        }
        #endregion

        #region Methods

        public async Task<LoginDetailsModel> ValidateUser(LoginModel model)
        {
            var result = await _userData.ValidateUser(model);
            return _mapper.Map<LoginDetailsModel>(result);
        }

        public async Task<(List<UserModel>, int)> GetUserList(UserFilterModel model)
        {
            var (userList, totalRecord) = await _userData.GetUserList(model);
            return (_mapper.Map<List<UserModel>>(userList), totalRecord);
        }

        public async Task<UserModel> GetUserById(int id)
        {
            var result = await _userData.GetUserById(id);
            return _mapper.Map<UserModel>(result);
        }

        public async Task<bool> AddUser(AddEditUserModel model)
        {
            var user = _mapper.Map<Users>(model);
            user.CreateAt = DateTime.Now;
            return await _userData.AddUser(user);
        }

        public async Task<bool> UpdateUser(AddEditUserModel model)
        {
            var result = await _userData.GetUserById(model.Id);
            result.Username = model.Username;
            result.FullName = model.FullName;
            result.Email = model.Email;
            result.Designation = model.Designation;
            result.Education = model.Education;
            result.Experience = model.Experience;
            result.ContactNo = model.ContactNo;
            result.Gender = model.Gender;
            result.Address = model.Address;
            result.DOB = model.DOB;
            result.JoiningDate = model.JoiningDate;
            result.BankName = model.BankName;
            result.AccountNo = model.AccountNo;
            result.IFSC = model.IFSC;
            result.MacAddress = model.MacAddress;

            return await _userData.UpdateUser(result);
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _userData.DeleteUser(id);
        }

        #endregion
    }
}