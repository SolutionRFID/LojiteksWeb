using LinqKit;
using LojiteksDataAccess.DBContext;
using LojiteksEntity.Entities;
using LojiteksWeb.Entities.Dtos;
using LojiteksWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace LojiteksWeb.Controllers
{
    public class LicenceController : Controller
    {
        private readonly Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        public LicenceController(Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = httpContextAccessor.HttpContext.User;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public Expression<Func<TBL_Lisans, bool>> GetFilterExpression(long firmaID)
        {
            if (firmaID == 1)
            {
                return cihaz => true;
            }
            else
            {
                return cihaz => cihaz.FirmaID == firmaID;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLicence(int page = 1, int pageSize = 100, string sortColumn = "LisansID", string sortOrder = "asc", string filter = null)
        {
            var firmaID = _user.GetFirmaID();

            Expression<Func<TBL_Lisans, bool>> filterExpression = GetFilterExpression(firmaID);

            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<TBL_Lisans, bool>> baseFilter = f => f.Cihaz.Tanim.Contains(filter) || f.Cihaz.Kod.Contains(filter)
                || f.Firma.Unvan.Contains(filter);

                filterExpression = PredicateBuilder.And(filterExpression, baseFilter);
            }

            Func<IQueryable<TBL_Lisans>, IOrderedQueryable<TBL_Lisans>> orderBy = null;

            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "LisansTip":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.LisansTip) : q.OrderByDescending(f => f.LisansTip);
                        break;
                    case "Cihaz":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Cihaz.Tanim) : q.OrderByDescending(f => f.Cihaz.Tanim);
                        break;
                    case "KayitTarih":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.KayitTarih) : q.OrderByDescending(f => f.KayitTarih);
                        break;
                    case "Firma.Unvan":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Firma.Unvan) : q.OrderByDescending(f => f.Firma.Unvan);
                        break;
                    case "BaslangicTarih":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.BaslangicTarih) : q.OrderByDescending(f => f.BaslangicTarih);
                        break;
                    case "BitisTarih":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.BitisTarih) : q.OrderByDescending(f => f.BitisTarih);
                        break;
                    default:
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.LisansID) : q.OrderByDescending(f => f.LisansID);
                        break;
                }
            }

            var query = _context.TBL_Lisans.AsNoTracking();

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            int skipCount = (page - 1) * pageSize;

            var cihazlar = await query.Skip(skipCount).Take(pageSize).Include(x => x.Firma).Include(x => x.Cihaz).ToListAsync();
            var result = cihazlar.Select(k => new LisansDto
            {
                LisansID = k.LisansID,
                LisansTip = k.LisansTip,
                Cihaz = k.Cihaz?.SeriNo + " " + k.Cihaz?.Tanim,
                KayitTarih = k.KayitTarih,
                BaslangicTarih = k.BaslangicTarih,
                BitisTarih = k.BitisTarih,
                Firma = k.Firma?.Unvan,
            }).ToList();

            return Ok(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TBL_Lisans createData)
        {
            createData.LisansTip = 1;
            createData.KayitTarih = DateTime.Now;

            try
            {
                if (ModelState.IsValid)
                {
                    var cihaz = await _context.TBL_Cihazlar.FindAsync(createData.CihazID);
                    createData.FirmaID = cihaz.FirmaID;
                    _context.Add(createData);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = Const.CreateSuccess;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, Const.CreateFail);
            }

            return View(createData);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            var editData = await _context.TBL_Lisans.Include(x => x.Cihaz).FirstOrDefaultAsync(x => x.LisansID == id);
            if (editData == null)
            {
                return RedirectToAction("Index", "Licence", new { error = Const.NotFound });
            }
            return View(editData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, TBL_Lisans editData)
        {
            if (id != editData.LisansID)
            {
                return RedirectToAction("Index", "Licence", new { error = Const.NotFound });
            }

            editData.KayitTarih = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    var cihaz = await _context.TBL_Cihazlar.FindAsync(editData.CihazID);
                    editData.FirmaID = cihaz.FirmaID;
                    _context.Update(editData);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = Const.EditSuccess;
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError(string.Empty, Const.EditFail);
                }
            }
            return View(editData);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            try
            {
                var deleteData = await _context.TBL_Lisans.FindAsync(id);
                if (deleteData != null)
                {
                    _context.TBL_Lisans.Remove(deleteData);
                }

                await _context.SaveChangesAsync();
                TempData["Message"] = Const.DeleteSuccess;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, Const.DeleteFail);
            }
            return View();
        }
    }
}
