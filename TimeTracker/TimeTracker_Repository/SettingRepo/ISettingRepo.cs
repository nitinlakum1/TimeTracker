using TimeTracker_Model.Holiday;

namespace TimeTracker_Repository
{
    public interface ISettingRepo
    {
        Task<(List<SettingModel>, int)> SettingList(SettingFilterModel model);

        Task<SettingModel> GetSettingById(int id);

        Task<bool> UpdateSetting(SettingModel model);

        Task<ResourceModel> GetResourceById(string id);

        Task<bool> AddResources(string dataId, string preferenceId, string data);
    }
}