using Microsoft.AspNetCore.Mvc;
using LojiteksWeb.Models;
using LojiteksWeb.Services;
using System.Threading.Tasks;

public class AuthController : Controller
{
    private readonly AuthApiService _authService;

    public AuthController(AuthApiService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Login2(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMessage = "Kullanıcı adı ve şifre boş olamaz.";
            return View(model);
        }

        // API’ye bağlan ve sonucu al
        var result = await _authService.LoginAsync(model.KullaniciAdi, model.Sifre);

        if (result.isSuccess)
        {
            // Başarılı giriş → Kullanıcı bilgilerini Session’a kaydet
            HttpContext.Session.SetString("User", result.Data.KullaniciAdi);
            return RedirectToAction("Index", "Dashboard");
        }
        else
        {
            ViewBag.ErrorMessage = "Giriş başarısız! " + result.Message;
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Login2()
    {
        return View();
    }
}
