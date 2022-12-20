using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TimeTracker.Helper;
using TimeTracker.Models;
using TimeTracker.Models.Salary;
using TimeTracker_Data.Migrations;
using TimeTracker_Data.Model;
using TimeTracker_Model.Salary;
using TimeTracker_Model.SystemLog;
using TimeTracker_Repository.SalaryRepo;
using TimeTracker_Repository.UserRepo;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class SalaryController : Controller
    {
        #region Declaration
        private readonly ISalaryRepo _salaryRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepo _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Const
        public SalaryController(ISalaryRepo salaryRepo, IMapper mapper, IUserRepo userRepo, IHttpContextAccessor httpContextAccessor)
        {
            _salaryRepo = salaryRepo;
            _mapper = mapper;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
        } 
        #endregion

        #region Method
        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetUserLookup();
            ViewBag.Users = new SelectList(users, "Id", "Username");
            return View();
        }

        public async Task<IActionResult> LoadSalary(DatatableParamViewModel param, string filter)
        {
            var dtParam = _mapper.Map<SalaryFilterModel>(param);

            int userId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim() ?? 0;
            dtParam.UserId = userId;

            if (!string.IsNullOrWhiteSpace(filter) && filter != "{}")
            {
                var filterData = JsonConvert.DeserializeObject<SalaryFilterModel>(filter);
                dtParam.UserId = filterData?.UserId == null || filterData?.UserId == 0 ? userId : filterData?.UserId;
            }

            var (salaryList, totalRecord) = await _salaryRepo.GetSalary(dtParam);

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecord,
                iTotalDisplayRecords = totalRecord,
                aaData = salaryList
            });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var users = await _userRepo.GetUserLookup();
            ViewBag.Users = new SelectList(users, "Id", "Username");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(SalaryViewModel model)
        {
            try
            {
                if (ModelState.IsValid && model.FromDate < model.ToDate)
                {
                    var addSalary = _mapper.Map<AddEditSalaryModel>(model);
                    var result = await _salaryRepo.AddSalary(addSalary);

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
            var updateSalary = await _salaryRepo.GetSalaryById(id);
            var model = _mapper.Map<EditSalaryViewModel>(updateSalary);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(EditSalaryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var editSalary = _mapper.Map<AddEditSalaryModel>(model);
                    var result = await _salaryRepo.UpdateSalary(editSalary);

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
        #endregion

    }
}