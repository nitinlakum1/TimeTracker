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
            var result = _context.Salarys
                            .Include(a => a.Users)
                            .Where(a => a.UserId == model.UserId)
                            .OrderByDescending(a => a.Id)
                            .FirstOrDefault();
            if (result != null)
            {
                result.ToDate = model.FromDate.AddDays(-1);
            }


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

        public async Task<(decimal, decimal)> GetSalaryAmountById(int id, string month)
        {
            var selectedMonthStart = DateTime.Parse(month);
            var selectedMonthEnd = new DateTime(DateTime.Parse(month).Year, DateTime.Parse(month).Month, 1).AddMonths(1).AddDays(-1);

            var result = await _context.Salarys
                .Where(a => a.UserId == id).ToListAsync();

            var firstSalary = await _context.Salarys
                .Where(a => a.UserId == id)
                .OrderBy(a => a.Id)
                .FirstOrDefaultAsync();

            var lastSalary = await _context.Salarys
                .Where(a => a.UserId == id)
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();

            int presentDay = 0;
            decimal salary = 0;
            foreach (var item in result)
            {
                if (item.ToDate == null)
                {
                    item.ToDate = DateTime.Now;
                }
                if ((item.FromDate <= selectedMonthStart || item.FromDate <= selectedMonthEnd)
                    && (selectedMonthStart <= item.ToDate || selectedMonthEnd <= item.ToDate))
                {
                    var fromDateLast = new DateTime(item.FromDate.Year, item.FromDate.Month, 1).AddMonths(1).AddDays(-1);
                    if (item.FromDate == firstSalary.FromDate && fromDateLast == selectedMonthEnd)
                    {
                        presentDay = (31 - item.FromDate.Day) < 0 ? 0 : (31 - item.FromDate.Day);
                    }
                    else if (item.ToDate == lastSalary.ToDate)
                    {
                        presentDay = (item.ToDate.Value.Day) > 30 ? 30 : (item.ToDate.Value.Day);
                    }
                    else
                    {
                        presentDay = 30;
                    }
                    salary = item.Salary;
                }
            }

            //var result = await _context.Salarys
            //    .Where(a => a.UserId == id)
            //    .OrderByDescending(a => a.Id)
            //    .FirstOrDefaultAsync();

            //result ??= new Salarys();
            return (salary, presentDay);
        }
        #endregion
    }
}