using AutoMapper;
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

        public async Task<(List<UserListModel>, int)> GetUserList(UserFilterModel model)
        {
            var (userList, totalRecord) = await _userData.GetUserList(model);
            return (_mapper.Map<List<UserListModel>>(userList), totalRecord);
        }

        public async Task<UserListModel> GetUserById(int id)
        {
            var result = await _userData.GetUserById(id);
            return _mapper.Map<UserListModel>(result);
        }

        public async Task<bool> AddUser(AddUserModel model)
        {
            var user = _mapper.Map<Users>(model);
            user.CreateAt = DateTime.Now;
            return await _userData.AddUser(user);
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _userData.DeleteUser(id);
        }

        #endregion
    }
}