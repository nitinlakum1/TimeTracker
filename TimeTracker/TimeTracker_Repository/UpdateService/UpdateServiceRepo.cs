using AutoMapper;
using TimeTracker_Data.Model;
using TimeTracker_Data.Modules;
using TimeTracker_Model.UpdateService;

namespace TimeTracker_Repository
{
    public class UpdateServiceRepo : IUpdateServiceRepo
    {
        #region Declaration
        private readonly UpdateServiceData _updateServiceData;
        private readonly IMapper _mapper;
        #endregion

        #region Const
        public UpdateServiceRepo(UpdateServiceData updateServiceData, IMapper mapper)
        {
            _updateServiceData = updateServiceData;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<List<UpdateServiceModel>> GetUpdateServiceList(UpdateServiceFilterModel model)
        {
            var updateServiceList = await _updateServiceData.GetUpdateServiceList(model);
            return _mapper.Map<List<UpdateServiceModel>>(updateServiceList);
        }

        public async Task<bool> AddUpdateService(UpdateServiceModel model)
        {
            var result = _mapper.Map<UpdateServices>(model);
            return await _updateServiceData.AddUpdateService(result);
        }

        public async Task<bool> DeleteUpdateService(int id)
        {
            return await _updateServiceData.DeleteUpdateService(id);
        }

        #endregion
    }
}