using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
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
                int? userId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim();
                var todaysSystemLog = await _systemlogRepo.GetTodaysSystemLog(userId ?? 0);

                var todaysHour = GetTotalHours(todaysSystemLog);

                string lastTime = string.Format("{0:D2}:{1:D2}:{2:D2}", todaysHour.Hours, todaysHour.Minutes, todaysHour.Seconds);
                
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
                    //var todaysHour = GetTotalHours(todaysSystemLog);

                    //string lastTime = string.Format("{0:D2}:{1:D2}:{2:D2}", todaysHour.Hours, todaysHour.Minutes, todaysHour.Seconds);

                    monthlyReport = systemLogs
                        .GroupBy(a => a.LogTime.Date)
                        .Select(a => new MonthlyReportListViewModel()
                        {
                            Date = a.Key,
                            Username = a.FirstOrDefault()?.Username ?? "",
                            FullName = a.FirstOrDefault()?.FullName ?? "",
                            StartingTime = a.FirstOrDefault().LogTime,
                            ClosingTime = a.OrderByDescending(x => x.LogTime).FirstOrDefault().LogTime,
                            TotalTimeSpan = GetTotalHours(a.ToList()),
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
        #endregion

        #region Private_Methods
        private TimeSpan GetTotalHours(List<SystemLogModel> todaysSystemLog)
        {
            try
            {
                if (todaysSystemLog is { Count: > 0 })
                {
                    var firstLog = todaysSystemLog
                        .FirstOrDefault(a => a.LogType == LogTypes.SystemLogOn
                               || a.LogType == LogTypes.ServiceStart);

                    if (firstLog == null)
                    {
                        return new TimeSpan();
                    }

                    var firstTwoIds = todaysSystemLog
                        .Take(2)
                        .Where(a => a.LogType == LogTypes.ServiceStart
                               || a.LogType == LogTypes.SystemLogOn)
                        .Select(a => a.Id)
                        .ToList();


                    todaysSystemLog = todaysSystemLog
                        .Where(a => !firstTwoIds.Contains(a.Id))
                        .ToList();

                    var locked = todaysSystemLog
                        .Where(a => a.LogType == LogTypes.SystemLock)
                        .Select(a => a.LogTime)
                        .ToList();

                    var unlocked = todaysSystemLog
                        .Where(a => a.LogType == LogTypes.SystemUnlock
                               || a.LogType == LogTypes.SystemLogOn)
                        .Select(a => a.LogTime)
                        .ToList();

                    TimeSpan deduction = new();
                    for (int i = 0; i < locked.Count; i++)
                    {
                        deduction += (unlocked[i] - locked[i]);
                    }

                    return DateTime.Now - firstLog.LogTime - deduction;
                }

                return new TimeSpan();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
