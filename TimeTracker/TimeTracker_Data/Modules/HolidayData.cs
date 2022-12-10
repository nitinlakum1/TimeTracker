using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model.Holiday;

namespace TimeTracker_Data.Modules
{
    public class HolidayData
    {
        #region Declaration
        private readonly TTContext _context;
        #endregion

        #region Const
        public HolidayData(TTContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<(List<Holidays>, int)> GetHolidayList(HolidayFilterModel model)
        {
            var result = _context.Holidays
                .Where(a => string.IsNullOrWhiteSpace(model.SearchText)
                       || a.Name.ToLower().Contains(model.SearchText));

            var totalRecord = result.Count();

            if (model.SortOrder.ToLower().Equals("desc")
                && model.SortColumn.ToLower().Equals("name"))
            {
                result = result.OrderByDescending(a => a.Name);
            }
            if (model.SortOrder.ToLower().Equals("asc")
                && model.SortColumn.ToLower().Equals("username"))
            {
                result = result.OrderBy(a => a.Name);
            }

            result = result
                .Skip(model.DisplayStart)
                .Take(model.PageSize);

            return (await result.ToListAsync(), totalRecord);
        }

        public async Task<Holidays> GetHolidayById(int id)
        {
            var result = await _context.Holidays.FirstOrDefaultAsync(a => a.Id == id);

            result ??= new Holidays();
            return result;
        }

        public async Task<bool> AddHoliday(Holidays model)
        {
            _context.Holidays.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateHoliday(Holidays model)
        {
            _context.Holidays.Update(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHoliday(int id)
        {
            var result = await _context.Holidays.FirstOrDefaultAsync(a => a.Id == id);

            if (result == null)
            {
                return false;
            }
            _context.Holidays.Remove(result);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion
    }
}