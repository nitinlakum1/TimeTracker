using AutoMapper;
using TimeTracker.Models;
using TimeTracker.Models.Setting;
using TimeTracker_Data.Model;
using TimeTracker_Model.Holiday;
using TimeTracker_Model.Resources;
using TimeTracker_Model.Setting;

namespace TimeTracker.AutoMapper
{
    public class SettingMappings
    {
        public static void Map(Profile profile)
        {
            profile.CreateMap<Settings, SettingModel>();
            profile.CreateMap<SettingModel, SettingViewModel>();
            profile.CreateMap<EditSettingViewModel, SettingModel>();
            profile.CreateMap<SettingModel, EditSettingViewModel>();
            profile.CreateMap<Settings, SettingModel>();
            
            //profile.CreateMap<SettingModel, Settings>();
            profile.CreateMap<DatatableParamViewModel, SettingFilterModel>()
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