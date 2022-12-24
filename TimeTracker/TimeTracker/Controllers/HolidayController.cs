using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimeTracker.Helper;
using TimeTracker.Models;
using TimeTracker.Models.Holiday;
using TimeTracker.Models.User;
using TimeTracker_Model;
using TimeTracker_Model.Holiday;
using TimeTracker_Model.User;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class HolidayController : Controller
    {
        private readonly IHolidayRepo _holidayRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HolidayController(IHolidayRepo holidayRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _holidayRepo = holidayRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadHoliday(DatatableParamViewModel param, string filter)
        {
            try
            {
                var dtParam = _mapper.Map<HolidayFilterModel>(param);

                if (!string.IsNullOrWhiteSpace(filter) && filter != "{}")
                {
                    var filterData = JsonConvert.DeserializeObject<HolidayFilterModel>(filter);
                    dtParam.Year = filterData?.Year ?? 0;
                }

                var holidayList = await _holidayRepo.GetHolidayList(dtParam);

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = holidayList
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(HolidayViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var addHoliday = _mapper.Map<HolidayModel>(model);
                    var result = await _holidayRepo.AddHoliday(addHoliday);

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
            var holiday = await _holidayRepo.GetHolidayById(id);
            var model = _mapper.Map<HolidayViewModel>(holiday);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(HolidayViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var editHoliday = _mapper.Map<HolidayModel>(model);
                    var result = await _holidayRepo.UpdateHoliday(editHoliday);

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
        public async Task<IActionResult> DeleteHoliday(int id)
        {
            bool isSuccess = false;
            string message = "";
            try
            {
                if (id > 0)
                {
                    isSuccess = await _holidayRepo.DeleteHoliday(id);
                    message = isSuccess ? AppMessages.DELETE_SUCCESS : AppMessages.SOMETHING_WRONG;
                }
            }
            catch (Exception ex)
            {
                //LogWriter.LogWrite(ex.Message, MessageTypes.Error);
            }
            return Json(new { isSuccess, message });
        }
    }
}