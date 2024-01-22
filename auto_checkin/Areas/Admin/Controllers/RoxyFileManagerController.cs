using auto_checkin.Controllers;
using auto_checkin.Persistances;
using Microsoft.AspNetCore.Mvc;
using PNN.Identity.Securities.Authorization;

namespace auto_checkin.Areas.Admin.Controllers
{
    public class RoxyFileManagerController : AdminController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebsocketHandler _ws;
        private readonly ApplicationDbContext _db;
        private OdooClient _odooClient;

        public RoxyFileManagerController(ILogger<HomeController> logger, WebsocketHandler ws, ApplicationDbContext db)
        {
            _logger = logger;
            _ws = ws;
            _db = db;
        }

        [HttpPost]
        [Route("roxyfilemanager/processrequest")]
        public async Task<IActionResult> roxyfilemanager([FromQuery] string a, [FromQuery] string type)
        {

            return Ok("[\r\n    {\r\n        \"p\": \"file\",\r\n        \"n\": \"Các tập tin\\n\",\r\n        \"f\": 11488,\r\n        \"d\": 10\r\n    },\r\n    {\r\n        \"p\": \"file/ảnh bảo hiểm\",\r\n        \"f\": 42,\r\n        \"d\": 0\r\n    },\r\n    {\r\n        \"p\": \"file/ảnh thẻ bệnh án\",\r\n        \"f\": 2,\r\n        \"d\": 0\r\n    },\r\n    {\r\n        \"p\": \"file/bài seo\",\r\n        \"f\": 26,\r\n        \"d\": 0\r\n    },\r\n    {\r\n        \"p\": \"file/bv pk\",\r\n        \"f\": 13,\r\n        \"d\": 0\r\n    },\r\n    {\r\n        \"p\": \"file/cddha truyen thong\",\r\n        \"f\": 12,\r\n        \"d\": 1\r\n    },\r\n    {\r\n        \"p\": \"file/cddha truyen thong/trang thiết bị\",\r\n        \"f\": 8,\r\n        \"d\": 0\r\n    },\r\n    {\r\n        \"p\": \"file/cong nghe cao\",\r\n        \"f\": 6,\r\n        \"d\": 1\r\n    },\r\n    {\r\n        \"p\": \"file/cong nghe cao/ton chi hoat dong\",\r\n        \"f\": 2,\r\n        \"d\": 0\r\n    },\r\n    {\r\n        \"p\": \"file/hội nghị\",\r\n        \"f\": 1,\r\n        \"d\": 0\r\n    },\r\n    {\r\n        \"p\": \"file/vp med\",\r\n        \"f\": 8,\r\n        \"d\": 0\r\n    }\r\n]");
        }

       
    }
}
