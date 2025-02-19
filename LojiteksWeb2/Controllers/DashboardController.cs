using Microsoft.AspNetCore.Mvc;

namespace Skote.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public IActionResult Index()
        {
            return View();
        }
    }
}