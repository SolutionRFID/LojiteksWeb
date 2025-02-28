using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LojiteksWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LojiteksWeb.Controllers
{
    [Route("/[controller]")]
    public class NewShipmentController : Controller
    {
        private readonly DataBaseContext _context;

        public NewShipmentController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("/Views/Pages/NewShipment.cshtml");
        }

        [HttpGet("/api/GetCustomers")]
        public IActionResult GetCustomers()
        {
            try
            {
                var userJson = HttpContext.Session.GetString("Sessions");
                var SessionUsers = JsonSerializer.Deserialize<Sessions>(userJson);

                var customers = _context.TblMusteri.Where(m => m.Firma == SessionUsers.Firma).Select(m => new { id = m.Mkno, name = m.Musteri }).ToList();

                return Json(customers);
            }
            catch(Exception ex)
            {
                return Ok(new { message = "Hata!" + ex });
            }

        }

        [HttpPost("/api/AddShipment")]
        public IActionResult AddShipment([FromBody] TblBaslik model)
        {
            if (model == null)
            {
                return BadRequest("Boş veri alındı. Lütfen tüm alanları doldurun.");
            }

            try
            {
                var userJson = HttpContext.Session.GetString("Sessions");
                var SessionUsers = JsonSerializer.Deserialize<Sessions>(userJson);
                if (model == null)
                {
                    return BadRequest("Geçersiz veri");
                }

                var shipment = new TblBaslik
                {
                    Po = model.Po,
                    GonderimTarihi = model.GonderimTarihi,
                    Aciklama = model.Aciklama,
                    Musteri = model.Musteri,
                    GonderiAdedi = model.GonderiAdedi,
                    Kullanici = SessionUsers.KullaniciAdi,
                    Firma = SessionUsers.Firma,
                    Silindi = false,
                    KayitTarihi = DateTime.Now,
                    Tipi = 0

                };

                _context.TblBaslik.Add(shipment);
                _context.SaveChanges();

                return Ok(new { message = "Sevkiyat başarıyla eklendi." });
            }
            catch(Exception ex)
            {
                return Ok(new { message = "Hata!" + ex });
            }

        }
    }
}
