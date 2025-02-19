using Microsoft.AspNetCore.Mvc;

namespace Skote.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: Projects
        public IActionResult ProjectCreate()
        {
            return View();
        }

        public IActionResult ProjectGrid()
        {
            return View();
        }

        public IActionResult ProjectList()
        {
            return View();
        }

        public IActionResult ProjectOverview()
        {
            return View();
        }

    }
}