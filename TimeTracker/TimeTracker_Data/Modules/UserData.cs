using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model;
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

        public async Task<Users> ValidateUser(LoginModel model)
        {
            var result = await _context
                .Users
                .Include(a => a.Roles)
                .FirstOrDefaultAsync(a => a.Username.ToLower() == model.Username.ToLower()
                                     && a.Password == model.Password);

            if (result == null)
            {
                result = new Users();
            }
            return result;
        }

        public async Task<(List<Users>, int)> GetUserList(UserFilterModel model)
        {
            var result = _context.Users
                .Where(a => string.IsNullOrWhiteSpace(model.SearchText)
                       || a.FullName.ToLower().Contains(model.SearchText)
                       || a.ContactNo.ToLower().Contains(model.SearchText)
                       || a.Email.ToLower().Contains(model.SearchText)
                       || a.Username.ToLower().Contains(model.SearchText));

            var totalRecord = result.Count();

            if (model.SortOrder.ToLower().Equals("desc")
                && model.SortColumn.ToLower().Equals("username"))
            {
                result = result.OrderByDescending(a => a.Username);
            }
            if (model.SortOrder.ToLower().Equals("asc")
                && model.SortColumn.ToLower().Equals("username"))
            {
                result = result.OrderBy(a => a.Username);
            }

            if (model.SortOrder.ToLower().Equals("desc")
                && model.SortColumn.ToLower().Equals("contactno"))
            {
                result = result.OrderByDescending(a => a.ContactNo);
            }
            if (model.SortOrder.ToLower().Equals("asc")
                && model.SortColumn.ToLower().Equals("contactno"))
            {
                result = result.OrderBy(a => a.ContactNo);
            }

            result = result
                .Skip(model.DisplayStart)
                .Take(model.PageSize);

            return (await result.ToListAsync(), totalRecord);
        }

        public async Task<Users> GetUserById(int id)
        {
            var result = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

            result ??= new Users();
            return result;
        }

        public async Task<bool> AddUser(Users model)
        {
            _context.Users.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUser(Users model)
        {
            _context.Users.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var result = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

            if (result == null)
            {
                return false;
            }
            _context.Users.Remove(result);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Users>> GetUserLookup()
        {
            return await _context.Users.Where(a => a.RoleId != (int)TimeTracker_Model.Roles.Admin).Select(a => new Users()
            {
                Id = a.Id,
                Username = a.Username,
            }).ToListAsync();
        }

        public async Task<bool> DeleteProfilePic(int id)
        {
            var result = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);
            if (result == null)
            {
                return false;
            }
            result.Url = "";
            _context.Users.Update(result);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<DateTime> GetJoiningDate(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);
            if (user == null)
            {
                return DateTime.MinValue;
            }
            return user.JoiningDate;
        }

        public async Task<bool> UpdateKey(string email, string key)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Email == email);
            user.Key = key;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetKey(string email)
        {
            var result = await _context.Users.FirstOrDefaultAsync(a => a.Email == email);
            return result.Key;
        }

        public async Task<bool> CreatePassword(Users model)
        {
            var result = _context.Users.FirstOrDefault(a => a.Email == model.Email);
            result.Password = model.Password;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateEmail(string email, int userId)
        {
            var result = !await _context.Users
                .AnyAsync(a => a.Email == email
                          && (userId == 0 || a.Id != userId));
            return result;
        }

        public async Task<bool> ValidateContactNo(string contactNo, int userId)
        {
            var result = !await _context.Users
                .AnyAsync(a => a.ContactNo == contactNo
                          && (userId == 0 || a.Id != userId));
            return result;
        }

        public async Task<bool> ValidateEmailForgotPass(string email)
        {
            var result = await _context.Users
                .AnyAsync(a => a.Email == email);
            return result;
        }

        public async Task<bool> ValidateUser(string username)
        {
            var result = await _context.Users
                .AnyAsync(a => a.Username == username);
            return result;
        }
        #endregion
    }
}