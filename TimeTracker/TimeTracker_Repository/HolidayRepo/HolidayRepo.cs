using AutoMapper;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model.Holiday;

namespace TimeTracker_Repository
{
    public class HolidayRepo : IHolidayRepo
    {
        #region Declaration
        private readonly HolidayData _holidayData;
        private readonly IMapper _mapper;
        #endregion

        #region Const
        public HolidayRepo(HolidayData HolidayData, IMapper mapper)
        {
            _holidayData = HolidayData;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<List<HolidayModel>> GetHolidayList(HolidayFilterModel model)
        {
            var holidayList = await _holidayData.GetHolidayList(model);
            return _mapper.Map<List<HolidayModel>>(holidayList);
        }

        public async Task<HolidayModel> GetHolidayById(int id)
        {
            var result = await _holidayData.GetHolidayById(id);
            return _mapper.Map<HolidayModel>(result);
        }

        public async Task<bool> AddHoliday(HolidayModel model)
        {
            var result = _mapper.Map<Holidays>(model);
            return await _holidayData.AddHoliday(result);
        }

        public async Task<bool> UpdateHoliday(HolidayModel model)
        {
            var result = _mapper.Map<Holidays>(model);
            return await _holidayData.UpdateHoliday(result);
        }

        public async Task<bool> DeleteHoliday(int id)
        {
            return await _holidayData.DeleteHoliday(id);
        }

        #endregion
    }
}