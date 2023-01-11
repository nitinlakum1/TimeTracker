using AutoMapper;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model;
using TimeTracker_Model.Holiday;
using TimeTracker_Model.User;

namespace TimeTracker_Repository.UserRepo
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
            model.Password = Common.Encrypt(model.Password);
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
            model.Password = Common.Encrypt(model.Password);
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
            result.ContactNo = model.ContactNo;
            result.Gender = model.Gender;
            result.Address = model.Address;
            result.DOB = model.DOB;
            result.BankName = model.BankName;
            result.AccountNo = model.AccountNo;
            result.IFSC = model.IFSC;
            result.Url = model.Url;

            if (model.RoleId == (int)TimeTracker_Model.Roles.Admin)
            {
                result.JoiningDate = model.JoiningDate;
                result.Education = model.Education;
                result.Experience = model.Experience;
                result.Designation = model.Designation;
                result.MacAddress = model.MacAddress;
            }

            return await _userData.UpdateUser(result);
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _userData.DeleteUser(id);
        }

        public async Task<List<UserModel>> GetUserLookup()
        {
            var userList = await _userData.GetUserLookup();
            return _mapper.Map<List<UserModel>>(userList);
        }

        public async Task<bool> DeleteProfilePic(int id)
        {
            return await _userData.DeleteProfilePic(id);
        }

        public async Task<DateTime> GetJoiningDate(int id)
        {
            return await _userData.GetJoiningDate(id);
        }

        public async Task<bool> UpdateKey(string email, string key)
        {
            return await _userData.UpdateKey(email, key);
        }

        public async Task<string> GetKey(string email)
        {
            return await _userData.GetKey(email);
        }

        public async Task<bool> CreatePassword(CreatePasswordModel model)
        {
            model.Password = Common.Encrypt(model.Password);
            var result = _mapper.Map<Users>(model);
            return await _userData.CreatePassword(result);
        }

        public async Task<bool> ValidateEmail(string email, int userId)
        {
            return await _userData.ValidateEmail(email, userId);
        }

        public async Task<bool> ValidateContactNo(string contactNo, int userId)
        {
            return await _userData.ValidateContactNo(contactNo, userId);
        }

        public async Task<bool> ValidateEmailForgotPass(string email)
        {
            return await _userData.ValidateEmailForgotPass(email);
        }
        #endregion
    }
}