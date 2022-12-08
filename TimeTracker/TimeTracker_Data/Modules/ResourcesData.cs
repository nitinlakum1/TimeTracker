using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model.Resources;
using TimeTracker_Model.SystemLog;
using TimeTracker_Model.User;

namespace TimeTracker_Data.Modules
{
    public class ResourcesData
    {
        #region Declaration
        private readonly TTContext _context;
        #endregion

        #region Const
        public ResourcesData(TTContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        public async Task<(List<Resources>, int)> GetResourcesList(ResourcesFilterModel model)
        {
            var result = _context.Resources
                .Where(a => a.workYears == model.Experience
                       && (string.IsNullOrWhiteSpace(model.SearchText)
                       || a.name.ToLower().Contains(model.SearchText)
                       || a.mobile.ToLower().Contains(model.SearchText)
                       || a.email.ToLower().Contains(model.SearchText)
                       || a.degree.ToLower().Contains(model.SearchText)
                       || a.designation.ToLower().Contains(model.SearchText)));

            var totalRecord = result.Count();

            if (model.SortOrder.ToLower().Equals("desc")
                && model.SortColumn.ToLower().Equals("username"))
            {
                result = result.OrderByDescending(a => a.name);
            }
            if (model.SortOrder.ToLower().Equals("asc")
                && model.SortColumn.ToLower().Equals("username"))
            {
                result = result.OrderBy(a => a.name);
            }

            if (model.SortOrder.ToLower().Equals("desc")
                && model.SortColumn.ToLower().Equals("contactno"))
            {
                result = result.OrderByDescending(a => a.mobile);
            }
            if (model.SortOrder.ToLower().Equals("asc")
                && model.SortColumn.ToLower().Equals("contactno"))
            {
                result = result.OrderBy(a => a.mobile);
            }

            result = result
                .Skip(model.DisplayStart)
                .Take(model.PageSize);

            return (await result.ToListAsync(), totalRecord);
        }

        #endregion
    }
}