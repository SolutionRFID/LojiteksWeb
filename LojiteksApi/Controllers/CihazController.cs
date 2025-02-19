using LojiteksApi.Models;
using LojiteksDataAccess.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojiteksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CihazController : ControllerBase
    {
        private readonly Context _context;

        public CihazController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new ResponseResultMessages<CihazModel>();

            try
            {
                var result = await _context.TBL_Cihazlar
                    .AsNoTracking()
                    .Select(x => new CihazModel
                    {
                        CihazID = x.CihazID,
                        Kod = x.Kod ?? "",
                        Tanim = x.Tanim ?? "",
                        SeriNo = x.SeriNo ?? "",
                        CihazTip = x.CihazTip ?? -1,
                        UygulamaTipi = x.UygulamaTipi ?? -1,
                        ApiUrl = x.ApiUrl ?? "",
                        ApiKey = x.ApiKey ?? "",
                        FirmaID = x.FirmaID ?? -1,
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
            var response = new ResponseResultMessage<CihazModel>();
            response.Data = new CihazModel();

            try
            {
                var result = await _context.TBL_Cihazlar
                    .AsNoTracking()
                    .Where(x => x.CihazID == id)
                    .Select(x => new CihazModel
                    {
                        CihazID = x.CihazID,
                        Kod = x.Kod ?? "",
                        Tanim = x.Tanim ?? "",
                        SeriNo = x.SeriNo ?? "",
                        CihazTip = x.CihazTip ?? -1,
                        UygulamaTipi = x.UygulamaTipi ?? -1,
                        ApiUrl = x.ApiUrl ?? "",
                        ApiKey = x.ApiKey ?? "",
                        FirmaID = x.FirmaID ?? -1,
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
                    response.Message = "Cihaz bulunamadı";
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

        [HttpGet("GetCihazByFirmaID")]
        public async Task<IActionResult> GetCihazByFirmaID(long id)
        {
            var response = new ResponseResultMessages<CihazModel>();

            try
            {
                var result = await _context.TBL_Cihazlar
                    .AsNoTracking()
                    .Where(x => x.FirmaID == id)
                    .Select(x => new CihazModel
                    {
                        CihazID = x.CihazID,
                        Kod = x.Kod ?? "",
                        Tanim = x.Tanim ?? "",
                        SeriNo = x.SeriNo ?? "",
                        CihazTip = x.CihazTip ?? -1,
                        UygulamaTipi = x.UygulamaTipi ?? -1,
                        ApiUrl = x.ApiUrl ?? "",
                        ApiKey = x.ApiKey ?? "",
                        FirmaID = x.FirmaID ?? -1,
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

    }
}
