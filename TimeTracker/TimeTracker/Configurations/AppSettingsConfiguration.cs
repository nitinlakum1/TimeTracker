using TimeTracker_Model;

namespace TimeTracker.Configurations
{
    public static class AppSettingsConfiguration
    {
        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettingModel>(options =>
            {
                options.ValidateIssuerSigningKey = Convert.ToBoolean(configuration.GetSection("JWTKeys:ValidateIssuerSigningKey").Value);
                options.IssuerSigningKey = configuration.GetSection("JWTKeys:IssuerSigningKey").Value;
                options.ValidateIssuer = Convert.ToBoolean(configuration.GetSection("JWTKeys:ValidateIssuer").Value);
                options.ValidIssuer = configuration.GetSection("JWTKeys:ValidIssuer").Value;
                options.ValidateAudience = Convert.ToBoolean(configuration.GetSection("JWTKeys:ValidateAudience").Value);
                options.ValidAudience = configuration.GetSection("JWTKeys:ValidAudience").Value;
                options.RequireExpirationTime = Convert.ToBoolean(configuration.GetSection("JWTKeys:RequireExpirationTime").Value);
                options.ValidateLifetime = Convert.ToBoolean(configuration.GetSection("JWTKeys:ValidateLifetime").Value);
                options.ExpiryDurationMinutes = Convert.ToInt32(configuration.GetSection("JWTKeys:ExpiryDurationMinutes").Value);
            });
        }
    }
}