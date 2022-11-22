using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Models;
using TimeTracker.Models.SystemLog;
using TimeTracker.Models.User;
using TimeTracker_Model.SystemLog;
using TimeTracker_Model.User;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    public class SystemLogController : Controller
    {
        #region Declaration
        private readonly ISystemLogRepo _SystemLogRepo;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public SystemLogController(ISystemLogRepo SystemLogRepo, IMapper mapper)
        {
            _SystemLogRepo = SystemLogRepo;
            _mapper = mapper;
        }
        #endregion

        #region Action
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetSystemLog(DatatableParamViewModel param)
        {
            var dtParam = _mapper.Map<SystemLogFilterModel>(param);

            var (systemLogs, totalRecord) = await _SystemLogRepo.GetSystemLog(dtParam);

            var lst = _mapper.Map<List<SystemLogListModel>>(systemLogs);
            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecord,
                iTotalDisplayRecords = totalRecord,
                aaData = lst
            });
        }
        #endregion
    }
}
