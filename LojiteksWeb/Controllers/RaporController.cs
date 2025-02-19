using LinqKit;
using LojiteksDataAccess.DBContext;
using LojiteksEntity.Entities;
using LojiteksWeb.Entities.Dtos;
using LojiteksWeb.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace LojiteksWeb.Controllers
{
    [Authorize]
    public class RaporController : Controller
    {
        private readonly Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public RaporController(Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = httpContextAccessor.HttpContext.User;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetRapor(int page = 1, int pageSize = 100, string sortColumn = "baslikID", string sortOrder = "asc", string filter = null, DateTime? startDate = null, DateTime? finishDate = null)
        {
            var firmaID = _user.GetFirmaID();
            Expression<Func<TBL_Baslik, bool>> baseFilter = x => x.SilindiMi == false;

            finishDate = (finishDate ?? DateTime.Today.AddDays(1)).Date;
            if (startDate.HasValue)
            {
                Expression<Func<TBL_Baslik, bool>> filterExpression = x => x.GonderimTarihi >= startDate ;
                baseFilter = PredicateBuilder.And(baseFilter, filterExpression);
            }

            if (finishDate.HasValue)
            {
                Expression<Func<TBL_Baslik, bool>> filterExpression = x => x.GonderimTarihi <= finishDate;
                baseFilter = PredicateBuilder.And(baseFilter, filterExpression);
            }

            if (firmaID != 1)
            {
                Expression<Func<TBL_Baslik, bool>> filterExpression = x => x.FirmaID == firmaID;
                baseFilter = PredicateBuilder.And(baseFilter, filterExpression);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<TBL_Baslik, bool>> filterExpression = f => f.Kullanici.Contains(filter) || f.SevkiyatAd.Contains(filter) || f.PO_Number.Contains(filter);
                baseFilter = PredicateBuilder.And(baseFilter, filterExpression);
            }

            Func<IQueryable<TBL_Baslik>, IOrderedQueryable<TBL_Baslik>> orderBy = null;

            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "GonderiAdedi":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.GonderiAdedi) : q.OrderByDescending(f => f.GonderiAdedi);
                        break;
                    case "Kullanici":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Kullanici) : q.OrderByDescending(f => f.Kullanici);
                        break;
                    case "SevkiyatAd":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.SevkiyatAd) : q.OrderByDescending(f => f.SevkiyatAd);
                        break;
                    case "Aciklama":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Aciklama) : q.OrderByDescending(f => f.Aciklama);
                        break;
                    case "PO_Number":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.PO_Number) : q.OrderByDescending(f => f.PO_Number);
                        break;
                    case "GonderimTarihi":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.GonderimTarihi) : q.OrderByDescending(f => f.GonderimTarihi);
                        break;
                    case "Firma.Unvan":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Firma.Unvan) : q.OrderByDescending(f => f.Firma.Unvan);
                        break;
                    default:
                        orderBy = q => sortOrder == "asc" ? q.OrderByDescending(f => f.BaslikID) : q.OrderBy(f => f.BaslikID);
                        break;
                }
            }

            var query = _context.TBL_Basliklar.AsNoTracking();

            query = query.Where(baseFilter);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            int skipCount = (page - 1) * pageSize;

            try
            {
                var cihazlar = await query.Skip(skipCount).Take(pageSize).Include(x => x.Firma).ToListAsync();
                var result = cihazlar.Select(k => new RaporDto
                {
                    BaslikID = k.BaslikID,
                    GonderimTarihi = k.GonderimTarihi,
                    GonderiAdedi = k.GonderiAdedi,
                    Kullanici = k.Kullanici,
                    SevkiyatAd = k.SevkiyatAd,
                    PO_Number = k.PO_Number,
                    Firma = k.Firma?.Unvan,
                    Aciklama = k.Aciklama
                }).ToList();

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Error");
                throw;
            }

        }

        public async Task<IActionResult> Detail(long id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Rapor");
            }

            var detail = await _context.TBL_Basliklar
                .Include(x => x.Firma)
                .Include(x => x.Musteri)
                .FirstOrDefaultAsync(x => x.BaslikID == id);

            ViewBag.koliCount = await  _context.TBL_Koliler
                .Where(x => x.BaslikID == id)
                .CountAsync();
            if (detail == null)
            {
                return RedirectToAction("Index", "Rapor");
            }
            return View(detail);
        }

        [HttpGet]
        public async Task<IActionResult> GetEpc(long id, int page = 1, int pageSize = 100, string sortColumn = "KoliID", string sortOrder = "asc", string filter = null)
        {
            var firmaID = _user.GetFirmaID();

            Expression<Func<TBL_Epc, bool>> filterExpression = x => x.BaslikID == id && x.SilindiMi == false;

            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<TBL_Epc, bool>> baseFilter = f => f.Epc.Contains(filter) || f.Upc.Contains(filter) || f.Beden.Contains(filter)
                || f.Firma.Unvan.Contains(filter) || f.KoliId.ToString().Contains(filter);

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
                    case "KayitTarihi":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.KayitTarihi) : q.OrderByDescending(f => f.KayitTarihi);
                        break;
                    default:
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.KoliId) : q.OrderByDescending(f => f.KoliId);
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
            var koli = await _context.TBL_Koliler
                .AsNoTracking()
                .Where(x => epcler.Select(x => x.KoliId).FirstOrDefault() == x.KoliId)
                .FirstOrDefaultAsync();

            var result = epcler
                .Select(k => new EpcDto
                {
                    KoliID = k.KoliId,
                    KoliBarkod = koli?.KoliBarkod ?? "",
                    Epc = k.Epc,
                    Upc = k.Upc,
                    Beden = k.Beden,
                    KayitTarihi = k.KayitTarihi,
                }).ToList();

            return Ok(result);
        }

    }
}
