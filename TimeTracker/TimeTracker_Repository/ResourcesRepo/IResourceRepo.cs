using TimeTracker_Model.Resources;

namespace TimeTracker_Repository.ResourcesRepo
{
    public interface IResourceRepo
    {
        Task<ResourceModel> GetResourceById(string id);

        Task<List<ResourceModel>> GetResources();

        Task<bool> AddResources(ResourceModel model);

        Task<bool> EditDesignation(ResourceModel model);

        Task<bool> EditResource(ResourceModel model);

        Task<(List<ResourceListModel>, int)> GetResourcesList(ResourceFilterModel model);

        Task<bool> AddFollowup(FollowupModel model);

        Task<List<FollowupListModel>> GetFollowupList(string id);
    }
}