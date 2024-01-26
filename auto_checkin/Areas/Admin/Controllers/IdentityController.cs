using auto_checkin.Controllers;
using auto_checkin.Models;
using auto_checkin.Persistances;
using auto_checkin.Services.Odoo;
using auto_checkin.Services.Websocket;
using Microsoft.AspNetCore.Mvc;

namespace auto_checkin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class IdentityController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebsocketHandler _ws;
        private readonly ApplicationDbContext _db;
        private OdooClient _odooClient;

        public IdentityController(ILogger<HomeController> logger, WebsocketHandler ws, ApplicationDbContext db)
        {
            _logger = logger;
            _ws = ws;
            _db = db;
        }
        [HttpGet]
        [Route("/login")]

        public async Task<IActionResult> Login()
        {
           
            return View();
        }
    }
}

