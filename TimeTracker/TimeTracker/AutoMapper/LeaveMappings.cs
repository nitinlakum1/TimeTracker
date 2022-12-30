using AutoMapper;
using TimeTracker.Models;
using TimeTracker.Models.Leave;
using TimeTracker.Models.Resource;
using TimeTracker.Models.Salary;
using TimeTracker.Models.User;
using TimeTracker_Data.Model;
using TimeTracker_Model.Leave;
using TimeTracker_Model.Salary;
using TimeTracker_Model.User;

namespace TimeTracker.AutoMapper
{
    public class LeaveMappings
    {

        public static void Map(Profile profile)
        {
            profile.CreateMap<LeaveModel, LeaveListViewModel>();
            profile.CreateMap<AddLeaveViewModel, AddLeaveModel>();
            profile.CreateMap<AddLeaveModel, Leaves>();
            profile.CreateMap<Leaves, LeaveModel>()
               .ForMember(dest => dest.Username, source => source.MapFrom(x => x.Users.Username));

            profile.CreateMap<DatatableParamViewModel, LeaveFilterModel>()
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