using AutoMapper;
using TimeTracker.Models;
using TimeTracker.Models.Holiday;
using TimeTracker.Models.UpdateService;
using TimeTracker.Models.User;
using TimeTracker_Data.Model;
using TimeTracker_Model.Holiday;
using TimeTracker_Model.UpdateService;
using TimeTracker_Model.User;

namespace TimeTracker.AutoMapper
{
    public class UpdateServiceMappings
    {

        public static void Map(Profile profile)
        {
            profile.CreateMap<UpdateServices, UpdateServiceModel>();
            profile.CreateMap<AddUpdateServiceViewModel, UpdateServiceModel>();
            profile.CreateMap<UpdateServiceModel, UpdateServices>();

            profile.CreateMap<DatatableParamViewModel, UpdateServiceFilterModel>()
                .ForMember(source => source.DisplayStart, dest => dest.MapFrom(x => x.iDisplayStart))
                .ForMember(source => source.PageSize, dest => dest.MapFrom(x => x.iDisplayLength))
                .ForMember(source => source.SearchText, dest => dest.MapFrom(x => x.sSearch))
                .ForMember(source => source.SortOrder, dest => dest.MapFrom(x => x.sSortDir_0))
                .ForMember(source => source.SortColumn, dest => dest.MapFrom((src, dest, outcome, context) =>
                {
                    return src.sColumns.Split(',')[src.iSortCol_0];
                }));
        }
    }
}