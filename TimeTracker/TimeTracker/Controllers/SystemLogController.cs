using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Models;
using TimeTracker.Models.SystemLog;
using TimeTracker_Model;
using TimeTracker_Model.SystemLog;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    public class SystemLogController : Controller
    {
        #region Declaration
        private readonly ISystemLogRepo _systemlogRepo;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public SystemLogController(ISystemLogRepo systemlogRepo, IMapper mapper)
        {
            _systemlogRepo = systemlogRepo;
            _mapper = mapper;
        }
        #endregion

        #region Action
        public IActionResult Index()
        {
            try
            {
                var name = string.Format(@"{0:dd_MM_yy}.txt", DateTime.Now);
                string[] data = System.IO.File.ReadAllLines(@"C:\Program Files\WCT\" + name);

                if (data == null || !data.Any())
                {
                    //TODO: Set validation
                    throw new Exception("Record not found.");
                }

                var logs = data.Select(a => new SystemLogListModel()
                {
                    LogTime = Convert.ToDateTime(a.Split("|")[0].Trim()),
                    Description = a.Split("|")[1].Trim(),
                }).ToList();

                return View(logs);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetSystemLog(DatatableParamViewModel param)
        {
            var dtParam = _mapper.Map<SystemLogFilterModel>(param);

            var (systemLogs, totalRecord) = await _systemlogRepo.GetSystemLog(dtParam);

            var lst = _mapper.Map<List<SystemLogListModel>>(systemLogs);
            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecord,
                iTotalDisplayRecords = totalRecord,
                aaData = lst
            });
        }

        public async Task<IActionResult> DeleteSystemLog(int id)
        {
            bool isSuccess = false;
            string message = "";
            try
            {
                if (id > 0)
                {
                    isSuccess = await _systemlogRepo.DeleteSystemLog(id);
                    message = isSuccess ? AppMessages.DELETE_SUCCESS : AppMessages.SOMETHING_WRONG;
                }
            }
            catch (Exception ex)
            {
                //LogWriter.LogWrite(ex.Message, MessageTypes.Error);
            }
            return Json(new { isSuccess, message });
        }

        #endregion
    }
}
