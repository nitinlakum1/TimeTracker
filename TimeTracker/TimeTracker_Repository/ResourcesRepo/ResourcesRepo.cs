using AutoMapper;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model.Holiday;
using TimeTracker_Model.Resources;

namespace TimeTracker_Repository.ResourcesRepo
{
    public class ResourcesRepo : IResourcesRepo
    {
        #region Declaration
        private readonly ResourcesData _resourcesData;
        private readonly IMapper _mapper;
        #endregion

        #region Const
        public ResourcesRepo(ResourcesData resourcesData, IMapper mapper)
        {
            _resourcesData = resourcesData;
            _mapper = mapper;
        }
        #endregion

        #region Methods

        public async Task<(List<ResourcesModel>, int)> GetResourcesList(ResourcesFilterModel model)
        {
            var (userList, totalRecord) = await _resourcesData.GetResourcesList(model);

            var systemLogs = _mapper.Map<List<ResourcesModel>>(userList);

            return (systemLogs, totalRecord);
        }

        public async Task<bool> AddRemarks(ResourcerRemarksModel model)
        {
            var remarks = _mapper.Map<ResourcesRemarks>(model);

            var resources = await _resourcesData.GetResourceById(model.PreferenceId ?? "");
            if (resources != null && !string.IsNullOrWhiteSpace(resources.preferenceId))
            {
                resources.ResourceStatus = model.ResourceStatus;
                await _resourcesData.EditResource(resources);
            }
            return await _resourcesData.AddRemarks(remarks);
        }

        #endregion
    }
}