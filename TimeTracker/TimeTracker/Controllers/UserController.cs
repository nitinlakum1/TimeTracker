using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TimeTracker.Models;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}