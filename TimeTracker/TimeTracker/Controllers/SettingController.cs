using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Models;
using TimeTracker.Models.Setting;
using TimeTracker.Models.User;
using TimeTracker_Model;
using TimeTracker_Model.Setting;
using TimeTracker_Model.User;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    public class SettingController : Controller
    {
        #region Declaration
        private readonly ISettingRepo _settingRepo;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public SettingController(ISettingRepo SettingRepo, IMapper mapper)
        {
            _settingRepo = SettingRepo;
            _mapper = mapper;
        }
        #endregion

        #region Action
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SettingList(DatatableParamViewModel param)
        {
            var dtParam = _mapper.Map<SettingFilterModel>(param);

            var (systemLogs, totalRecord) = await _settingRepo.SettingList(dtParam);

            var lst = _mapper.Map<List<SettingViewModel>>(systemLogs);
            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecord,
                iTotalDisplayRecords = totalRecord,
                aaData = lst
            });
        }

        public async Task<IActionResult> Update(int id)
        {
            var user = await _settingRepo.GetSettingById(id);
            var model = _mapper.Map<EditSettingViewModel>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditSettingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var editUser = _mapper.Map<SettingModel>(model);
                    var result = await _settingRepo.UpdateSetting(editUser);

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

        //[HttpPost]
        //public async Task<IActionResult> StatusSettings(int id)
        //{
        //    bool isSuccess = false;
        //    string message = "";
        //    try
        //    {
        //        if (id > 0)
        //        {
        //            isSuccess = await _settingRepo.StatusSettings(id);
        //            message = isSuccess ? AppMessages.DELETE_SUCCESS : AppMessages.SOMETHING_WRONG;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //LogWriter.LogWrite(ex.Message, MessageTypes.Error);
        //    }
        //    return Json(new { isSuccess, message });
        //}
        #endregion
    }
}
