using AutoMapper;
using Newtonsoft.Json;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model.Holiday;
using TimeTracker_Model.Resources;

namespace TimeTracker_Repository.ResourcesRepo
{
    public class ResourceRepo : IResourceRepo
    {
        #region Declaration
        private readonly ResourcesData _resourcesData;
        private readonly IMapper _mapper;
        #endregion

        #region Const
        public ResourceRepo(ResourcesData resourcesData, IMapper mapper)
        {
            _resourcesData = resourcesData;
            _mapper = mapper;
        }
        #endregion

        #region Methods

        public async Task<ResourceModel> GetResourceById(string id)
        {
            var result = await _resourcesData.GetResourceById(id);
            return _mapper.Map<ResourceModel>(result);
        }

        public async Task<List<ResourceModel>> GetResources()
        {
            var result = await _resourcesData.GetResources();
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
                CreatedOn = DateTime.Now
            };

            if (!string.IsNullOrWhiteSpace(model.city))
            {
                var preferences = JsonConvert.DeserializeObject<List<Preferences>>(model.city);
                if (preferences is { Count: > 0 })
                {
                    var data = preferences.FirstOrDefault();
                    resource.city = data.city;

                    if (string.IsNullOrWhiteSpace(resource.designation))
                    {
                        resource.designation = data.channel;
                    }
                    else
                    {
                        resource.designation = string.Format("{0} | {1}", resource.designation, data.channel);
                    }
                }
            }
            return await _resourcesData.AddResources(resource);
        }

        public async Task<bool> EditDesignation(ResourceModel model)
        {
            var resource = await _resourcesData.GetResourceById(model.id);

            if (!string.IsNullOrWhiteSpace(model.city))
            {
                var preferences = JsonConvert.DeserializeObject<List<Preferences>>(model.city);
                if (preferences is { Count: > 0 })
                {
                    var data = preferences.FirstOrDefault();
                    resource.city = data.city;

                    if (string.IsNullOrWhiteSpace(model.designation))
                    {
                        resource.designation = data.channel;
                    }
                    else
                    {
                        resource.designation = string.Format("{0} | {1}", model.designation, data.channel);
                    }
                }
            }
            resource.CreatedOn = DateTime.Now;
            return await _resourcesData.EditResource(resource);
        }

        public async Task<bool> EditResource(ResourceModel model)
        {
            var result = await _resourcesData.GetResourceById(model.id);
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
            return await _resourcesData.EditResource(result);
        }

        public async Task<(List<ResourceListModel>, int)> GetResourcesList(ResourceFilterModel model)
        {
            var (result, totalRecord) = await _resourcesData.GetResourcesList(model);

            var resourceList = _mapper.Map<List<ResourceListModel>>(result);

            return (resourceList, totalRecord);
        }

        public async Task<bool> AddFollowup(FollowupModel model)
        {
            var remarks = _mapper.Map<ResourcesRemarks>(model);

            var resources = await _resourcesData.GetResourceByPrId(model.PreferenceId ?? "");
            if (resources != null && !string.IsNullOrWhiteSpace(resources.preferenceId))
            {
                resources.ResourceStatus = model.ResourceStatus;
                await _resourcesData.EditResourceStatus(resources);
            }
            return await _resourcesData.AddFollowup(remarks);
        }

        public async Task<List<FollowupListModel>> GetFollowupList(string id)
        {
            var result = await _resourcesData.GetFollowupList(id);

            return _mapper.Map<List<FollowupListModel>>(result);
        }
        #endregion
    }
}