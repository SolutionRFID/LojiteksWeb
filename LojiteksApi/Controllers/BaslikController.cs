using Azure;
using LojiteksApi.Models;
using LojiteksDataAccess.DBContext;
using LojiteksEntity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojiteksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaslikController : ControllerBase
    {
        private readonly Context _context;

        public BaslikController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new ResponseResultMessages<BaslikModel>();

            try
            {
                var result = await _context.TBL_Basliklar
                    .AsNoTracking()
                    .Where(x => x.SilindiMi == false)
                    .Select(x => new BaslikModel
                    {
                        BaslikID = x.BaslikID,
                        Tipi = x.Tipi ?? -1,
                        Aciklama = x.Aciklama ?? "",
                        MusteriID = x.MusteriID ?? -1,
                        GonderimTarihi = x.GonderimTarihi ?? DateTime.MinValue,
                        GonderiAdedi = x.GonderiAdedi ?? -1,
                        Kullanici = x.Kullanici ?? "",
                        SilindiMi = x.SilindiMi,
                        FirmaID = x.FirmaID ?? -1,
                        SevkiyatAd = x.SevkiyatAd ?? "",
                        PO_Number = x.PO_Number ?? "",
                        CihazID = x.CihazID ?? -1
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
            var response = new ResponseResultMessage<BaslikModel>();
            response.Data = new BaslikModel();
            try
            {
                var result = await _context.TBL_Basliklar
                    .AsNoTracking()
                    .Where(x => x.CihazID == id)
                   .Select(x => new BaslikModel
                   {
                       BaslikID = x.BaslikID,
                       Tipi = x.Tipi ?? -1,
                       Aciklama = x.Aciklama ?? "",
                       MusteriID = x.MusteriID ?? -1,
                       GonderimTarihi = x.GonderimTarihi ?? DateTime.MinValue,
                       GonderiAdedi = x.GonderiAdedi ?? -1,
                       Kullanici = x.Kullanici ?? "",
                       SilindiMi = x.SilindiMi,
                       FirmaID = x.FirmaID ?? -1,
                       SevkiyatAd = x.SevkiyatAd ?? "",
                       PO_Number = x.PO_Number ?? "",
                       CihazID = x.CihazID ?? -1
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
                    response.StatusCode = 404;
                    response.Message = "Başlık bulunamadı";
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

        [HttpGet("GetBaslikByFirmaID")]
        public async Task<IActionResult> GetBaslikByFirmaID(long id)
        {
            var response = new ResponseResultMessages<BaslikModel>();

            try
            {
                var result = await _context.TBL_Basliklar
                    .AsNoTracking()
                    .Where(x => x.FirmaID == id && x.SilindiMi == false)
                    .OrderByDescending(x => x.KayitTarihi)
                   .Select(x => new BaslikModel
                   {
                       BaslikID = x.BaslikID,
                       Tipi = x.Tipi ?? -1,
                       Aciklama = x.Aciklama ?? "",
                       MusteriID = x.MusteriID ?? -1,
                       GonderimTarihi = x.GonderimTarihi ?? DateTime.MinValue,
                       GonderiAdedi = x.GonderiAdedi ?? -1,
                       Kullanici = x.Kullanici ?? "",
                       SilindiMi = x.SilindiMi,
                       FirmaID = x.FirmaID ?? -1,
                       SevkiyatAd = x.SevkiyatAd ?? "",
                       PO_Number = x.PO_Number ?? "",
                       CihazID = x.CihazID ?? -1
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

        [HttpPost("CreateBaslik")]
        public async Task<IActionResult> CreateBaslik(BaslikDetailModel baslik)
        {
            var response = new ResponseMessage();
            try
            {
                // FirmaID değerini kontrol ediyoruz
                var firmaID = baslik.FirmaID == -1 ? (long?)null : baslik.FirmaID;

                // FirmaID ve GönderimTarihi kriterlerine göre mevcut kayıt var mı kontrol ediyoruz
                var existingHeader = await _context.Set<TBL_Baslik>()
                    .FirstOrDefaultAsync(b => b.FirmaID == firmaID && b.GonderimTarihi == baslik.GonderimTarihi);

                if (existingHeader != null)
                {
                    // İlişkili TBL_Epc kayıtlarını alıp siliyoruz
                    var existingEpcs = await _context.TBL_Epcler
                        .Where(e => e.BaslikID == existingHeader.BaslikID)
                        .ToListAsync();
                    _context.TBL_Epcler.RemoveRange(existingEpcs);

                    // İlişkili TBL_Koli kayıtlarını alıp siliyoruz
                    var existingKoliler = await _context.TBL_Koliler
                        .Where(k => k.BaslikID == existingHeader.BaslikID)
                        .ToListAsync();
                    _context.TBL_Koliler.RemoveRange(existingKoliler);

                    // Eski baslığı siliyoruz
                    _context.TBL_Basliklar.Remove(existingHeader);

                    await _context.SaveChangesAsync();
                }

                // Yeni baslık kaydı oluşturuluyor
                TBL_Baslik newHeader = new TBL_Baslik
                {
                    Tipi = baslik.Tipi,
                    Aciklama = baslik.Aciklama,
                    MusteriID = baslik.MusteriID == -1 ? (long?)null : baslik.MusteriID,
                    GonderimTarihi = baslik.GonderimTarihi,
                    GonderiAdedi = baslik.GonderiAdedi,
                    Kullanici = baslik.Kullanici,
                    FirmaID = firmaID,
                    SevkiyatAd = baslik.SevkiyatAd,
                    PO_Number = baslik.PO_Number,
                    CihazID = baslik.CihazID == -1 ? (long?)null : baslik.CihazID,
                    KayitTarihi = DateTime.Now,
                    SilindiMi = baslik.SilindiMi
                };

                await _context.AddAsync(newHeader);
                await _context.SaveChangesAsync();

                // Response'a insert edilen baslığın numarasını (BaslikID) ekliyoruz
                response.isSuccess = true;
                response.StatusCode = 200;
                response.Message = "Baslık başarıyla oluşturuldu.";
                response.Data = newHeader.BaslikID; // Oluşan baslığın numarası

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.StatusCode = 400;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        


        [HttpPost("UpdateBaslik")]
        public async Task<IActionResult> UpdateBaslik(BaslikModel baslikModel)
        {
            var responseMessage = new ResponseMessage();

            var baslik = await _context.TBL_Basliklar
                .AsNoTracking()
                .Where(x => x.BaslikID == baslikModel.BaslikID)
                .FirstOrDefaultAsync();

            if (baslik == null)
            {
                responseMessage.isSuccess = false;
                responseMessage.StatusCode = 404;
                responseMessage.Message = "Başlık bulunamadı";
                return Ok(responseMessage);
            }

            baslik.Tipi = baslikModel.Tipi;
            baslik.Aciklama = baslikModel.Aciklama;
            baslik.MusteriID = baslikModel.MusteriID == -1 ? (long?)null : baslikModel.MusteriID;
            baslik.GonderimTarihi = baslikModel.GonderimTarihi;
            baslik.GonderiAdedi = baslikModel.GonderiAdedi;
            baslik.Kullanici = baslikModel.Kullanici;
            baslik.FirmaID = baslikModel.FirmaID == -1 ? (long?)null : baslikModel.FirmaID;
            baslik.SevkiyatAd = baslikModel.SevkiyatAd;
            baslik.PO_Number = baslikModel.PO_Number;
            baslik.CihazID = baslikModel.CihazID == -1 ? (long?)null : baslikModel.CihazID;
            baslik.SilindiMi = baslikModel.SilindiMi;

            try
            {
                _context.Update(baslik);
                await _context.SaveChangesAsync();

                responseMessage.isSuccess = true;
                responseMessage.StatusCode = 200;
                responseMessage.Message = $"{baslik.BaslikID}";
                return Ok(responseMessage);
            }
            catch (Exception e)
            {
                responseMessage.isSuccess = false;
                responseMessage.StatusCode = 400;
                responseMessage.Message = e.ToString();
                return Ok(responseMessage);
            }
        }

        [HttpPost("DeleteBaslik")]
        public async Task<IActionResult> DeleteBaslik(long id)
        {
            var responseMessage = new ResponseMessage();
            var baslik = await _context.TBL_Basliklar
                .AsNoTracking()
                .Where(x => x.BaslikID == id)
                .FirstOrDefaultAsync();

            if (baslik == null)
            {
                responseMessage.isSuccess = false;
                responseMessage.StatusCode = 404;
                responseMessage.Message = "Başlık bulunamadı";
                return Ok(responseMessage);
            }

            var epcLine = await _context.TBL_Epcler
                .Where(x => x.BaslikID == id)
                .ToListAsync();
            try
            {
                _context.RemoveRange(epcLine); 
                _context.Remove(baslik);
                await _context.SaveChangesAsync();

                responseMessage.isSuccess = true;
                responseMessage.StatusCode = 200;
                responseMessage.Message = $"Başlık silindi başlık id:{baslik.BaslikID}";
                return Ok(responseMessage);
            }
            catch (Exception e)
            {
                responseMessage.isSuccess = false;
                responseMessage.StatusCode = 400;
                responseMessage.Message = e.ToString();
                return Ok(responseMessage);
            }
        }

        [HttpPost("SafeDelete")]
        public async Task<IActionResult> SafeDeleteBaslik(long id)
        {
            var responseMessage = new ResponseMessage();
            var baslik = await _context.TBL_Basliklar
                .AsNoTracking()
                .Where(x => x.BaslikID == id)
                .FirstOrDefaultAsync();

            if (baslik == null)
            {
                responseMessage.isSuccess = false;
                responseMessage.StatusCode = 404;
                responseMessage.Message = "Başlık bulunamadı";
                return Ok(responseMessage);
            }
            try
            {
                baslik.SilindiMi = true;
                _context.Update(baslik);
                await _context.SaveChangesAsync();

                responseMessage.isSuccess = true;
                responseMessage.StatusCode = 200;
                responseMessage.Message = $"Başlık silindi başlık id:{baslik.BaslikID}";
                return Ok(responseMessage);
            }
            catch (Exception e)
            {
                responseMessage.isSuccess = false;
                responseMessage.StatusCode = 400;
                responseMessage.Message = e.ToString();
                return Ok(responseMessage);
            }
        }
    }
}
