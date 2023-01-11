using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model.UpdateService;

namespace TimeTracker_Data.Modules
{
    public class UpdateServiceData
    {
        #region Declaration
        private readonly TTContext _context;
        #endregion

        #region Const
        public UpdateServiceData(TTContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<List<UpdateServices>> GetUpdateServiceList(UpdateServiceFilterModel model)
        {
            return await _context.UpdateServices.ToListAsync();
        }

        public async Task<bool> AddUpdateService(UpdateServices model)
        {
            _context.UpdateServices.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUpdateService(int id)
        {
            var result = await _context.UpdateServices.FirstOrDefaultAsync(a => a.Id == id);

            if (result == null)
            {
                return false;
            }
            _context.UpdateServices.Remove(result);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion
    }
}