using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Helper;
using TimeTracker.Models;
using TimeTracker.Models.SystemLog;
using TimeTracker_Model;
using TimeTracker_Model.SystemLog;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class MonthlyReportController : Controller
    {
        #region Declaration
        private readonly IMonthlyReportRepo _monthlyReportRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public MonthlyReportController(IMonthlyReportRepo monthlyReportRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _monthlyReportRepo = monthlyReportRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Action
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //public async Task<IActionResult> GetSystemLog(DatatableParamViewModel param)
        //{
        //    try
        //    {
        //        var dtParam = _mapper.Map<SystemLogFilterModel>(param);
        //        int? userId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim();
        //        dtParam.UserId = userId ?? 0;

        //        var (systemLogs, totalRecord) = await _systemlogRepo.GetSystemLog(dtParam);

        //        var lst = _mapper.Map<List<SystemLogListModel>>(systemLogs);

        //        return Json(new
        //        {
        //            param.sEcho,
        //            iTotalRecords = totalRecord,
        //            iTotalDisplayRecords = totalRecord,
        //            aaData = lst,
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion
    }
}
