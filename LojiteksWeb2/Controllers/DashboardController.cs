using LojiteksWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LojiteksWeb.Controllers
{
    public class DashboardController : Controller
    {

        private readonly DataBaseContext _context;  // EF DbContext

        public DashboardController(DataBaseContext context)
        {

            _context = context;
        }
        [Route("Index")]
        // GET: Dashboard
        public IActionResult Index()
        {
            return View();
        }

        // Kullanıcı "kilidi aç" butonuna bastığında çalışacak action
        [HttpPost]
        public IActionResult Unlock()
        {
            // Session'daki kilit bilgisini temizle (0 veya kaldır)
            HttpContext.Session.SetInt32("IsLocked", 0);
            return RedirectToAction("Index", "Dashboard"); // veya uygun sayfaya yönlendirin
        }

        // Örnek statik veri
        [HttpPost]
        public async Task<IActionResult> GetLast6MonthShipments()
        {
            try
            {
                var SessionUsers = new Sessions();
                var userJson = HttpContext.Session.GetString("Sessions");
                SessionUsers = JsonSerializer.Deserialize<Sessions>(userJson);

                var sixMonthsAgo = DateTime.Now.AddMonths(-6);

                var Last6MonthShipmentsLists = from baslik in _context.TblBaslik
                                               where baslik.Firma == SessionUsers.Firma && baslik.KayitTarihi >= sixMonthsAgo
                                               join musteri in _context.TblMusteri on baslik.Musteri equals musteri.Mkno
                                               group baslik by musteri.Musteri into newtable
                                               select new
                                               {
                                                   Musteri = newtable.Key,
                                                   Adet = newtable.Count()
                                               };

                var data = new
                {
                    series = Last6MonthShipmentsLists.Select(x => x.Adet).ToList(),
                    labels = Last6MonthShipmentsLists.Select(x => x.Musteri).ToList()
                };

                return Ok(data);
            }
            catch(Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetLast6ClosedMonthShipments()
        {
            try
            {
                var SessionUsers = new Sessions();
                var userJson = HttpContext.Session.GetString("Sessions");
                SessionUsers = JsonSerializer.Deserialize<Sessions>(userJson);

                var sixMonthsAgo = DateTime.Now.AddMonths(-6);

                var Last6MonthShipmentsLists = from baslik in _context.TblBaslik
                                               where baslik.Firma == SessionUsers.Firma && baslik.KayitTarihi >= sixMonthsAgo
                                               group baslik by baslik.Silindi into newtable
                                               select new
                                               {
                                                   Kapatildi = newtable.Key,
                                                   Adet = newtable.Count()
                                               };

                var data = new
                {
                    series = Last6MonthShipmentsLists.Select(x => x.Adet).ToList(),
                    labels = Last6MonthShipmentsLists.Select(x => x.Kapatildi).ToList()
                };

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost]
        public IActionResult GetLast6MonthProduct()
        {
            try
            {
                var SessionUsers = new Sessions();
                var userJson = HttpContext.Session.GetString("Sessions");
                SessionUsers = JsonSerializer.Deserialize<Sessions>(userJson);

                var lastRecordedDate = _context.TblBaslik
                    .Where(baslik => baslik.Firma == SessionUsers.Firma)
                    .OrderByDescending(baslik => baslik.GonderimTarihi)
                    .Select(baslik => baslik.GonderimTarihi)
                    .FirstOrDefault(); // 📌 En son kayıtlı tarihi al

                if (lastRecordedDate == null)
                {
                    return Json(new { labels = new string[0], series = new int[0] }); // 📌 Veri yoksa boş döndür
                }

                var lastDate = lastRecordedDate.Value; // 📌 Nullable olduğundan dolayı Value alıyoruz
                var lastMonth = new DateOnly(lastDate.Year, lastDate.Month, 1); // 📌 Yıl ve Ayı al
                var sixMonths = Enumerable.Range(0, 6)
                    .Select(i => lastMonth.AddMonths(-i))
                    .OrderBy(date => date) // 📌 Son 6 ayı içeren tam listeyi oluştur
                    .ToList();

                // 📌 Veritabanından ilgili verileri çekiyoruz
                var shipmentData = _context.TblBaslik
                    .Where(baslik => baslik.Firma == SessionUsers.Firma && baslik.GonderimTarihi >= sixMonths.Min())
                    .Join(_context.TblEpc, baslik => baslik.Bkno, epc => epc.BaslikNo, (baslik, epc) => new { baslik, epc })
                    .GroupBy(x => new
                    {
                        Tarih = x.baslik.GonderimTarihi.HasValue
                            ? x.baslik.GonderimTarihi.Value
                            : DateOnly.FromDateTime(DateTime.MinValue)
                    })
                    .Select(g => new
                    {
                        Ay = g.Key.Tarih.ToDateTime(TimeOnly.MinValue).ToString("MMMM"), // 📌 Ay ismi
                        YilAy = g.Key.Tarih.ToString("yyyy-MM"), // 📌 Ayı karşılaştırma için (2024-02 gibi)
                        EPCAdet = g.Count() // 📌 EPC adeti
                    })
                    .ToList();

                // 📌 Son 6 Ayın Tam Listesi ile Verileri Eşleştir
                var finalData = sixMonths.Select(date => new
                {
                    Ay = date.ToDateTime(TimeOnly.MinValue).ToString("MMMM"),
                    EPCAdet = shipmentData.FirstOrDefault(x => x.YilAy == date.ToString("yyyy-MM"))?.EPCAdet ?? 0
                }).ToList();


                var data = new
                {
                    labels = finalData.Select(x => x.Ay),
                    series = new[] { finalData.Select(x => x.EPCAdet) }
                };

                return Json(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost]
        public IActionResult GetDashboardStats()
        {
            try
            {
                var SessionUsers = new Sessions();
                var userJson = HttpContext.Session.GetString("Sessions");
                SessionUsers = JsonSerializer.Deserialize<Sessions>(userJson);

                // 📌 Verileri çekiyoruz
                int toplamSevkiyat = _context.TblBaslik.Count(b => b.Firma == SessionUsers.Firma);
                int toplamKoli = _context.TblKoli.Count(k => k.Firma == SessionUsers.Firma);
                int toplamUrun = _context.TblEpc.Count(e => e.Firma == SessionUsers.Firma);

                // 📌 JSON olarak döndür
                return Json(new
                {
                    toplamSevkiyat,
                    toplamKoli,
                    toplamUrun
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = "Veri çekme hatası: " + ex.Message });
            }
        }

    }
}