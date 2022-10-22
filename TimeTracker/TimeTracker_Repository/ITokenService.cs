using TimeTracker_Model;
using TimeTracker_Model.User;

namespace TimeTracker_Repository
{
    public interface ITokenService
    {
        UserToken BuildToken(LoginDetailsModel user, JwtSettingModel jwtSettings);
        bool IsTokenValid(string key, string issuer, string token);
    }
}