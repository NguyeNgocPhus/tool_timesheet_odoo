using auto_checkin.Extensions;
using auto_checkin.Models;
using auto_checkin.Services.Odoo;
using auto_checkin.Services.Websocket;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace auto_checkin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebsocketHandler _ws;
        private OdooClient _odooClient;

        public HomeController(ILogger<HomeController> logger, WebsocketHandler ws)
        {
            _logger = logger;
            _ws = ws;
        }

        public async Task<IActionResult> Index()
        {

            return View();
        }
        public async Task<IActionResult> User([FromQuery] string sessionId)
        {
            try
            {
                _logger.LogInformation("Get user by session_id", sessionId);
                _odooClient = new OdooClient(session_id: sessionId);

                var rawUser = JsonConvert.DeserializeObject<User>(_odooClient.getUserDetail().ToString());
                var jsonResponse = new List<object>();
                foreach (var user in rawUser.result)
                {
                    jsonResponse.Add(new
                    {
                        Id = user[0],
                        Name = user[1]
                    });
                }

                return Json(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
        public async Task<IActionResult> Projects([FromQuery] string sessionId)
        {
            try
            {
                _odooClient = new OdooClient(session_id: sessionId);

                var rawProjects = JsonConvert.DeserializeObject<User>(_odooClient.getProjects().ToString());
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
        public async Task<IActionResult> Tasks([FromQuery] string sessionId, [FromQuery] int projectId)
        {
            try
            {
                _odooClient = new OdooClient(session_id: sessionId);

                var rawProjects = JsonConvert.DeserializeObject<User>(_odooClient.getTasks(projectId).ToString());
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTimesheetRequest request)
        {  
            var successRecord = 0;
            var errorRecord = 0;
            var days = new List<string>();
            if(request.type == 0)
            {
                var dayNumber = DateTime.DaysInMonth(DateTime.Now.Year, int.Parse(request.dateTimesheet));
                for (int i = 1; i <= dayNumber; i++)
                {
                    string day = string.Format("{0}/{1}/{2}", i, request.dateTimesheet, DateTime.Now.Year);
                    DateTime ngay = DateTime.ParseExact(day, "d/M/yyyy", null);

                    if (ngay.DayOfWeek == DayOfWeek.Sunday || ngay.DayOfWeek == DayOfWeek.Saturday)
                    {
                        continue;
                    }
                    var dayFormat = string.Format("{0}-{1}-{2}", DateTime.Now.Year, request.dateTimesheet, day.Split("/")[0]);
                    days.Add(dayFormat);
                }
            }
            else if(request.type == 1)
            {
                var date = request.dateTimesheet.Split("-");
                var from = date[0];
                var to = date[1];
                var fromDate = DateTime.Parse(from);
                var toDate = DateTime.Parse(to);
                foreach (DateTime dateTime in fromDate.ListAllDates(toDate))
                {
                    if (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
                    {
                        continue;
                    }
                    //var dayFormat = string.Format("{0}-{1}-{2}", DateTime.Now.Year, request.dateTimesheet, day.Split("/")[0]);
                    days.Add(dateTime.ToString("yyy-MM-dd"));
                }
            }
            
            var index = 1;
            foreach (var day in days)
            {

                try
                {
                    // core handle
                    _odooClient = new OdooClient(session_id: request.sessionId);
                    var data = _odooClient.createTimeSheet(request.projectId, request.employeeId, request.taskId, request.description, day);
                    var rawProjects = JsonConvert.DeserializeObject<CreateTimesheetResponse>(data.ToString());
                    if (rawProjects != null && rawProjects.result != null && rawProjects.error == null)
                    {
                        _logger.LogDebug($"👉 👉 👉 {request.employeeName} ADD TIMESHEET. DAY: {day}, PROJECT : {request.projectName}, DESCRIPTION: {request.description}");
                        successRecord++;
                        var loaded = (index * 100) / days.Count;
                        await _ws.SendMessageToClient(request.sessionId, new WsClientMessage
                        {
                            command = "loaded",
                            data = loaded
                        });
                        index += 1;
                    }
                    else
                    {
                        errorRecord++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    errorRecord++;
                }
            }


            _logger.LogInformation($"🙌 🙌 {successRecord} RECORD SUCCESS, {errorRecord} RECORD ERROR");
            return Json(new
            {
                successRecord,
                errorRecord
            });
        }
        [HttpPost]
        public async Task<IActionResult> TestWs([FromQuery] string session_id, [FromQuery] int loaded)
        {

            await _ws.SendMessageToClient(session_id, new WsClientMessage
            {
                command = "TestWs",
                data = loaded
            });
            return Ok("ok");
        }


    }
}
