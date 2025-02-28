using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LojiteksWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LojiteksWeb.Controllers
{
    [Route("/[controller]")]
    public class ShipmentsReportsController : Controller
    {
        private readonly DataBaseContext _context;

        public ShipmentsReportsController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("/Views/Pages/ShipmentsReports.cshtml");
        }

        [HttpPost("/api/GetShipments")]
        public IActionResult GetShipments()
        {
            var userJson = HttpContext.Session.GetString("Sessions");
            var SessionUsers = JsonSerializer.Deserialize<Sessions>(userJson);

            var shipmentsData = (from baslik in _context.TblBaslik
                                 where baslik.Firma == SessionUsers.Firma 
                                 join musteri in _context.TblMusteri on baslik.Musteri equals musteri.Mkno
                                 select new
                                 {
                                     po = baslik.Po,                      // JSON'da küçük harfle "po"
                                     gonderimTarihi = baslik.GonderimTarihi, // küçük harfle "gonderimTarihi"
                                     aciklama = baslik.Aciklama,
                                     musteri = musteri.Musteri,
                                     kullanici = baslik.Kullanici,
                                     adet = baslik.GonderiAdedi,
                                     bid = baslik.Bkno
                                 }).ToList();

            return Json(new { data = shipmentsData });
        }
    }
}
