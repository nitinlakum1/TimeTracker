using TimeTracker_Data.Modules;

namespace TimeTracker_Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly UserData _userData;
        public UserRepo(UserData userData)
        {
            _userData = userData;
        }
    }
}