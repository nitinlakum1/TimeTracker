using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class CommonController : Controller
    {
        public CommonController()
        {
        }
    }
}