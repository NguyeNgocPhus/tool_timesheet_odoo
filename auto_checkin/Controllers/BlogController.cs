using Microsoft.AspNetCore.Mvc;

namespace auto_checkin.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebsocketHandler _ws;
        private OdooClient _odooClient;

        public BlogController(ILogger<HomeController> logger, WebsocketHandler ws)
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
