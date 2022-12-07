using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model.Holiday;
using TimeTracker_Model.Setting;

namespace TimeTracker_Data.Modules
{
    public class SettingData
    {
        #region Declaration
        private readonly TTContext _context;
        #endregion

        #region Const
        public SettingData(TTContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        public async Task<(List<Settings>, int)> SettingList(SettingFilterModel model)
        {
            var result = _context.Settings
                .Where(a => string.IsNullOrWhiteSpace(model.SearchText)
                       || a.Key.ToLower().Contains(model.SearchText)
                       || a.Value.ToLower().Contains(model.SearchText));

            var totalRecord = result.Count();

            if (model.SortOrder.ToLower().Equals("desc")
                && model.SortColumn.ToLower().Equals("key"))
            {
                result = result.OrderByDescending(a => a.Key);
            }
            if (model.SortOrder.ToLower().Equals("asc")
                && model.SortColumn.ToLower().Equals("key"))
            {
                result = result.OrderBy(a => a.Key);
            }
            if (model.SortOrder.ToLower().Equals("desc")
                && model.SortColumn.ToLower().Equals("value"))
            {
                result = result.OrderByDescending(a => a.Value);
            }
            if (model.SortOrder.ToLower().Equals("asc")
                && model.SortColumn.ToLower().Equals("value"))
            {
                result = result.OrderBy(a => a.Value);
            }

            result = result
                .Skip(model.DisplayStart)
                .Take(model.PageSize);

            return (await result.ToListAsync(), totalRecord);
        }

        public async Task<Settings> GetSettingById(int id)
        {
            var result = await _context.Settings.FirstOrDefaultAsync(a => a.Id == id);

            result ??= new Settings();

            return result;
        }

        public async Task<bool> UpdateSetting(Settings model)
        {
            _context.Settings.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Resources> GetResourceById(string id)
        {
            var result = await _context.Resources.FirstOrDefaultAsync(a => a.id == id);

            result ??= new Resources();

            return result;
        }

        public async Task<List<Resources>> GetResources()
        {
            var result = await _context.Resources.ToListAsync();

            result ??= new List<Resources>();

            return result;
        }

        public async Task<bool> AddResources(Resources model)
        {
            _context.Resources.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditResource(Resources model)
        {
            _context.Resources.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}