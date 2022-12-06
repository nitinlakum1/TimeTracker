using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using TimeTracker.Helper;
using TimeTracker.Models;
using TimeTracker.Models.SystemLog;
using TimeTracker.Models.User;
using TimeTracker_Model;
using TimeTracker_Model.SystemLog;
using TimeTracker_Model.User;
using TimeTracker_Repository.SystemLogRepo;
using TimeTracker_Repository.UserRepo;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class SystemLogController : Controller
    {
        #region Declaration
        private readonly ISystemLogRepo _systemlogRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public SystemLogController(ISystemLogRepo systemlogRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserRepo userRepo)
        {
            _systemlogRepo = systemlogRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userRepo = userRepo;
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

                var users = await _userRepo.GetUserLookup();
                ViewBag.Users = new SelectList(users, "Id", "Username");

                ViewBag.RoleId = _httpContextAccessor?.HttpContext?.User.GetLoginRole() ?? 0;

                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int GetRoleId()
        {
            try
            {
                return _httpContextAccessor?.HttpContext?.User.GetLoginRole() ?? 0;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<IActionResult> GetSystemLog(DatatableParamViewModel param, string filter)
        {
            try
            {
                var dtParam = _mapper.Map<SystemLogFilterModel>(param);
                int userId = _httpContextAccessor?.HttpContext?.User.GetIdFromClaim() ?? 0;
                dtParam.UserId = userId;

                if (!string.IsNullOrWhiteSpace(filter) && filter != "{}")
                {
                    var filterData = JsonConvert.DeserializeObject<SystemLogFilterModel>(filter);
                    dtParam.UserId = filterData?.UserId == null || filterData?.UserId == 0 ? userId : filterData?.UserId;

                    dtParam.FromDate = filterData?.FromDate ?? DateTime.Now;
                    dtParam.ToDate = filterData?.ToDate ?? DateTime.Now;
                }

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
                var users = await _userRepo.GetUserLookup();
                ViewBag.Users = new SelectList(users, "Id", "Username");

                ViewBag.RoleId = _httpContextAccessor?.HttpContext?.User.GetLoginRole() ?? 0;


                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetMonthlyReport(MonthlyReportFilterViewModel param,
            string filter)
        {
            try
            {
                var dtParam = _mapper.Map<SystemLogFilterModel>(param);
                var userId = _httpContextAccessor?.HttpContext?.User
                    .GetIdFromClaim() ?? 0;
                dtParam.UserId = userId;

                if (!string.IsNullOrWhiteSpace(filter) && filter != "{}")
                {
                    var curentDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var filterData = JsonConvert.DeserializeObject<SystemLogFilterModel>(filter);
                    dtParam.UserId = filterData?.UserId == null || filterData?.UserId == 0 ? userId : filterData?.UserId;
                    dtParam.FromDate = filterData?.FromDate ?? curentDay;
                    dtParam.ToDate = (filterData?.ToDate ?? curentDay).AddMonths(1).AddDays(-1);
                }

                var systemLogs = await _systemlogRepo.GetMonthlyReport(dtParam);
                var monthlyReport = new List<MonthlyReportListViewModel>();
                if (systemLogs is { Count: > 0 })
                {
                    monthlyReport = systemLogs
                        .GroupBy(a => new { a.Username, a.LogTime.Date })
                        .Select(a => new MonthlyReportListViewModel()
                        {
                            Date = a.Key.Date,
                            Username = a.Key.Username,
                            StartingTime = a.FirstOrDefault()?.LogTime ?? DateTime.Now,
                            ClosingTime = a.OrderByDescending(x => x.LogTime)
                                           .FirstOrDefault()?.LogTime ?? DateTime.Now,
                            TotalTimeSpan = GetTotalHours(a.ToList()),
                        })
                        .ToList();
                }

                var MonthlyTime = monthlyReport.Select(a => a.TotalTimeSpan).ToList();
                TimeSpan totalMonthlyTime = new();
                for (int i = 0; i < MonthlyTime.Count; i++)
                {
                    totalMonthlyTime += MonthlyTime[i];
                }

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = monthlyReport,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                SystemLogViewModel model = new();

                var users = await _userRepo.GetUserLookup();
                ViewBag.Users = new SelectList(users, "Id", "Username");

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(SystemLogViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var addLog = _mapper.Map<SystemLogAdddModel>(model);
                    var result = await _systemlogRepo.AddLog(addLog);

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

        #region Private_Methods
        private TimeSpan GetTotalHours(List<SystemLogModel> todaysSystemLog)
        {
            try
            {
                if (todaysSystemLog is { Count: > 0 })
                {
                    var removeExtra = todaysSystemLog
                        .Take(2)
                        .Where(a => a.LogType == LogTypes.SystemLogOn
                               || a.LogType == LogTypes.ServiceStart)
                        .ToList();

                    if (removeExtra.Count > 1)
                    {
                        int id = removeExtra.FirstOrDefault()?.Id ?? 0;
                        todaysSystemLog = todaysSystemLog
                        .Where(a => a.Id != id)
                        .ToList();
                    }

                    var start = todaysSystemLog.Where(a => a.LogType == LogTypes.ServiceStart
                                       || a.LogType == LogTypes.SystemLogOn
                                       || a.LogType == LogTypes.SystemUnlock)
                                 .Select(a => a.LogTime)
                                 .ToList();

                    var end = todaysSystemLog.Where(a => a.LogType == LogTypes.SystemLock
                                     || a.LogType == LogTypes.SystemLogOff)
                               .Select(a => a.LogTime)
                               .ToList();

                    TimeSpan totalTime = new();

                    for (int i = 0; i < start.Count; i++)
                    {
                        totalTime += ((end.Count < i + 1 ? DateTime.Now : end[i]) - start[i]);
                    }
                    return totalTime;



                    //var firstLog = todaysSystemLog
                    //    .FirstOrDefault(a => a.LogType == LogTypes.SystemLogOn
                    //           || a.LogType == LogTypes.ServiceStart);

                    //var lastLog = todaysSystemLog
                    //    .OrderByDescending(a => a.LogTime)
                    //    .FirstOrDefault();

                    //if (firstLog == null)
                    //{
                    //    return new TimeSpan();
                    //}

                    //var firstTwoIds = todaysSystemLog
                    //    .Take(2)
                    //    .Where(a => a.LogType == LogTypes.ServiceStart
                    //           || a.LogType == LogTypes.SystemLogOn)
                    //    .Select(a => a.Id)
                    //    .ToList();


                    //todaysSystemLog = todaysSystemLog
                    //    .Where(a => !firstTwoIds.Contains(a.Id))
                    //    .ToList();

                    //var locked = todaysSystemLog
                    //    .Where(a => a.LogType == LogTypes.SystemLock
                    //            || a.LogType == LogTypes.SystemLogOff)
                    //    .Select(a => a.LogTime)
                    //    .ToList();

                    //var unlocked = todaysSystemLog
                    //    .Where(a => a.LogType == LogTypes.SystemUnlock
                    //           || a.LogType == LogTypes.SystemLogOn)
                    //    .Select(a => a.LogTime)
                    //    .ToList();

                    //TimeSpan deduction = new();
                    //for (int i = 0; i < locked.Count; i++)
                    //{
                    //    deduction += (unlocked[i] - locked[i]);
                    //}

                    //if (firstLog.LogTime.Date == DateTime.Now.Date)
                    //{
                    //    return DateTime.Now - firstLog.LogTime - deduction;
                    //}
                    //else
                    //{
                    //    return lastLog.LogTime - firstLog.LogTime - deduction;
                    //}
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
