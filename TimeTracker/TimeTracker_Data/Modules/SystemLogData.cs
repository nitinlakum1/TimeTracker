﻿using Microsoft.EntityFrameworkCore;
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
                .Include(a => a.Users)
                .Where(a => (model.UserId == 0
                             || a.UserId == model.UserId)
                       && (string.IsNullOrWhiteSpace(model.SearchText)
                           || a.Description.ToLower().Contains(model.SearchText)));

            if (model.FromDate != null && model.ToDate != null)
            {
                result = result
                    .Where(a => a.LogTime.Date >= model.FromDate.Value.Date
                           && a.LogTime.Date <= model.ToDate.Value.Date);
            }

            //var name = result.ToListAsync();
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
            //var result = _context.SystemLogs
            //    .Include(a => a.Users)
            //    .Where(a => (model.UserId == 0
            //                 || a.UserId == model.UserId)
            //           && (string.IsNullOrWhiteSpace(model.SearchText)
            //               || a.Description.ToLower().Contains(model.SearchText))
            //           && a.LogTime.Date >= model.FromDate.Value.Date
            //           && a.LogTime.Date <= model.ToDate.Value.Date);

            var result = _context.SystemLogs
                .Include(a => a.Users)
                .Where(a => (model.UserId == 0
                             || a.UserId == model.UserId));

            if (model.FromDate != null && model.ToDate != null)
            {
                result = result
                    .Where(a => a.LogTime.Date >= model.FromDate.Value.Date
                           && a.LogTime.Date <= model.ToDate.Value.Date);
            }
            return await result.ToListAsync();
        }

        #endregion
    }
}