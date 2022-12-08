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
            SettingMappings.Map(this);
            HolidayMappings.Map(this);
            SalaryMappings.Map(this);
            ResourceMappings.Map(this);
        }
    }
}
