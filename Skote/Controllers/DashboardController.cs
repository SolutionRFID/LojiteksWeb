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

        public IActionResult DashboardBlog()
        {
            return View();
        }

        public IActionResult DashboardCrypto()
        {
            return View();
        }
        public IActionResult Dashboardjob()
        {
            return View();
        }

        public IActionResult DashboardSaas()
        {
            return View();
        }


    }
}