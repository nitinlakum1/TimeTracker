using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;
using TimeTracker_Model.Leave;
using TimeTracker_Model;

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

        public async Task<(List<Leaves>, int)> GetLeave(LeaveFilterModel model)
        {
            var result = _context.Leaves
                .Include(a => a.Users)
                .OrderByDescending(a => a.ApplyDate)
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

        public async Task<bool> AddLeave(Leaves model)
        {
            model.ApplyDate = DateTime.Now;

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
            var count = await _context.Leaves.
                Where(a => a.UserId == id
                        && a.Status == Status.Approved
                        && a.IsPaid == true).ToListAsync();

            var countFrom = count.Select(a => a.LeaveFromDate).ToList();
            var countTo = count.Select(a => a.LeaveToDate).ToList();

            int result = 0;
            for (int i = 0; i < countFrom.Count; i++)
            {
                result += (countTo[i] - countFrom[i]).Days + 1;
            }
            return result;
        }
        #endregion
    }
}