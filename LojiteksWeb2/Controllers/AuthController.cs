using Microsoft.AspNetCore.Mvc;
using LojiteksWeb.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

public class AuthController : Controller
{
    private readonly DataBaseContext _context;  // EF DbContext

    public AuthController(DataBaseContext context)
    {
        _context = context;
    }

    // 📌 **Login Sayfasını Aç**
    [HttpGet]
    public IActionResult Login2()
    {
        return View();
    }

    // 📌 **Lock Screen Sayfasını Aç**
    [HttpGet]
    public IActionResult LockScreen2()
    {
        return View(); // Views/Auth/LockScreen2.cshtml dosyasını açar
    }




    // 📌 **Kullanıcı Girişi (Login)**
    [HttpPost]
    public async Task<IActionResult> Login2(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMessage = "Kullanıcı adı ve şifre boş olamaz.";
            return View(model);
        }

        try
        {
            var dbUser = await _context.TblKullanici.FirstOrDefaultAsync(e => e.KullaniciAdi == model.KullaniciAdi && e.Sifre == model.Sifre);

            if (dbUser == null)
            {
                ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre!";
                return View(model);
            }

            // 📌 Kullanıcı bilgilerini Sessions nesnesine dönüştür
            var SessionUsers = new Sessions
            {
                KKno = dbUser.KKno,
                IsLocked = 0, // Kullanıcı ilk giriş yaptığında kilitli olmasın
                KullaniciAdi = dbUser.KullaniciAdi,
                Firma = dbUser.Firma,
                AdSoyad = dbUser.AdSoyad,
                Yetki = dbUser.Yetki,
                Email = dbUser.Email,
                EmailConfirmed = dbUser.EmailConfirmed,
                TwoFactorCode = dbUser.TwoFactorCode,
                TwoFactorCodeExpiration = dbUser.TwoFactorCodeExpiration,
                TwoFactorCounter = dbUser.TwoFactorCounter
            };

            // 📌 JSON olarak Session’a kaydet
            var userJson = JsonSerializer.Serialize(SessionUsers);
            HttpContext.Session.SetString("Sessions", userJson);

            return RedirectToAction("Index", "Dashboard");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Giriş başarısız! " + ex.Message;
            return View(model);
        }
    }


    // 📌 **Kullanıcı Girişi (Unlock)**
    [HttpPost]
    public async Task<IActionResult> LockScreen2(LoginViewModel model)
    {
        try
        {
            var userJson = HttpContext.Session.GetString("Sessions");
            var SessionUsers = JsonSerializer.Deserialize<Sessions>(userJson);
            model.KullaniciAdi = SessionUsers.KullaniciAdi;

            if (model.Sifre == null)
            {
                ViewBag.ErrorMessage = "şifre boş olamaz.";

                return View(model);
            }
            var dbUser = await _context.TblKullanici.FirstOrDefaultAsync(e => e.KullaniciAdi == model.KullaniciAdi && e.Sifre == model.Sifre);

            if (dbUser == null)
            {
                ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre!";
                return View(model);
            }

            // 📌 Kullanıcı bilgilerini Sessions nesnesine dönüştür
            SessionUsers.IsLocked = 0;

            // 📌 JSON olarak Session’a kaydet
            var UserSetJson = JsonSerializer.Serialize(SessionUsers);
            HttpContext.Session.SetString("Sessions", UserSetJson);

            return RedirectToAction("Index", "Dashboard");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Giriş başarısız! " + ex.Message;
            return View(model);
        }
    }




    // 📌 **Lock Screen (Ekranı Kilitle)**
    [HttpPost]
    public IActionResult LockScreen()
    {
        var userJson = HttpContext.Session.GetString("Sessions");

        if (!string.IsNullOrEmpty(userJson))
        {
            var SessionUsers = JsonSerializer.Deserialize<Sessions>(userJson);
            SessionUsers.IsLocked = 1; // Kullanıcıyı kilitle

            HttpContext.Session.SetString("Sessions", JsonSerializer.Serialize(SessionUsers));
        }

        return Json(new { success = true, redirectUrl = "/Auth/LockScreen2" });
    }


    // 📌 **Logout (Çıkış Yap)**
    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Tüm session verilerini temizle
        return Json(new { success = true, redirectUrl = "/login" });
    }
}
