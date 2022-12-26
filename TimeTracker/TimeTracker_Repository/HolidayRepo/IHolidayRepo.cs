using TimeTracker_Model.Holiday;

namespace TimeTracker_Repository
{
    public interface IHolidayRepo
    {
        Task<List<HolidayModel>> GetHolidayList(HolidayFilterModel model);

        Task<HolidayModel> GetHolidayById(int id);

        Task<bool> AddHoliday(HolidayModel model);

        Task<bool> UpdateHoliday(HolidayModel model);

        Task<bool> DeleteHoliday(int id);
    }
}