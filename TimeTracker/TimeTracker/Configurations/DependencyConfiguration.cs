using TimeTracker_Data.Modules;
using TimeTracker_Repository;

namespace TimeTracker.Configurations
{
    public static class DependencyConfiguration
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddTransient<IUserRepo, UserRepo>();

            services.AddTransient<UserData>();
        }
    }
}