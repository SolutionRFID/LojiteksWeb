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
    }
}
