using AutoMapper;
using Newtonsoft.Json;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model.Setting;

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

        public async Task<ResourceModel> GetResourceById(string id)
        {
            var result = await _settingData.GetResourceById(id);
            return _mapper.Map<ResourceModel>(result);
        }

        public async Task<List<ResourceModel>> GetResources()
        {
            var result = await _settingData.GetResources();
            return _mapper.Map<List<ResourceModel>>(result);
        }

        public async Task<bool> AddResources(ResourceModel model)
        {
            Resources resource = new Resources()
            {
                id = model.id,
                preferenceId = model.preferenceId,
                name = model.name,
                gender = model.gender,
                mobile = model.mobile,
                email = model.email,
                workYears = model.workYears,
                designation = model.designation,
                degree = model.degree,
                birthDate = model.birthDate,
                workStartDate = model.workStartDate,
                companyExperiences = model.companyExperiences,
                city = model.city,
            };

            if (!string.IsNullOrWhiteSpace(model.city))
            {
                var preferences = JsonConvert.DeserializeObject<List<Preferences>>(model.city);
                if (preferences is { Count: > 0 })
                {
                    var data = preferences.FirstOrDefault();
                    resource.city = data.city;
                    resource.designation = string.IsNullOrWhiteSpace(resource.designation) ? data.channel : resource.designation;
                }
            }
            return await _settingData.AddResources(resource);
        }

        public async Task<bool> EditResource(ResourceModel model)
        {
            var result = await _settingData.GetResourceById(model.id);
            result.name = model.name;
            result.gender = model.gender;
            result.mobile = model.mobile;
            result.email = model.email;
            result.workYears = model.workYears;
            result.designation = model.designation;
            result.degree = model.degree;
            result.birthDate = model.birthDate;
            result.workStartDate = model.workStartDate;
            result.companyExperiences = model.companyExperiences;
            result.city = model.city;
            return await _settingData.EditResource(result);
        }
        #endregion
    }
}