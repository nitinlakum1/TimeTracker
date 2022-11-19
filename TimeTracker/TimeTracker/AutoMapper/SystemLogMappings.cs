using AutoMapper;
using TimeTracker.Models;
using TimeTracker.Models.SystemLog;
using TimeTracker_Data.Model;
using TimeTracker_Model.SystemLog;

namespace TimeTracker.AutoMapper
{
    public class SystemLogMappings
    {
        public static void Map(Profile profile)
        {
            profile.CreateMap<SystemLogs, SystemLogModel>();
            profile.CreateMap<SystemLogModel, SystemLogListModel>();
            profile.CreateMap<DatatableParamViewModel, SystemLogFilterModel>()
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