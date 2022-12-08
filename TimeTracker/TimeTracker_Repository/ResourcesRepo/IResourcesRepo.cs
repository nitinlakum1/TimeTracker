using TimeTracker_Model.Resources;
using TimeTracker_Model.Setting;
using TimeTracker_Model.User;

namespace TimeTracker_Repository.ResourcesRepo
{
    public interface IResourcesRepo
    {
        Task<(List<ResourcesModel>, int)> GetResourcesList(ResourcesFilterModel model);

    }
}