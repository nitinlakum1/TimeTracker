using AutoMapper;
using TimeTracker.Models;
using TimeTracker.Models.Salary;
using TimeTracker.Models.User;
using TimeTracker_Data.Model;
using TimeTracker_Model.Salary;
using TimeTracker_Model.User;

namespace TimeTracker.AutoMapper
{
    public class SalaryMappings
    {

        public static void Map(Profile profile)
        {
            profile.CreateMap<SalaryModel, EditSalaryViewModel>();
            profile.CreateMap<EditSalaryViewModel, AddEditSalaryModel>();
            profile.CreateMap<SalaryViewModel, AddEditSalaryModel>();
            profile.CreateMap<AddEditSalaryModel, Salarys>();
            profile.CreateMap<Salarys, SalaryModel>()
               .ForMember(dest => dest.Username, source => source.MapFrom(x => x.Users.Username));

            profile.CreateMap<DatatableParamViewModel, SalaryFilterModel>()
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