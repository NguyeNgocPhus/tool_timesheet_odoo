using auto_checkin.Models;
using auto_checkin.Persistances;
using auto_checkin.Persistances.Entities;
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
            //var a = new Blog()
            //{
            //    Id = Guid.NewGuid(),
            //    Title = "Test blog ",
            //    Order = 0,
            //    SerieId = Guid.Parse("dbdba84c-3b2b-48ff-9818-03b0e6f3c4ed"),
            //    Content = "",
            //    Slug = "test-blog",
            //    ShortTitle = "test"
            //};
            // _db.Blogs.Add(a);
            //await _db.SaveChangesAsync();
            var a = await _db.Blogs.FirstOrDefaultAsync();
            var model = new BlogModel() { Full = a.Content };
            return View(model);
        }
    }
}
