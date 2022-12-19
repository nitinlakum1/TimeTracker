using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model.Resources;
using TimeTracker_Model.Setting;

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

        public async Task<(List<Resources>, int)> GetResourcesList(ResourceFilterModel model)
        {
            var result = _context.Resources
                .Where(a => a.workYears == model.Experience
                       && (string.IsNullOrWhiteSpace(model.SearchText)
                       || a.preferenceId.ToLower().Contains(model.SearchText)
                       || a.name.ToLower().Contains(model.SearchText)
                       || a.gender.ToLower().Contains(model.SearchText)
                       || a.mobile.ToLower().Contains(model.SearchText)
                       || a.email.ToLower().Contains(model.SearchText))
                       && (string.IsNullOrWhiteSpace(model.Designation)
                       || a.designation.ToLower().Contains(model.Designation))
                       && (string.IsNullOrWhiteSpace(model.City)
                       || a.city.ToLower().Contains(model.City)));

            var totalRecord = result.Count();

            result = result
                .Skip(model.DisplayStart)
                .Take(model.PageSize);

            return (await result.ToListAsync(), totalRecord);
        }

        public async Task<Resources> GetResourceByPrId(string preferenceId)
        {
            return await _context.Resources
                .FirstOrDefaultAsync(a => a.preferenceId == preferenceId) ?? new Resources();
        }

        public async Task<bool> AddFollowup(ResourcesRemarks model)
        {
            _context.ResourcesRemarks.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditResourceStatus(Resources model)
        {
            _context.Resources.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ResourcesRemarks>> GetFollowupList(string id)
        {
            return await _context.ResourcesRemarks
                .Where(a => a.PreferenceId == id)
                .OrderByDescending(a => a.DateTime)
                .ToListAsync();
        }

        #endregion
    }
}