using LojiteksApi.Models;
using LojiteksDataAccess.DBContext;
using LojiteksEntity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojiteksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirmaController : ControllerBase
    {
        private readonly Context _context;

        public FirmaController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new ResponseResultMessages<TBL_Firma>();

            try
            {
                var result = await _context.TBL_Firmalar
                    .AsNoTracking()
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
            var response = new ResponseResultMessage<TBL_Firma>();
            response.Data = new TBL_Firma();

            try
            {
                var result = await _context.TBL_Firmalar
                    .AsNoTracking()
                    .Where(x =>x.FirmaID == id)
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
                    response.Message = "Firma bulunamadı";
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
