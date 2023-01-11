using TimeTracker_Model.UpdateService;

namespace TimeTracker_Repository
{
    public interface IUpdateServiceRepo
    {
        Task<List<UpdateServiceModel>> GetUpdateServiceList(UpdateServiceFilterModel model);

        Task<bool> AddUpdateService(UpdateServiceModel model);

        Task<bool> DeleteUpdateService(int id);
    }
}