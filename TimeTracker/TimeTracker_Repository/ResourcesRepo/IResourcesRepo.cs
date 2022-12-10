using TimeTracker_Model.Resources;

namespace TimeTracker_Repository.ResourcesRepo
{
    public interface IResourcesRepo
    {
        Task<(List<ResourcesModel>, int)> GetResourcesList(ResourcesFilterModel model);

    }
}