using LojiteksDataAccess.DBContext;
using LojiteksEntity.Entities;
using LojiteksWeb.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LojiteksWeb.Controllers
{
    [Authorize]
    public class FindController : Controller
    {
        private readonly Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        public FindController(Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = httpContextAccessor.HttpContext.User;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("Find/FindFirma")]
        public async Task<IActionResult> FindFirma(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }

            IEnumerable<TBL_Firma> list = await _context.TBL_Firmalar.Where(x => x.Unvan.Contains(search)).ToListAsync();

            var values = list.Select(x => new
            {
                id = x.FirmaID,
                text = $"{x.Unvan}"
            });

            return Ok(values);
        }

        [Route("Find/FindMusteri")]
        public async Task<IActionResult> FindMusteri(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }
            var firmaID = _user.GetFirmaID();
            IEnumerable<TBL_Musteri> list;

            if (firmaID != 1)
            {
                list = await _context.TBL_Musteriler.Where(x => x.FirmaID == firmaID && x.MusteriAd.Contains(search)).ToListAsync();
            }
            else
            {
                list = await _context.TBL_Musteriler.Where(x => x.MusteriAd.Contains(search)).ToListAsync();
            }

            var values = list.Select(x => new
            {
                id = x.MusteriID,
                text = $"{x.MusteriAd}"
            });

            return Ok(values);
        }

        [Route("Find/FindCihaz")]
        public async Task<IActionResult> FindCihaz(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }
            var firmaID = _user.GetFirmaID();
            IEnumerable<TBL_Cihaz> list;

            if (firmaID != 1)
            {
                list = await _context.TBL_Cihazlar.Where(x => x.FirmaID == firmaID && x.Tanim.Contains(search) && x.SeriNo.Contains(search)).ToListAsync();
            }
            else
            {
                list = await _context.TBL_Cihazlar.Where(x => x.Tanim.Contains(search) && x.SeriNo.Contains(search)).ToListAsync();
            }

            var values = list.Select(x => new
            {
                id = x.CihazID,
                text = $"{x.SeriNo} {x.Tanim}"
            });

            return Ok(values);
        }
    }
}
