using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TimeTracker.Models;
using TimeTracker.Models.Salary;
using TimeTracker_Data.Migrations;
using TimeTracker_Model.Salary;
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
        #endregion

        #region Const
        public SalaryController(ISalaryRepo salaryRepo, IMapper mapper, IUserRepo userRepo)
        {
            _salaryRepo = salaryRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        } 
        #endregion

        #region Method
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadSalary(DatatableParamViewModel param)
        {
            var dtParam = _mapper.Map<SalaryFilterModel>(param);

            var (salaryList, totalRecord) = await _salaryRepo.GetSalary(dtParam);

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecord,
                iTotalDisplayRecords = totalRecord,
                aaData = salaryList
            });
        }

        public async Task<IActionResult> Create()
        {
            var users = await _userRepo.GetUserLookup();
            ViewBag.Users = new SelectList(users, "Id", "Username");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SalaryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
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

        public async Task<IActionResult> Update(int id)
        {
            var salary = await _salaryRepo.GetSalaryById(id);
            var model = _mapper.Map<EditSalaryViewModel>(salary);
            return View(model);
        }

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