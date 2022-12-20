using System.ComponentModel;

namespace TimeTracker.Helper
{
    public static class CommonHelper
    {
        public static string GetEnumDescription(this Enum value)
        {
            if (value != null)
            {
                // get attributes  
                var field = value.GetType().GetField(value.ToString());
                var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                // return description
                return attributes.Any() ? ((DescriptionAttribute)attributes.ElementAt(0)).Description : "Description Not Found";
            }
            return "N/A";
        }
        //public static string IsLogin(this Enum value)
        //{

        //}
    }
}