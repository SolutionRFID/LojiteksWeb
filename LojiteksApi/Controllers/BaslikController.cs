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
                    .Where(x => x.FirmaID == id)
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
            var responseMessage = new ResponseMessage();
            TBL_Baslik tblBaslik = new TBL_Baslik
            {
                Tipi = baslik.Tipi,
                Aciklama = baslik.Aciklama,
                MusteriID = baslik.MusteriID == -1 ? (long?)null : baslik.MusteriID,
                GonderimTarihi = baslik.GonderimTarihi,
                GonderiAdedi = baslik.GonderiAdedi,
                Kullanici = baslik.Kullanici,
                FirmaID = baslik.FirmaID == -1 ? (long?)null : baslik.FirmaID,
                SevkiyatAd = baslik.SevkiyatAd,
                PO_Number = baslik.PO_Number,
                CihazID = baslik.CihazID == -1 ? (long?)null : baslik.CihazID,
                KayitTarihi = DateTime.Now,
                SilindiMi = baslik.SilindiMi
            };

            var epcs = new List<TBL_Epc>();
            var koliler = new Dictionary<long?, TBL_Koli>();

            try
            {
                await _context.AddAsync(tblBaslik);
                await _context.SaveChangesAsync();

                foreach (var item in baslik.EpcModels)
                {
                    var epc = new TBL_Epc
                    {
                        BaslikID = tblBaslik.BaslikID,
                        KoliId = item.KoliId == -1 ? (long?)null : item.KoliId,
                        Epc = item.Epc,
                        Upc = item.Upc,
                        Beden = item.Beden,
                        KayitTarihi = DateTime.Now,
                        SilindiMi = item.SilindiMi,
                        FirmaID = baslik.FirmaID == -1 ? (long?)null : baslik.FirmaID
                    };

                    if (epc.KoliId.HasValue)
                    {
                        if (koliler.ContainsKey(epc.KoliId))
                        {
                            koliler[epc.KoliId].Adet++;
                        }
                        else
                        {
                            var koli = new TBL_Koli
                            {
                                KoliId = epc.KoliId,
                                BaslikID = tblBaslik.BaslikID,  
                                KoliBarkod = "",
                                Adet = 1,
                                SilindiMi = false,
                                FirmaID = baslik.FirmaID == -1 ? (long?)null : baslik.FirmaID,
                                KayitTarihi = DateTime.Now
                            };
                            koliler.Add(epc.KoliId, koli);
                        }
                    }

                    epcs.Add(epc);
                }

                await _context.TBL_Epcler.AddRangeAsync(epcs);

                await _context.TBL_Koliler.AddRangeAsync(koliler.Values);
                await _context.SaveChangesAsync();

                responseMessage.isSuccess = true;
                responseMessage.StatusCode = 200;
                responseMessage.Message = $"{tblBaslik.BaslikID}";
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
