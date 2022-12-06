using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Helper;
using TimeTracker.Models;
using TimeTracker.Models.User;
using TimeTracker_Model;
using TimeTracker_Model.User;
using TimeTracker_Repository.UserRepo;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserRepo userRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

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

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
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

        public async Task<IActionResult> Update(int id)
        {
            var user = await _userRepo.GetUserById(id);
            var model = _mapper.Map<EditUserViewModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var editUser = _mapper.Map<AddEditUserModel>(model);
                    var result = await _userRepo.UpdateUser(editUser);

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
            var userDetails = await _userRepo.GetUserDetails(userId ?? 0);
            var Name = userDetails.Select(a => a.FullName);
            return View();
        }
    }
}