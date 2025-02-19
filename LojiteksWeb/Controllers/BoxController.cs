using LinqKit;
using LojiteksWeb.Entities.Dtos;
using LojiteksWeb.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using LojiteksDataAccess.DBContext;
using LojiteksEntity.Entities;

namespace LojiteksWeb.Controllers
{
    [Authorize]
    public class BoxController : Controller
    {
        private readonly Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        public BoxController(Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = httpContextAccessor.HttpContext.User;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public Expression<Func<TBL_Koli, bool>> GetFilterExpression(long firmaID)
        {
            if (firmaID == 1)
            {
                return cihaz => true && cihaz.SilindiMi == false;
            }
            else
            {
                return cihaz => cihaz.FirmaID == firmaID && cihaz.SilindiMi == false;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBox(int page = 1, int pageSize = 100, string sortColumn = "musteriID", string sortOrder = "asc", string filter = null)
        {
            var firmaID = _user.GetFirmaID();

            Expression<Func<TBL_Koli, bool>> filterExpression = GetFilterExpression(firmaID);

            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<TBL_Koli, bool>> baseFilter = f => f.KoliBarkod.Contains(filter) || f.Firma.Unvan.Contains(filter);
                filterExpression = PredicateBuilder.And(filterExpression, baseFilter);
            }

            Func<IQueryable<TBL_Koli>, IOrderedQueryable<TBL_Koli>> orderBy = null;

            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "KoliBarkod":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.KoliBarkod) : q.OrderByDescending(f => f.KoliBarkod);
                        break;
                    case "Adet":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Adet) : q.OrderByDescending(f => f.Adet);
                        break;
                    case "KayitTarihi":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.KayitTarihi) : q.OrderByDescending(f => f.KayitTarihi);
                        break;
                    case "Firma":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Firma.Unvan) : q.OrderByDescending(f => f.Firma.Unvan);
                        break;
                    default:
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.KoliId) : q.OrderByDescending(f => f.KoliId);
                        break;
                }
            }

            var query = _context.TBL_Koliler.AsNoTracking();

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            int skipCount = (page - 1) * pageSize;

            var koliler = await query.Skip(skipCount).Take(pageSize).Include(x => x.Firma).ToListAsync();
            var result = koliler.Select(k => new KoliDto
            {
                KoliID = k.KoliID,
                KoliBarkod = k.KoliBarkod,
                KayitTarihi = k.KayitTarihi,
                Adet = k.Adet,
                Firma = k.Firma?.Unvan,
            }).ToList();

            return Ok(result);
        }

        public async Task<IActionResult> Details(long id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Box");
            }

            var detail = await _context.TBL_Koliler
                .FirstOrDefaultAsync(x => x.KoliID == id);

            if (detail == null)
            {
                return RedirectToAction("Index", "Box");
            }
            return View(detail);
        }

        [HttpGet]
        public async Task<IActionResult> GetBoxDetails(long id, int page = 1, int pageSize = 100, string sortColumn = "KayitTarihi", string sortOrder = "asc", string filter = null)
        {
            var firmaID = _user.GetFirmaID();
            var detail = await _context.TBL_Koliler
                .FirstOrDefaultAsync(x => x.KoliID == id);
            Expression<Func<TBL_Epc, bool>> filterExpression = x => x.KoliId == detail.KoliId && x.SilindiMi == false;

            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<TBL_Epc, bool>> baseFilter = f => f.Epc.Contains(filter) || f.Upc.Contains(filter) || f.Beden.Contains(filter)
                || f.Firma.Unvan.Contains(filter);

                filterExpression = PredicateBuilder.And(filterExpression, baseFilter);
            }

            Func<IQueryable<TBL_Epc>, IOrderedQueryable<TBL_Epc>> orderBy = null;

            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "Epc":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Epc) : q.OrderByDescending(f => f.Epc);
                        break;
                    case "Upc":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Upc) : q.OrderByDescending(f => f.Upc);
                        break;
                    case "Beden":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Beden) : q.OrderByDescending(f => f.Beden);
                        break;
                    default:
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.KayitTarihi) : q.OrderByDescending(f => f.KayitTarihi);
                        break;
                }
            }

            var query = _context.TBL_Epcler.AsNoTracking();

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            int skipCount = (page - 1) * pageSize;

            var epcler = await query.Skip(skipCount).Take(pageSize).ToListAsync();
            var result = epcler.Select(k => new BoxDetailsDto
            {
                EpcID = k.EpcID,
                Epc = k.Epc,
                Upc = k.Upc,
                Beden = k.Beden,
                KayitTarihi = k.KayitTarihi,
            }).ToList();

            return Ok(result);
        }
    }
}
