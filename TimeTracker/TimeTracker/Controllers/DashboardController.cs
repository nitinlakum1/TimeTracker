using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;
using System.Text;
using TimeTracker.Models.Setting;
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