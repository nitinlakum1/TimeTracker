using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TimeTracker.Helper
{
    public static class MyHTMLHelpers
    {
        public static IHtmlContent HelloWorldHTMLString(this IHtmlHelper htmlHelper, string data)
        {
            return new HtmlString($"<title>{data} | | WCT HR</title>");
        }

        public static String GetProfilePic(this IHtmlHelper htmlHelper)
        {
            return "₹";
        }
    }
}
