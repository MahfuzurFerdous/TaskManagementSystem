using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Web.Controllers
{
    [Route("xyz")]
    public class TestController : Controller
    {
        [HttpGet("Dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
