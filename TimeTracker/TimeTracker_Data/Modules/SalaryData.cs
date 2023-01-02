using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model;
using TimeTracker_Model.Salary;

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
                .Where(a => (model.UserId == 0
                            || model.UserId == 1
                            || a.UserId == model.UserId)
                      && (string.IsNullOrWhiteSpace(model.SearchText)));

            var totalRecord = result.Count();

            result = result
                .Skip(model.DisplayStart)
                .Take(model.PageSize);

            return (await result.ToListAsync(), totalRecord);
        }

        public async Task<Salarys> GetSalaryById(int id)
        {
            var result = await _context.Salarys
                .Include(a => a.Users)
                .FirstOrDefaultAsync(a => a.Id == id);

            result ??= new Salarys();
            return result;
        }

        public async Task<bool> AddSalary(Salarys model)
        {
            _context.Salarys.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSalary(Salarys model)
        {
            _context.Salarys.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(List<SalaryReports>, int)> GetSalaryReport(SalaryFilterModel model)
        {
            var result = _context.SalaryReports
                .Include(a => a.Users)
                .Where(a => (model.UserId == 0
                            || model.UserId == 1
                            || a.UserId == model.UserId)
                      && (string.IsNullOrWhiteSpace(model.SearchText)));

            var totalRecord = result.Count();

            result = result
                .Skip(model.DisplayStart)
                .Take(model.PageSize);

            return (await result.ToListAsync(), totalRecord);
        }

        public async Task<bool> AddSalaryReport(SalaryReports model)
        {
            var result = await _context.Salarys
                .Where(a => a.UserId == model.UserId)
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();

            model.BasicSalary = result.Salary;
            model.SalaryDate = DateTime.Now.Date;

            _context.SalaryReports.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetAmountById(int id, string month)
        {
            



            var result = await _context.Salarys
                .Where(a => a.UserId == id)
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();

            result ??= new Salarys();
            return result.Salary;
        }
        #endregion
    }
}