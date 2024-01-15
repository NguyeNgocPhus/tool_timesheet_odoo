using auto_checkin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace auto_checkin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ZaloClient _zaloClient;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
          
            return View();
        }
        public async Task<IActionResult> User([FromQuery] string sessionId)
        {
            try
            {
                _zaloClient = new ZaloClient(session_id: sessionId);

                var rawUser = JsonConvert.DeserializeObject<User>(_zaloClient.getUserDetail().ToString());
                var jsonResponse = new List<object>(); 
                foreach(var user in rawUser.result)
                {
                    jsonResponse.Add(new
                    {
                        Id = user[0],
                        Name = user[1]
                    });
                }

                return Json(jsonResponse);
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        public async Task<IActionResult> Projects([FromQuery] string sessionId)
        {
            try
            {
                _zaloClient = new ZaloClient(session_id: sessionId);

                var rawProjects = JsonConvert.DeserializeObject<User>(_zaloClient.getProjects().ToString());
                var jsonResponse = new List<object>();
                foreach (var user in rawProjects.result)
                {
                    jsonResponse.Add(new
                    {
                        Id = user[0],
                        Name = user[1]
                    });
                }

                return Json(jsonResponse);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<IActionResult> Tasks([FromQuery] string sessionId, [FromQuery] int projectId)
        {
            try
            {
                _zaloClient = new ZaloClient(session_id: sessionId);

                var rawProjects = JsonConvert.DeserializeObject<User>(_zaloClient.getTasks(projectId).ToString());
                var jsonResponse = new List<object>();
                foreach (var user in rawProjects.result)
                {
                    jsonResponse.Add(new
                    {
                        Id = user[0],
                        Name = user[1]
                    });
                }

                return Json(jsonResponse);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<IActionResult> Create([FromQuery] string sessionId, [FromQuery] int projectId, [FromQuery] int employeeId, [FromQuery] int taskId, [FromQuery] string description, [FromQuery] string month)
        {
            try
            {
                string ngayCanKiemTra = "2/1/2024";
                // Chuyển đổi chuỗi ngày thành đối tượng DateTime
                DateTime ngay = DateTime.ParseExact(ngayCanKiemTra, "d/M/yyyy", null);
                // Lấy thông tin về thứ của ngày
                DayOfWeek thu = ngay.DayOfWeek;

                _zaloClient = new ZaloClient(session_id: sessionId);
                var data = _zaloClient.createTimeSheet(projectId, employeeId, taskId, description, "2024-02-11");
                var rawProjects = JsonConvert.DeserializeObject<CreateTimesheetResponse>(data.ToString());

                return Json(rawProjects);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
