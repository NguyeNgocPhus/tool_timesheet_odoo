using auto_checkin.Areas.Admin.Models;
using auto_checkin.Controllers;
using auto_checkin.Persistances;
using auto_checkin.Services.Odoo;
using auto_checkin.Services.Websocket;
using Microsoft.AspNetCore.Mvc;
using PNN.Identity.Securities.Authorization;

namespace auto_checkin.Areas.Admin.Controllers
{

    public class BlogController : AdminController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebsocketHandler _ws;
        private readonly ApplicationDbContext _db;
        private OdooClient _odooClient;

        public BlogController(ILogger<HomeController> logger, WebsocketHandler ws, ApplicationDbContext db)
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

        [HttpGet]
        [Route("blog/create")]
        public async Task<IActionResult> Create()
        {

            return View();
        }

		[HttpPost]
        [Permissions]
		[Route("blog/create")]
		public async Task<IActionResult> CreateBlog(BlogModel model)
		{

			return Redirect("List");
		}
	}

}
