using LojiteksDataAccess.DBContext;
using LojiteksWeb.Dapper.DapperRepository;
using LojiteksWeb.Extensions;
using LojiteksWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace LojiteksWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public HomeController(ILogger<HomeController> logger, Context context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _user = httpContextAccessor.HttpContext.User;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var firmaID = _user.GetFirmaID();

            //adetler
            CountRepository countDb = new CountRepository("DbConnection");
            ViewBag.Counts = countDb.GetCount(firmaID);

            //lisans
            LicenceRepository licenceDb = new LicenceRepository("DbConnection");
            ViewBag.Licence = licenceDb.GetLicenceData(firmaID);

            //6 aylık bar
            MonthlyDataRepository monthlyDb = new MonthlyDataRepository("DbConnection");
            var monthlyData = monthlyDb.GetMonthlyData(firmaID);

            var labelsBar = new List<string>();
            var dataBar = new List<int>();

            for (int i = 0; i < 6; i++)
            {
                var date = DateTime.Now.AddMonths(-i);
                labelsBar.Add(date.ToString("MMMM", CultureInfo.CreateSpecificCulture("en")));
                var monthTotal = monthlyData.FirstOrDefault(x => x.Year == date.Year && x.Month == date.Month);
                dataBar.Add(monthTotal?.Total ?? 0);
            }
            labelsBar.Reverse();
            dataBar.Reverse();

            ViewBag.LabelsBar = labelsBar;
            ViewBag.DataBar = dataBar;

            //6 aylık müşteri
            var labelCustomerBar = new List<string>();
            var dataCustomerBar = new List<int>();
            var customerOrderCounts = monthlyDb.GetCustomerOrderCountModels(firmaID);


            foreach (var item in customerOrderCounts)
            {
                labelCustomerBar.Add(item.MusteriAd);
                dataCustomerBar.Add(item.TotalOrderCount);
            }
            ViewBag.LabelCustomerBar = labelCustomerBar;
            ViewBag.DataCustomerBar = dataCustomerBar;

            //6aylık müşteri pasta
            var labelCustomerPasta = new List<string>();
            var dataCustomerPasta = new List<int>();
            var customerOrderEpcCounts = monthlyDb.GetCustomerOrderEpcCountModels(firmaID);

            foreach (var item in customerOrderEpcCounts)
            {
                labelCustomerPasta.Add(item.MusteriAd);
                dataCustomerPasta.Add(item.TotalOrderCount);
            }

            ViewBag.LabelCustomerPasta = labelCustomerPasta;
            ViewBag.DataCustomerPasta = dataCustomerPasta;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {

            if (statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {
                ViewBag.errorCode = statuscode;
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
