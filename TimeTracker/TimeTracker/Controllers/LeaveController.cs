using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TimeTracker.Helper;
using TimeTracker.Models;
using TimeTracker.Models.Leave;
using TimeTracker_Data.Model;
using TimeTracker_Model;
using TimeTracker_Model.Leave;
using TimeTracker_Repository.LeaveRepo;
using TimeTracker_Repository.UserRepo;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class LeaveController : Controller
    {
        #region Declaration
        private readonly ILeaveRepo _leaveRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepo _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Const
        public LeaveController(ILeaveRepo leaveRepo, IMapper mapper, IUserRepo userRepo, IHttpContextAccessor httpContextAccessor)
        {
            _leaveRepo = leaveRepo;
            _mapper = mapper;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Method
        public async Task<IActionResult> Index()
        {
            int userId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim() ?? 0;
            var LeaveCount = await _leaveRepo.LeaveCount(userId);
            ViewBag.Count = LeaveCount;


            var users = await _userRepo.GetUserLookup();
            ViewBag.Users = new SelectList(users, "Id", "Username");
            return View();
        }

        public async Task<IActionResult> LoadLeave(DatatableParamViewModel param, string filter)
        {
            var dtParam = _mapper.Map<LeaveFilterModel>(param);

            int userId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim() ?? 0;
            dtParam.UserId = userId;

            if (!string.IsNullOrWhiteSpace(filter) && filter != "{}")
            {
                var filterData = JsonConvert.DeserializeObject<LeaveFilterModel>(filter);
                dtParam.UserId = filterData?.UserId == null || filterData?.UserId == 0 ? userId : filterData?.UserId;
            }

            var (leaveList, totalRecord) = await _leaveRepo.GetLeave(dtParam);
            var lst = _mapper.Map<List<LeaveListViewModel>>(leaveList);

            var Data = JsonConvert.DeserializeObject<LeaveFilterModel>(filter);
            int? Id = Data?.UserId == null || Data?.UserId == 0 ? userId : Data?.UserId;
            var LeaveCount = await _leaveRepo.LeaveCount(Id);
            lst = lst.Select(a => { a.PendingLeave = (12 - LeaveCount); return a; }).ToList();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecord,
                iTotalDisplayRecords = totalRecord,
                aaData = lst
            });
        }

        public async Task<IActionResult> Create()
        {
            int userId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim() ?? 0;
            var users = await _userRepo.GetUserLookup();
            var currentUser = users.Where(a => a.Id == userId);
            int roleId = User.GetLoginRole();
            if (roleId == (int)TimeTracker_Model.Roles.Admin)
            {
                ViewBag.Users = new SelectList(users, "Id", "Username");
            }
            else
            {
                ViewBag.Users = new SelectList(currentUser, "Id", "Username");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddLeaveViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int userId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim() ?? 0;
                    int roleId = User.GetLoginRole();
                    if (roleId != (int)TimeTracker_Model.Roles.Admin)
                    {
                        model.UserId = userId;
                    }
                    var leaveCount = await _leaveRepo.LeaveCount(model.UserId);

                    var addLeave = _mapper.Map<AddLeaveModel>(model);
                    addLeave.IsPaid = leaveCount <= 12;
                    var result = await _leaveRepo.AddLeave(addLeave);

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
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, int btnId)
        {
            bool isSuccess = false;
            string message = "";
            try
            {
                if (id > 0)
                {
                    isSuccess = await _leaveRepo.ChangeStatus(id, btnId);
                    message = isSuccess ? AppMessages.SAVE_SUCCESS : AppMessages.SOMETHING_WRONG;
                }
            }
            catch (Exception)
            {
                throw;
                //LogWriter.LogWrite(ex.Message, MessageTypes.Error);
            }
            return Json(new { isSuccess, message });
        }

        public async Task<IActionResult> GetLeaveDetail(int id)
        {
            try
            {
                var leaveDetail = new List<LeaveListViewModel>();
                if (id > 0)
                {
                    var result = await _leaveRepo.GetLeaveDetail(id);
                    leaveDetail = _mapper.Map<List<LeaveListViewModel>>(result);
                }
                return PartialView("_LeaveDetail", leaveDetail);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}