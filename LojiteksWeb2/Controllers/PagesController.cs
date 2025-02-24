using Microsoft.AspNetCore.Mvc;

namespace LojiteksWeb.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages
        public IActionResult Page404()
        {
            return View();
        }

        public IActionResult Page500()
        {
            return View();
        }

        public IActionResult PageComingsoon()
        {
            return View();
        }

        public IActionResult PageFaqs()
        {
            return View();
        }

        public IActionResult PageMaintenance()
        {
            return View();
        }

        public IActionResult PagePricing()
        {
            return View();
        }

        public IActionResult PageTimeline()
        {
            return View();
        }

    }
}