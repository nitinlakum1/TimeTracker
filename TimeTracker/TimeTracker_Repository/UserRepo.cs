using AutoMapper;
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

        #endregion
    }
}