using AutoMapper;
using TimeTracker.Models;
using TimeTracker_Data.Model;
using TimeTracker_Model.User;

namespace TimeTracker.AutoMapper
{
    public class UserMappings
    {
        public static void Map(Profile profile)
        {
            profile.CreateMap<LoginViewModel, LoginModel>();
            profile.CreateMap<User, LoginDetailsModel>();
        }
    }
}