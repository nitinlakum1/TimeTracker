using AutoMapper;
using Newtonsoft.Json;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model.Holiday;
using TimeTracker_Model.Resources;
using TimeTracker_Model.Setting;

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
            var (result, totalRecord) = await _resourcesData.GetResourcesList(model);

            var resourceList = _mapper.Map<List<ResourcesModel>>(result);

            return (resourceList, totalRecord);
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

        public async Task<List<FollowupListModel>> GetFollowupList(string id)
        {
            var result = await _resourcesData.GetFollowupList(id);

            return _mapper.Map<List<FollowupListModel>>(result);
        }
        #endregion
    }
}