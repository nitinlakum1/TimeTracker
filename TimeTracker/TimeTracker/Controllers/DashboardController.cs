using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ISettingRepo _settingRepo;

        public DashboardController(ISettingRepo settingRepo)
        {
            _settingRepo = settingRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}