using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TimeTracker.Configurations;
using TimeTracker.Helper;
using TimeTracker.Models;
using TimeTracker.Models.User;
using TimeTracker_Data.Model;
using TimeTracker_Model;
using TimeTracker_Model.User;
using TimeTracker_Repository.AWSRepo;
using TimeTracker_Repository.UserRepo;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAWSS3BucketService _awsS3BucketService;

        public UserController(IUserRepo userRepo,
                              IMapper mapper,
                              IHttpContextAccessor httpContextAccessor,
                              IOptions<AwsConfiguration> awsConfiguration)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _awsS3BucketService = new AWSS3BucketService(awsConfiguration.Value);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LoadUser(DatatableParamViewModel param)
        {
            var dtParam = _mapper.Map<UserFilterModel>(param);

            var (userList, totalRecord) = await _userRepo.GetUserList(dtParam);

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecord,
                iTotalDisplayRecords = totalRecord,
                aaData = userList
            });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Url = await _awsS3BucketService.UploadFile(model.AvatarFile, "");
                    var addUser = _mapper.Map<AddEditUserModel>(model);
                    var result = await _userRepo.AddUser(addUser);

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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _userRepo.GetUserById(id);
            var model = _mapper.Map<EditUserViewModel>(user);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(EditUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Url = await _awsS3BucketService.UploadFile(
                        model.AvatarFile, model.Url ?? "");

                    var editUser = _mapper.Map<AddEditUserModel>(model);
                    editUser.RoleId = _httpContextAccessor?.HttpContext?.User?.GetLoginRole() ?? 0;

                    var result = await _userRepo.UpdateUser(editUser);
                    if (result)
                    {
                        if (model.FromProfile)
                        { return RedirectToAction("UserProfile"); }
                        else
                        { return RedirectToAction("Index"); }
                    }
                }
                if (model.FromProfile)
                { return View("UserProfile"); }
                else
                { return View(); }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool isSuccess = false;
            string message = "";
            try
            {
                if (id > 0)
                {
                    isSuccess = await _userRepo.DeleteUser(id);
                    message = isSuccess ? AppMessages.DELETE_SUCCESS : AppMessages.SOMETHING_WRONG;
                }
            }
            catch (Exception ex)
            {
                //LogWriter.LogWrite(ex.Message, MessageTypes.Error);
            }
            return Json(new { isSuccess, message });
        }

        public async Task<IActionResult> UserProfile()
        {
            int? userId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim();
            var user = await _userRepo.GetUserById(userId ?? 0);
            var pic = user.Url;
            var model = _mapper.Map<EditUserViewModel>(user);
            return View(model);
        }

        public async Task<IActionResult> DeleteProfilePic(int id, string url)
        {
            bool isSuccess = false;
            string message = "";
            try
            {
                if (id > 0)
                {
                    await _awsS3BucketService.DeleteFile(url);
                    isSuccess = await _userRepo.DeleteProfilePic(id);
                    message = isSuccess ? AppMessages.DELETE_SUCCESS : AppMessages.SOMETHING_WRONG;
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex);
            }
            return Json(new { isSuccess, message });
        }

        [HttpGet]
        public async Task<IActionResult> GetProfilePic()
        {
            string url = "";
            try
            {
                int? userId = _httpContextAccessor?.HttpContext?
                    .User.GetIdFromClaim();
                var user = await _userRepo.GetUserById(userId ?? 0);
                url = user?.Url ?? "";
            }
            catch { }
            return Json(url);
        }
    }
}