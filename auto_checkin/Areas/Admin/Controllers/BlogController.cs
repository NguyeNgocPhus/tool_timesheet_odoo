using auto_checkin.Areas.Admin.Models;
using auto_checkin.Controllers;
using auto_checkin.Persistances;
using Microsoft.AspNetCore.Mvc;

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
        [Route("blog/list")]
        public async Task<IActionResult> List()
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
		[Route("blog/create")]
		public async Task<IActionResult> CreateBlog(BlogModel model)
		{

			return Redirect("List");
		}
	}

}
