using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Models;
using TimeTracker.Models.Setting;
using TimeTracker_Model.Holiday;
using TimeTracker_Model.Setting;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    [Authorize(Roles = "Admin")]
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

            var (settingList, totalRecord) = await _settingRepo.SettingList(dtParam);

            var lst = _mapper.Map<List<SettingViewModel>>(settingList);
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
            var updateSetting = await _settingRepo.GetSettingById(id);
            var model = _mapper.Map<EditSettingViewModel>(updateSetting);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditSettingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var editSetting = _mapper.Map<SettingModel>(model);
                    var result = await _settingRepo.UpdateSetting(editSetting);

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
