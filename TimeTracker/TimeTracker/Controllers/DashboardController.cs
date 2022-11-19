using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Models.SystemLog;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            string[] text = System.IO.File.ReadAllLines(@"C:\Program Files\WCT\18_11_22.txt");
            //ViewBag.mainproduct = text;

            var logs = new List<SystemLogListModel>();

            foreach (var item in text)
            {
                logs.Add(new SystemLogListModel()
                {
                    LogTime = Convert.ToDateTime(item.Split("|")[0].Trim()),
                    Description = item.Split("|")[1].Trim(),
                });
            }
            return View(logs);
        }
    }
}
