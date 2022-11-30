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

        #endregion
    }
}