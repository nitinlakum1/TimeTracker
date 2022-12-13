using AutoMapper;
using TimeTracker.Models;
using TimeTracker.Models.ResourcesRemarks;
using TimeTracker.Models.SystemLog;
using TimeTracker_Data.Model;
using TimeTracker_Model.Resources;
using TimeTracker_Model.Setting;
using TimeTracker_Model.SystemLog;
using TimeTracker_Model.User;

namespace TimeTracker.AutoMapper
{
    public class ResourceMappings
    {
        public static void Map(Profile profile)
        {
            profile.CreateMap<Resources, ResourcesModel>();
            profile.CreateMap<ResourcerRemarksModel, ResourcesRemarks>();
            profile.CreateMap<ResourcesRemarksViewModel, ResourcerRemarksModel>();


            profile.CreateMap<DatatableParamViewModel, ResourcesFilterModel>()
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