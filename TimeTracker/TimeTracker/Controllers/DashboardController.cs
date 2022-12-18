using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}