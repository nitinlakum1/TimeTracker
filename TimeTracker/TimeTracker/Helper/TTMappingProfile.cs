using AutoMapper;
using TimeTracker.AutoMapper;

namespace TimeTracker.Helper
{
    public class TTMappingProfile : Profile
    {
        public TTMappingProfile()
        {
            UserMappings.Map(this);
            SystemLogMappings.Map(this);
        }
    }
}
