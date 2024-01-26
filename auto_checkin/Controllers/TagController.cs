using auto_checkin.Persistances;
using auto_checkin.Services.Odoo;
using auto_checkin.Services.Websocket;
using Microsoft.AspNetCore.Mvc;

namespace auto_checkin.Controllers
{
    public class TagsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebsocketHandler _ws;
        private readonly ApplicationDbContext _db;
        private OdooClient _odooClient;

        public TagsController(ILogger<HomeController> logger, WebsocketHandler ws, ApplicationDbContext db)
        {
            _logger = logger;
            _ws = ws;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View();
        }
    }
}
