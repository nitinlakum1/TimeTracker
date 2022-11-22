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

        public async Task<(List<SystemLogs>, int)> GetSystemLog(SystemLogFilterModel model)
        {
            var result = _context.SystemLogs
                .Where(a => string.IsNullOrWhiteSpace(model.SearchText)
                       || a.Description.ToLower().Contains(model.SearchText));

            var totalRecord = result.Count();

            if (model.SortOrder.ToLower().Equals("desc")
                && model.SortColumn.ToLower().Equals("description"))
            {
                result = result.OrderByDescending(a => a.Description);
            }
            if (model.SortOrder.ToLower().Equals("asc")
                && model.SortColumn.ToLower().Equals("description"))
            {
                result = result.OrderBy(a => a.Description);
            }
            if (model.SortOrder.ToLower().Equals("desc")
                && model.SortColumn.ToLower().Equals("logtime"))
            {
                result = result.OrderByDescending(a => a.LogTime);
            }
            if (model.SortOrder.ToLower().Equals("asc")
                && model.SortColumn.ToLower().Equals("logtime"))
            {
                result = result.OrderBy(a => a.LogTime);
            }

            result = result
                .Skip(model.DisplayStart)
                .Take(model.PageSize);

            return (await result.ToListAsync(), totalRecord);
        }

        public async Task<bool> DeleteSystemLog(int id)
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
    }
            #endregion
}