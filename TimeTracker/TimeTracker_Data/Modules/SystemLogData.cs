using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model.SystemLog;

namespace TimeTracker_Data.Modules
{
    public class SystemLogData
    {
        #region Declaration
        private readonly TTContext _context;
        #endregion

        #region Const
        public SystemLogData(TTContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        public async Task<List<SystemLogs>> GetSystemLog(SystemLogFilterModel model)
        {
            var result = _context.SystemLogs
                .Include(a => a.Users)
                .Where(a => a.UserId == model.UserId);

            if (model.FromDate != null && model.ToDate != null)
            {
                result = result
                    .Where(a => a.LogTime.Date >= model.FromDate.Value.Date
                           && a.LogTime.Date <= model.ToDate.Value.Date);
            }
            return (await result.OrderBy(a => a.LogTime).ToListAsync());
        }

        public async Task<List<SystemLogs>> GetTodaysSystemLog(int userId)
        {
            var result = await _context.SystemLogs
                .Where(a => a.LogTime.Date == DateTime.Now.Date
                       && a.UserId == userId)
                .OrderBy(a => a.LogTime)
                .ToListAsync();

            return result;
        }

        public async Task<List<SystemLogs>> GetMonthlyReport(SystemLogFilterModel model)
        {
            var result = _context.SystemLogs
                .Include(a => a.Users)
                .Where(a => a.UserId == model.UserId
                       && (string.IsNullOrWhiteSpace(model.SearchText)
                           || a.Description.ToLower().Contains(model.SearchText)));

            if (model.FromDate != null && model.ToDate != null)
            {
                result = result
                    .Where(a => a.LogTime.Date >= model.FromDate.Value.Date
                           && a.LogTime.Date <= model.ToDate.Value.Date);
            }

            return await result.OrderBy(a => a.LogTime).ToListAsync();
        }

        public async Task<bool> AddLog(SystemLogs model)
        {
            _context.SystemLogs.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLog(int id)
        {
            var result = await _context.SystemLogs.FirstOrDefaultAsync(a => a.Id == id);

            if (result == null)
            {
                return false;
            }
            _context.SystemLogs.Remove(result);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion
    }
}