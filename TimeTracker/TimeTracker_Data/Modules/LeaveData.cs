using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model.Leave;
using TimeTracker_Model;
using System.Net.Http;
using Microsoft.VisualBasic;

namespace TimeTracker_Data.Modules
{
    public class LeaveData
    {
        #region Declaration
        private readonly TTContext _context;

        #endregion

        #region Const
        public LeaveData(TTContext context)
        {
            _context = context;

        }
        #endregion

        #region Methods

        public async Task<List<Leaves>> GetLeave(LeaveFilterModel model)
        {
            var result = _context.Leaves
                .Include(a => a.Users)
                .OrderByDescending(a => a.ApplyDate)
                .Where(a => (model.UserId == 0
                            || model.UserId == 1
                            || a.UserId == model.UserId)
                      && (string.IsNullOrWhiteSpace(model.SearchText)));

            return await result.ToListAsync();
        }

        public async Task<bool> AddLeave(Leaves model)
        {
            model.ApplyDate = DateTime.Now;
            model.Status = Status.Apply;

            _context.Leaves.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatus(int id, int btnId)
        {
            var result = await _context.Leaves.FirstOrDefaultAsync(a => a.Id == id);

            _ = result == null ? false : true;

            _ = (btnId == 2) ? (result.Status = Status.Approved) : (result.Status = Status.Declined);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Leaves>> GetLeaveDetail(int id)
        {
            return await _context.Leaves
                .Include(a => a.Users)
                .Where(a => a.Id == id)
                .ToListAsync();
        }

        public async Task<int> LeaveCount(int? id)
        {
            var startFinancialYearDate
                = new DateTime(DateTime.Now.Month > 3 ? DateTime.Now.Year : DateTime.Now.Year - 1, 4, 1);

            var endFinancialYearDate
                = new DateTime(DateTime.Now.Month < 4 ? DateTime.Now.Year : DateTime.Now.Year - 1, 3, 31);

            var leaves = await _context.Leaves
                .Where(a => a.UserId == id
                      && a.Status == Status.Approved
                      && a.IsPaid)
                .ToListAsync();

            int result = 0;
            foreach (var item in leaves)
            {
                for (DateTime date = item.LeaveFromDate; date <= item.LeaveToDate; date = date.AddDays(1))
                {
                    if (date.Date >= startFinancialYearDate && date.Date <= endFinancialYearDate)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public async Task<int> MonthlyLeaveCount(int id, string month)
        {
            var firstDayMonth = DateTime.Parse(month);
            var lastDayMonth = firstDayMonth.AddMonths(1).AddDays(-1);

            var leaves = await _context.Leaves
                .Where(a => a.UserId == id
                      && a.Status == Status.Approved
                      && a.IsPaid)
                .ToListAsync();

            int result = 0;
            foreach (var item in leaves)
            {
                for (DateTime date = item.LeaveFromDate; date <= item.LeaveToDate; date = date.AddDays(1))
                {
                    if (date.Date >= firstDayMonth && date.Date <= lastDayMonth)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public async Task<int> UsedLeaveCountSalary(int id, string month)
        {
            var startFinancialYearDate
                = new DateTime(DateTime.Now.Month > 3 ? DateTime.Now.Year : DateTime.Now.Year - 1, 4, 1);

            var selectedMonth = DateTime.Parse(month);
            var lastDayOfSalaryPreMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1).AddDays(-1);

            var leaves = await _context.Leaves
                .Where(a => a.UserId == id
                      && a.Status == Status.Approved
                      && a.IsPaid)
                .ToListAsync();

            int result = 0;
            foreach (var item in leaves)
            {
                for (DateTime date = item.LeaveFromDate; date <= item.LeaveToDate; date = date.AddDays(1))
                {
                    if (date.Date >= startFinancialYearDate && date.Date <= lastDayOfSalaryPreMonth)
                    {
                        result++;
                    }
                }
            }
            return result;
        }
        #endregion
    }
}