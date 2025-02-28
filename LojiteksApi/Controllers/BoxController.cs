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
    public class BoxController : ControllerBase
    {
        private readonly Context _context;

        public BoxController(Context context)
        {
            _context = context;
        }

        [HttpPost("CreateBox")]
        public async Task<IActionResult> CreateBox(BoxDetailModel Box)
        {
            var responseMessage = new ResponseMessage();

            try
            {
                var TitleID = Box.TitleID == -1 ? (long?)null : Box.TitleID;

                // Mevcut kaydı kontrol ediyoruz.
                var existingBaslik = await _context.Set<TBL_Baslik>().FirstOrDefaultAsync(b => b.BaslikID == TitleID);

                if (existingBaslik != null)
                {
                    // İlişkili TBL_Epc kayıtlarını siliyoruz.
                    var existingEpcs = await _context.TBL_Epcler.Where(e => e.BaslikID == existingBaslik.BaslikID).ToListAsync();
                    _context.TBL_Epcler.RemoveRange(existingEpcs);

                    // İlişkili TBL_Koli kayıtlarını siliyoruz.
                    var existingKoliler = await _context.TBL_Koliler.Where(k => k.BaslikID == existingBaslik.BaslikID).ToListAsync();
                    _context.TBL_Koliler.RemoveRange(existingKoliler);

                    // Silme işlemlerini veritabanına yansıtıyoruz.
                    await _context.SaveChangesAsync();
                    // Yeni TBL_Epc ve TBL_Koli kayıtlarının oluşturulması
                    var epcs = new List<TBL_Epc>();
                    var koliler = new Dictionary<long?, TBL_Koli>();

                    foreach (var item in Box.EpcModels)
                    {
                        var epc = new TBL_Epc
                        {
                            BaslikID = existingBaslik.BaslikID,
                            KoliId = item.KoliId == -1 ? (long?)null : item.KoliId,
                            Epc = item.Epc,
                            Upc = item.Upc,
                            Beden = item.Beden,
                            KayitTarihi = DateTime.Now,
                            SilindiMi = item.SilindiMi,
                            FirmaID = existingBaslik.FirmaID
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
                                    BaslikID = existingBaslik.BaslikID,
                                    KoliBarkod = "",
                                    Adet = 1,
                                    SilindiMi = false,
                                    FirmaID = existingBaslik.FirmaID,
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
                    responseMessage.Message = "Koli oluşturma başarılı.";
                    return Ok(responseMessage);
                }
                else
                {
                    responseMessage.isSuccess = false;
                    responseMessage.StatusCode = 400;
                    responseMessage.Message = "BaslikID bulunamadı.";
                    return Ok(responseMessage);

                }

            }
            catch (Exception e)
            {
                responseMessage.isSuccess = false;
                responseMessage.StatusCode = 400;
                responseMessage.Message = e.ToString();
                return Ok(responseMessage);
            }
        }

        [HttpGet("GetBox")]
        public async Task<IActionResult> GetBox(long titleId)
        {
            var responseMessage = new ResponseMessage();
            try
            {
                // Verilen titleId'ye ait baslık kaydını getiriyoruz.
                var box = await _context.TBL_Basliklar
                    .FirstOrDefaultAsync(b => b.BaslikID == titleId);

                if (box == null)
                {
                    responseMessage.isSuccess = false;
                    responseMessage.StatusCode = 404;
                    responseMessage.Message = "Baslık bulunamadı.";
                    return NotFound(responseMessage);
                }

                // İlgili koli (box) kayıtlarını getiriyoruz.
                var boxs = await _context.TBL_Koliler
                    .Where(k => k.BaslikID == titleId)
                    .ToListAsync();

                // İlgili EPC kayıtlarını getiriyoruz.
                var epcs = await _context.TBL_Epcler
                    .Where(e => e.BaslikID == titleId)
                    .ToListAsync();

                // Gelen verileri BoxDetailModel DTO'suna eşliyoruz.
                var boxDetail = new BoxDetailModel
                {
                    TitleID = box.BaslikID,
                    EpcModels = epcs.Select(e => new EpcModel
                    {
                        // KoliId değeri varsa onu kullanıyoruz, yoksa -1 atıyoruz.
                        KoliId = e.KoliId.HasValue ? e.KoliId : -1,
                        Epc = e.Epc,
                        Upc = e.Upc,
                        Beden = e.Beden,
                        SilindiMi = e.SilindiMi,
                        KayitTarihi = e.KayitTarihi,
                        FirmaID = e.FirmaID,
                        BaslikNo = e.BaslikID
                    }).ToList(),

                    BoxModels = boxs.Select(e => new BoxModel
                    {
                        BoxID =(int)e.KoliId,
                        TotalEPCCount = (int)e.Adet,
                        CreatedDate = e.KayitTarihi,
                        Deleted = e.SilindiMi

                    }).ToList()
                };

                responseMessage.isSuccess = true;
                responseMessage.StatusCode = 200;
                responseMessage.Data = boxDetail;
                responseMessage.Message = "Box detayları başarıyla getirildi.";
                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                responseMessage.isSuccess = false;
                responseMessage.StatusCode = 500;
                responseMessage.Message = ex.Message;
                return StatusCode(500, responseMessage);
            }
        }

    }
}
