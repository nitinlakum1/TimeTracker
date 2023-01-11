using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Models;
using TimeTracker.Models.UpdateService;
using TimeTracker_Model;
using TimeTracker_Model.UpdateService;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UpdateServiceController : Controller
    {
        private readonly IUpdateServiceRepo _updateServiceRepo;
        private readonly IMapper _mapper;

        public UpdateServiceController(IUpdateServiceRepo updateServiceRepo, IMapper mapper)
        {
            _updateServiceRepo = updateServiceRepo;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadUpdateService(DatatableParamViewModel param)
        {
            try
            {
                var dtParam = _mapper.Map<UpdateServiceFilterModel>(param);

                var updateServiceList = await _updateServiceRepo.GetUpdateServiceList(dtParam);

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = updateServiceList
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
        public async Task<IActionResult> Create(AddUpdateServiceViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreatedOn = DateTime.Now;
                    var addUpdateService = _mapper.Map<UpdateServiceModel>(model);
                    var result = await _updateServiceRepo.AddUpdateService(addUpdateService);

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
        public async Task<IActionResult> DeleteUpdateService(int id)
        {
            bool isSuccess = false;
            string message = "";
            try
            {
                if (id > 0)
                {
                    isSuccess = await _updateServiceRepo.DeleteUpdateService(id);
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