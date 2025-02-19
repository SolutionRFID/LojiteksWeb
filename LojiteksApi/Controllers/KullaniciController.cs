using LojiteksApi.Models;
using LojiteksDataAccess.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojiteksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciController : ControllerBase
    {
        private readonly Context _context;

        public KullaniciController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new ResponseResultMessages<KullaniciModel>();

            try
            {
                var result = await _context.TBL_Kullanicilar
                    .AsNoTracking()
                    .Select(x => new KullaniciModel
                    {
                        KullaniciID = x.KullaniciID,
                        KullaniciAdi = x.KullaniciAdi ?? "",
                        Sifre = x.Sifre,
                        FirmaID = x.FirmaID,
                        AdSoyad = x.AdSoyad ?? "",
                        Yetki = x.Yetki
                    })
                    .ToListAsync();

                if (result != null && result.Any())
                {
                    response.Data = result;
                    response.isSuccess = true;
                    response.StatusCode = 200;
                    response.Message = "";
                }
                else
                {
                    response.isSuccess = false;
                    response.StatusCode = 204;
                    response.Message = "";
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.StatusCode = 400;
                response.Message = $"Bir hata gerçekleşti: {ex.Message}";
                return Ok(response);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var response = new ResponseResultMessage<KullaniciModel>();
            response.Data = new KullaniciModel();

            try
            {
                var result = await _context.TBL_Kullanicilar
                    .AsNoTracking()
                    .Where(x => x.KullaniciID == id)
                    .Select(x => new KullaniciModel
                    {
                        KullaniciID = x.KullaniciID,
                        KullaniciAdi = x.KullaniciAdi ?? "",
                        Sifre = x.Sifre,
                        FirmaID = x.FirmaID,
                        AdSoyad = x.AdSoyad ?? "",
                        Yetki = x.Yetki
                    })
                .FirstOrDefaultAsync();

                if (result != null)
                {
                    response.Data = result;
                    response.isSuccess = true;
                    response.StatusCode = 200;
                    response.Message = "";
                }
                else
                {
                    response.isSuccess = false;
                    response.StatusCode = 204;
                    response.Message = "";
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.StatusCode = 400;
                response.Message = $"Bir hata gerçekleşti: {ex.Message}";
                return Ok(response);
                throw;
            }
        }

        [HttpGet("GetKullaniciByFirmaID")]
        public async Task<IActionResult> GetKullaniciByFirmaID(long id)
        {
            var response = new ResponseResultMessages<KullaniciModel>();

            try
            {
                var result = await _context.TBL_Kullanicilar
                    .AsNoTracking()
                    .Where(x => x.FirmaID == id)
                    .Select(x => new KullaniciModel
                    {
                        KullaniciID = x.KullaniciID,
                        KullaniciAdi = x.KullaniciAdi ?? "",
                        Sifre = x.Sifre,
                        FirmaID = x.FirmaID,
                        AdSoyad = x.AdSoyad ?? "",
                        Yetki = x.Yetki
                    })
                    .ToListAsync();

                if (result != null && result.Any())
                {
                    response.Data = result;
                    response.isSuccess = true;
                    response.StatusCode = 200;
                    response.Message = "";
                }
                else
                {
                    response.isSuccess = false;
                    response.StatusCode = 404;
                    response.Message = "Kullanıcı bulunamadı";
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.StatusCode = 400;
                response.Message = $"Bir hata gerçekleşti: {ex.Message}";
                return Ok(response);
                throw;
            }
        }

    }
}
