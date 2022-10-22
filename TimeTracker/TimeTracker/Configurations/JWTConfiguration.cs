using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TimeTracker_Model;

namespace TimeTracker.Configurations
{
    public static class JWTConfiguration
    {
        public static void AddJWTConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Jwt Setings
            var jwtSettings = new JwtSettingModel();
            configuration.Bind("JWTKeys", jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidateLifetime = jwtSettings.ValidateLifetime,
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudience = jwtSettings.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey))
                };
            });
        }
    }
}