using TimeTracker_Data.Modules;
using TimeTracker_Model;
using TimeTracker_Repository;
using TimeTracker_Repository.ResourcesRepo;
using TimeTracker_Repository.SalaryRepo;
using TimeTracker_Repository.SystemLogRepo;
using TimeTracker_Repository.UserRepo;

namespace TimeTracker.Configurations
{
    public static class DependencyConfiguration
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<ISettingRepo, SettingRepo>();
            services.AddTransient<ISystemLogRepo, SystemLogRepo>();
            services.AddTransient<IHolidayRepo, HolidayRepo>();
            services.AddTransient<ISalaryRepo, SalaryRepo>();
            services.AddTransient<IResourcesRepo, ResourcesRepo>();
            services.AddTransient<JwtSettingModel>();
            services.AddSingleton<AwsConfiguration>();
            //services.AddTransient<IAWSS3BucketService, AWSS3BucketService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<UserData>();
            services.AddTransient<SystemLogData>();
            services.AddTransient<SettingData>();
            services.AddTransient<HolidayData>();
            services.AddTransient<SalaryData>();
            services.AddTransient<ResourcesData>();
        }
    }
}