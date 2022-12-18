using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TimeTracker.Models;
using TimeTracker_Model;
using TimeTracker_Model.User;
using TimeTracker_Repository;
using TimeTracker_Repository.UserRepo;

namespace TimeTracker.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly JwtSettingModel _jwtSettings;

        public LoginController(IUserRepo userRepo,
                              IMapper mapper,
                              ITokenService tokenService,
                              IOptions<JwtSettingModel> jwtSettings)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                var loginModel = _mapper.Map<LoginModel>(model);
                var result = await _userRepo.ValidateUser(loginModel);

                if (result != null && result.Id > 0)
                {
                    UserToken generatedToken = _tokenService.BuildToken(result, _jwtSettings);
                    if (generatedToken != null)
                    {
                        HttpContext.Session.SetString("Token", generatedToken.Token);
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        //TODO: Set validation. | Display error message to user.
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    //TODO: Set validation. | Display error message to user.
                    return View("Index");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}