using Microsoft.AspNetCore.Mvc;

namespace auto_checkin.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {

        public HomeController() { }

        [HttpGet]

        public async Task<IActionResult> Index()
        {

            return View();
        }
    }
}
