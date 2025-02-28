using Azure.Core;
using LojiteksWeb.Models;
using Microsoft.AspNetCore.Mvc;

public class ShipmentDetailsController : Controller
{
    private readonly DataBaseContext _context;

    public ShipmentDetailsController(DataBaseContext context)
    {
        _context = context;
    }

    // 📌 Aynı endpoint içinde **hem JSON hem de View döndüren yöntem**
    [HttpGet("/api/GetShipmentDetails")]
    public IActionResult GetShipmentDetails(int ShipmentID)
    {
        var details = _context.TblBaslik
            .Where(baslik => baslik.Bkno == ShipmentID)
            .Join(_context.TblKoli, baslik => baslik.Bkno, koli => koli.BaslikNo, (baslik, koli) => new { baslik, koli })
            .Select(x => new ShipmentDetailsViewModel // 📌 Anonim tür yerine model kullanıyoruz!
            {
                BoxNo = x.koli.KoliId,
                BoxBarcode = x.koli.KoliBarkodu,
                BoxInCount = x.koli.Adet,
                ReadingDate = x.koli.Tarih,

                EpcList = _context.TblEpc
                    .Where(epc => epc.BaslikNo == x.baslik.Bkno && epc.KoliNo == x.koli.KoliId)
                    .Select(epc => new EpcDetailsViewModel
                    {
                        EPC = epc.Epc,
                        UPC = epc.Upc,
                        Size = epc.Beden
                    }).ToList()
            }).ToList();

        if (!details.Any())
        {
            return NotFound("Sevkiyat bulunamadı.");
        }

        // 📌 Eğer AJAX çağrısıysa JSON dön
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new { data = details });
        }

        // 📌 Eğer normal bir tarayıcı isteği ise View döndür
        return View("/Views/Pages/ShipmentDetails.cshtml", details);
    }
}
