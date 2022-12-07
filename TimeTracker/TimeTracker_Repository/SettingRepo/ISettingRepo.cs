using TimeTracker_Model.Holiday;
using TimeTracker_Model.Setting;

namespace TimeTracker_Repository
{
    public interface ISettingRepo
    {
        Task<(List<SettingModel>, int)> SettingList(SettingFilterModel model);

        Task<SettingModel> GetSettingById(int id);

        Task<bool> UpdateSetting(SettingModel model);

        Task<ResourceModel> GetResourceById(string id);

        Task<List<ResourceModel>> GetResources();

        Task<bool> AddResources(ResourceModel model);

        Task<bool> EditResource(ResourceModel model);
    }
}