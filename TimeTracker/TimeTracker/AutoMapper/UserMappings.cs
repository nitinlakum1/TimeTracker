using AutoMapper;
using TimeTracker.Models;
using TimeTracker.Models.Login;
using TimeTracker.Models.User;
using TimeTracker_Data.Model;
using TimeTracker_Model.User;

namespace TimeTracker.AutoMapper
{
    public class UserMappings
    {

        public static void Map(Profile profile)
        {
            profile.CreateMap<LoginViewModel, LoginModel>();
            profile.CreateMap<Users, LoginDetailsModel>()
                .ForMember(dest => dest.RoleName, source => source.MapFrom(x => x.Roles.Name));

            profile.CreateMap<Users, UserModel>();
            profile.CreateMap<AddEditUserModel, Users>();
            profile.CreateMap<AddUserViewModel, AddEditUserModel>();
            profile.CreateMap<EditUserViewModel, AddEditUserModel>();
            profile.CreateMap<UserModel, EditUserViewModel>();

            profile.CreateMap<DatatableParamViewModel, UserFilterModel>()
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