using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public HolidayController(IHolidayRepo holidayRepo, IMapper mapper)
        {
            _holidayRepo = holidayRepo;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadHoliday(DatatableParamViewModel param)
        {
            try
            {
                var dtParam = _mapper.Map<HolidayFilterModel>(param);

                var (userList, totalRecord) = await _holidayRepo.GetHolidayList(dtParam);

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = totalRecord,
                    iTotalDisplayRecords = totalRecord,
                    aaData = userList
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HolidayViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var addUser = _mapper.Map<HolidayModel>(model);
                    var result = await _holidayRepo.AddHoliday(addUser);

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
            var user = await _holidayRepo.GetHolidayById(id);
            var model = _mapper.Map<HolidayViewModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HolidayViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var editUser = _mapper.Map<HolidayModel>(model);
                    var result = await _holidayRepo.UpdateHoliday(editUser);

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