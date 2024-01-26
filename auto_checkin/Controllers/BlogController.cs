using auto_checkin.Models;
using auto_checkin.Persistances;
using auto_checkin.Persistances.Entities;
using auto_checkin.Services.Odoo;
using auto_checkin.Services.Websocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace auto_checkin.Controllers
{
    public class BlogController : Controller
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

        public async Task<IActionResult> Index()
        {            
           
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string blog)
        {
            var a = await _db.Blogs.FirstOrDefaultAsync();
            var model = new BlogModel()
            {
                Full = a.Content,
                Comment = 1,
                Title = a.Title,
                Series = "Test series",
                BlogInSeries = new List<string>() { "Introduction", "Why react? Why redux", "Language and tooling" },
                CreateTime = $"{a.CreatedTime:dd/MM/yyy}"
            };
            return View(model);
        }
    }
}
