using AutoMapper;
using TimeTracker_Data.Modules;
using TimeTracker_Model.Holiday;

namespace TimeTracker_Repository
{
    public class SettingRepo : ISettingRepo
    {
        #region Declaration
        private readonly SettingData _settingData;
        private readonly IMapper _mapper;
        #endregion

        #region Const
        public SettingRepo(SettingData SettingData, IMapper mapper)
        {
            _settingData = SettingData;
            _mapper = mapper;
        }
        #endregion

        #region Methods

        public async Task<(List<SettingModel>, int)> SettingList(SettingFilterModel model)
        {
            var (userList, totalRecord) = await _settingData.SettingList(model);

            var systemLogs = _mapper.Map<List<SettingModel>>(userList);

            return (systemLogs, totalRecord);
        }

        public async Task<SettingModel> GetSettingById(int id)
        {
            var result = await _settingData.GetSettingById(id);
            return _mapper.Map<SettingModel>(result);
        }

        public async Task<bool> UpdateSetting(SettingModel model)
        {
            var result = await _settingData.GetSettingById(model.Id);
            result.Value = model.Value;
            result.IsActive = model.IsActive;

            return await _settingData.UpdateSetting(result);
        }
        #endregion
    }
}