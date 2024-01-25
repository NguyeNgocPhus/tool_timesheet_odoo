using auto_checkin.Services.Odoo;
using auto_checkin.Services.Websocket;
using Microsoft.AspNetCore.Mvc;

namespace auto_checkin.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebsocketHandler _ws;
        private OdooClient _odooClient;

        public TimesheetController(ILogger<HomeController> logger, WebsocketHandler ws)
        {
            _logger = logger;
            _ws = ws;
        }
        public async Task<IActionResult> Index()
        {

            return View();
        }
    }
}
