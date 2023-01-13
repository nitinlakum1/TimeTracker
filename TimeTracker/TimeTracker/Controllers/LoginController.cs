using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using TimeTracker.Models.Login;
using TimeTracker_Model;
using TimeTracker_Model.Holiday;
using TimeTracker_Model.User;
using TimeTracker_Repository;
using TimeTracker_Repository.UserRepo;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TimeTracker.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly JwtSettingModel _jwtSettings;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(IUserRepo userRepo,
                              IMapper mapper,
                              ITokenService tokenService,
                              IOptions<JwtSettingModel> jwtSettings,
                              IHttpContextAccessor httpContextAccessor,
                              Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var isAuthenticated = _httpContextAccessor?
                .HttpContext?.User?
                .Identity?.IsAuthenticated;

            if (isAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }
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

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            string key = string.Format("{0}", Guid.NewGuid().ToString().Replace("-", ""));

            _userRepo.UpdateKey(model.Email, key);

            string returnUrl = string.Format("https://{0}{1}", Request.Host.Value, Url.Action("CreatePassword", "Login", new { email = model.Email, key }));
            string filePath = Path.Combine(_hostingEnvironment.WebRootPath, @"Template\Email\ResetPassword.html");

            var fromEmail = new MailAddress("nitinb@capitalnumbers.com");
            var toEmail = new MailAddress(model.Email);
            var password = "lakum@123";
            var htmlFileData = System.IO.File.ReadAllText(filePath);
            htmlFileData = htmlFileData.Replace("{returnUrl}", returnUrl);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, password)
            };

            var mess = new MailMessage(fromEmail, toEmail);
            mess.Subject = "Subject";
            mess.IsBodyHtml = true;
            mess.Body = htmlFileData;
            smtp.Send(mess);

            return View();
        }

        public async Task<IActionResult> CreatePassword(string email, string key)
        {
            var GetKey = await _userRepo.GetKey(email);
            ViewBag.Email = email;
            if (GetKey == key)
            {
            return View();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePassword(CreatePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var createPassword = _mapper.Map<CreatePasswordModel>(model);
                    var result = await _userRepo.CreatePassword(createPassword);

                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
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

        public async Task<IActionResult> ValidateEmailForgotPass(string Email)
        {
            return Json(await _userRepo.ValidateEmailForgotPass(Email));
        }

        public async Task<IActionResult> ValidateUser(string Username)
        {
            return Json(await _userRepo.ValidateUser(Username));
        }

        public async Task<IActionResult> ValidatePassword(string Password, string Username)
        {
            return Json(await _userRepo.ValidatePassword(Password, Username));
        }
    }
}