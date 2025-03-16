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
                                     bid = baslik.Bkno,
                                     sil = baslik.Silindi,
                                     durum = _context.TblEpc
                                                .Where(epc => epc.Firma == SessionUsers.Firma && epc.BaslikNo == baslik.Bkno)
                                                .Count() // 📌 Doğru COUNT() kullanımı
                                 }).ToList();

            return Json(new { data = shipmentsData });
        }

        [HttpDelete("/api/DeleteShipment")]
        public IActionResult DeleteShipment(string po)
        {
            try
            {
                // Kullanıcı oturum bilgisini al
                var userJson = HttpContext.Session.GetString("Sessions");
                if (string.IsNullOrEmpty(userJson))
                {
                    return Unauthorized(new { message = "Oturum süresi doldu, lütfen tekrar giriş yapın." });
                }

                var SessionUsers = JsonSerializer.Deserialize<Sessions>(userJson);

                // PO numarasına göre sevkiyatı bul
                var shipment = _context.TblBaslik
                    .FirstOrDefault(b => b.Bkno == Convert.ToInt64(po));

                if (shipment == null)
                {
                    return NotFound(new { message = "Sevkiyat bulunamadı." });
                }

                // Sevkiyatı veritabanından kaldır
                _context.TblBaslik.Remove(shipment);
                _context.SaveChanges();

                return Ok(new { message = "Sevkiyat başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Silme işlemi sırasında hata oluştu.", error = ex.Message });
            }
        }
    }
}
