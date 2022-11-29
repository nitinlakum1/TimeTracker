using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Helper;
using TimeTracker.Models;
using TimeTracker.Models.SystemLog;
using TimeTracker_Model;
using TimeTracker_Model.SystemLog;
using TimeTracker_Repository.SystemLogRepo;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class SystemLogController : Controller
    {
        #region Declaration
        private readonly ISystemLogRepo _systemlogRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public SystemLogController(ISystemLogRepo systemlogRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _systemlogRepo = systemlogRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Action
        public async Task<IActionResult> Index()
        {
            try
            {
                string lastTime = "00:00:00";
                int? userId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim();
                var todaysSystemLog = await _systemlogRepo.GetTodaysSystemLog(userId ?? 0);
                if (todaysSystemLog is { Count: > 0 })
                {
                    var logOn = todaysSystemLog
                        .First(a => a.LogType == LogTypes.SystemLogOn
                               || a.LogType == LogTypes.ServiceStart
                               || a.LogType == LogTypes.SystemLock);

                    var data = todaysSystemLog.Skip(1).ToList();

                    var locked = data
                        .Where(a => a.LogType == LogTypes.SystemLock)
                        .Select(a => a.LogTime)
                        .ToList();

                    var unlocked = data
                        .Where(a => a.LogType == LogTypes.SystemUnlock
                               || a.LogType == LogTypes.SystemLogOn)
                        .Select(a => a.LogTime)
                        .ToList();

                    TimeSpan deduction = new();
                    for (int i = 0; i < locked.Count; i++)
                    {
                        deduction += (unlocked[i] - locked[i]);
                    }
                    var todaysHour = DateTime.Now - logOn.LogTime - deduction;
                    lastTime = string.Format("{0:D2}:{1:D2}:{2:D2}", todaysHour.Hours, todaysHour.Minutes, todaysHour.Seconds);
                }
                ViewBag.LastTime = lastTime;
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetSystemLog(DatatableParamViewModel param)
        {
            try
            {
                var dtParam = _mapper.Map<SystemLogFilterModel>(param);
                int? userId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim();
                dtParam.UserId = userId ?? 0;

                var (systemLogs, totalRecord) = await _systemlogRepo.GetSystemLog(dtParam);

                var lst = _mapper.Map<List<SystemLogListModel>>(systemLogs);

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = totalRecord,
                    iTotalDisplayRecords = totalRecord,
                    aaData = lst,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> MonthlyReport()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetMonthlyReport(MonthlyReportFilterViewModel param)
        {
            try
            {
                var dtParam = _mapper.Map<SystemLogFilterModel>(param);

                var roleId = _httpContextAccessor?.HttpContext?.User.GetLoginRole();
                if (roleId != (int)Roles.Admin)
                {
                    dtParam.UserId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim() ?? 0;
                }

                var systemLogs = await _systemlogRepo.GetMonthlyReport(dtParam);
                var monthlyReport = new List<MonthlyReportListViewModel>();
                if (systemLogs is { Count: > 0 })
                {
                    monthlyReport = systemLogs
                        .GroupBy(a => a.LogTime.Date)
                        .Select(a => new MonthlyReportListViewModel()
                        {
                            Date = a.Key,
                            Username = a.FirstOrDefault()?.Username ?? "",
                            FullName = a.FirstOrDefault()?.FullName ?? "",
                            StartingTime = a.FirstOrDefault().LogTime,
                            ClosingTime = a.OrderByDescending(x => x.LogTime).FirstOrDefault().LogTime,
                            TotalTime = GetTotalHours(a.ToList()),
                        })
                        .ToList();
                }

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = monthlyReport
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetTotalHours(List<SystemLogModel> lst)
        {
            if (lst is { Count: > 0 })
            {
                var logOn = lst
                    .First(a => a.LogType == LogTypes.SystemLogOn
                           || a.LogType == LogTypes.ServiceStart
                           || a.LogType == LogTypes.SystemLock);

                var data = lst.Skip(1).ToList();

                var locked = data
                    .Where(a => a.LogType == LogTypes.SystemLock)
                    .Select(a => a.LogTime)
                    .ToList();

                var unlocked = data
                    .Where(a => a.LogType == LogTypes.SystemUnlock
                           || a.LogType == LogTypes.SystemLogOn)
                    .Select(a => a.LogTime)
                    .ToList();

                TimeSpan deduction = new();
                for (int i = 0; i < locked.Count; i++)
                {
                    deduction += (unlocked[i] - locked[i]);
                }
                var todaysHour = DateTime.Now - logOn.LogTime - deduction;
                return string.Format("{0:D2}:{1:D2} Hours", todaysHour.Hours, todaysHour.Minutes);
            }
            return "";
        }
        
        #endregion
    }
}
