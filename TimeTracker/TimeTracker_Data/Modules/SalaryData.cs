using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model.User;

namespace TimeTracker_Data.Modules
{
    public class SalaryData
    {
        #region Declaration
        private readonly TTContext _context;
        #endregion

        #region Const
        public SalaryData(TTContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        public async Task<(List<Salarys>, int)> GetSalary(SalaryFilterModel model)
        {
            var result = _context.Salarys
                .Include(a => a.Users)
                .Where(a => string.IsNullOrWhiteSpace(model.SearchText));

            var totalRecord = result.Count();

            result = result
                .Skip(model.DisplayStart)
                .Take(model.PageSize);

            return (await result.ToListAsync(), totalRecord);
        }

        //public async Task<Users> GetUserById(int id)
        //{
        //    var result = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

        //    result ??= new Users();
        //    return result;
        //}

        //public async Task<bool> AddUser(Users model)
        //{
        //    _context.Users.Add(model);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> UpdateUser(Users model)
        //{
        //    _context.Users.Update(model);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> DeleteUser(int id)
        //{
        //    var result = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

        //    if (result == null)
        //    {
        //        return false;
        //    }
        //    _context.Users.Remove(result);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        #endregion
    }
}