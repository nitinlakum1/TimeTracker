using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model.User;

namespace TimeTracker_Data.Modules
{
    public class UserData
    {
        #region Declaration
        private readonly TTContext _context;
        #endregion

        #region Const
        public UserData(TTContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        public async Task<User> ValidateUser(LoginModel model)
        {
            var result = await _context.Users
                                       .FirstOrDefaultAsync(a => a.Username.ToLower() == model.Username.ToLower()
                                                            && a.Password == model.Password);

            if (result == null)
            {
                result = new User();
            }
            return result;
        }

        #endregion
    }
}