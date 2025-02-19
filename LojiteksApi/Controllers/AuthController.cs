using LojiteksApi.Models;
using LojiteksDataAccess.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojiteksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Context _context;

        public AuthController(Context context)
        {
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string kullaniciAdi, string sifre)
        {
            var responseMessage = new ResponseResultMessage<KullaniciModel>();
            if (!string.IsNullOrEmpty(kullaniciAdi) && !string.IsNullOrEmpty(sifre))
            {
                var isUser = await _context.TBL_Kullanicilar
                    .Select(x => new KullaniciModel
                    {
                        KullaniciID = x.KullaniciID,
                        KullaniciAdi = x.KullaniciAdi ?? "",
                        Sifre = x.Sifre,
                        FirmaID = x.FirmaID,
                        AdSoyad = x.AdSoyad ?? "",
                        Yetki = x.Yetki
                    })
                    .Where(x => x.KullaniciAdi == kullaniciAdi && x.Sifre == sifre)
                    .FirstOrDefaultAsync();

                if (isUser == null)
                {
                    responseMessage.isSuccess = false;
                    responseMessage.StatusCode = 404;
                    responseMessage.Message = "Kullanıcı adı veya şifre hatalı.";
                    responseMessage.Data = new KullaniciModel();
                    return Ok(responseMessage);
                }

                responseMessage.isSuccess = true;
                responseMessage.StatusCode = 200;
                responseMessage.Message = "Giriş başarılı";
                responseMessage.Data = isUser;
                return Ok(responseMessage);
            }
            responseMessage.isSuccess = false;
            responseMessage.StatusCode = 404;
            responseMessage.Message = "Kullanıcı adı ve şifre giriniz.";
            responseMessage.Data = new KullaniciModel();
            return Ok(responseMessage);
        }
    }
}
