using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Models;
using TimeTracker_Model;
using TimeTracker_Model.User;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserController(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadUser(DatatableParamViewModel param)
        {
            var dtParam = _mapper.Map<UserFilterModel>(param);

            var (userList, totalRecord) = await _userRepo.GetUserList(dtParam);

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecord,
                iTotalDisplayRecords = totalRecord,
                aaData = userList
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool isSuccess = false;
            string message = "";
            try
            {
                if (id > 0)
                {
                    isSuccess = await _userRepo.DeleteUser(id);
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