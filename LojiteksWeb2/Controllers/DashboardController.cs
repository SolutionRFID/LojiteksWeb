using Microsoft.AspNetCore.Mvc;

namespace LojiteksWeb.Controllers
{
    public class DashboardController : Controller
    {
        [Route("Index")]
        // GET: Dashboard
        public IActionResult Index()
        {
            return View();
        }
    }
}