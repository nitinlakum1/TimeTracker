using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TimeTracker_Model;
using TimeTracker_Model.User;

namespace TimeTracker_Repository
{
    public class TokenService : ITokenService
    {
        public UserToken BuildToken(LoginDetailsModel user, JwtSettingModel jwtSettings)
        {
            DateTime expiredTime = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryDurationMinutes);
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email??""),
                //new Claim(ClaimTypes.Role, user.RoleName),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Expiration, expiredTime.ToString("dd MMMM, yyyy : tt"))
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new JwtSecurityToken(
                jwtSettings.ValidIssuer, jwtSettings.ValidAudience, claims,
                expires: expiredTime,
                signingCredentials: credentials);

            var userToken = new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor),
                UserName = user.Username,
                Id = user.Id,
                Email = user.Email,
                ExpiredTime = expiredTime,
                RoleId = user.RoleId,
                RoleName = user.RoleName,
                ProfilePic = user.ProfilePic,
                GuidId = Guid.NewGuid()
            };
            return userToken;
        }

        public bool IsTokenValid(string key, string issuer, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = issuer,
                        ValidAudience = issuer,
                        IssuerSigningKey = mySecurityKey,
                    }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
