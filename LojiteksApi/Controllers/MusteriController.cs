using LojiteksApi.Models;
using LojiteksDataAccess.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojiteksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusteriController : ControllerBase
    {
        private readonly Context _context;

        public MusteriController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new ResponseResultMessages<MusteriModel>();

            try
            {
                var result = await _context.TBL_Musteriler
                    .AsNoTracking()
                    .Select(x => new MusteriModel
                    {
                        MusteriID = x.MusteriID,
                        FirmaID = x.FirmaID,
                        MusteriAd = x.MusteriAd
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
            var response = new ResponseResultMessage<MusteriModel>();
            response.Data = new MusteriModel();

            try
            {
                var result = await _context.TBL_Musteriler
                    .AsNoTracking()
                    .Where(x => x.MusteriID == id)
                                        .Select(x => new MusteriModel
                                        {
                                            MusteriID = x.MusteriID,
                                            FirmaID = x.FirmaID,
                                            MusteriAd = x.MusteriAd ?? ""
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
                    response.Message = "Müşteri bulunamadı";
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

        [HttpGet("GetMusteriByFirmaID")]
        public async Task<IActionResult> GetMusteriByFirmaID(long id)
        {
            var response = new ResponseResultMessages<MusteriModel>();

            try
            {
                var result = await _context.TBL_Musteriler
                    .AsNoTracking()
                    .Where(x => x.FirmaID == id)
                    .Select(x => new MusteriModel
                    {
                        MusteriID = x.MusteriID,
                        FirmaID = x.FirmaID,
                        MusteriAd = x.MusteriAd
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
