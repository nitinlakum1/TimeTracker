﻿using TimeTracker_Data.Modules;
using TimeTracker_Model;
using TimeTracker_Repository;

namespace TimeTracker.Configurations
{
    public static class DependencyConfiguration
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<ISettingRepo, SettingRepo>();
            services.AddTransient<ISystemLogRepo, SystemLogRepo>();
            services.AddTransient<JwtSettingModel>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddTransient<UserData>();
            services.AddTransient<SystemLogData>();
            services.AddTransient<SettingData>();
        }
    }
}