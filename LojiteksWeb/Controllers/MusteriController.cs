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
    [Authorize(Roles = "0, 1")]
    public class MusteriController : Controller
    {
        private readonly Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        public MusteriController(Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = httpContextAccessor.HttpContext.User;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public Expression<Func<TBL_Musteri, bool>> GetFilterExpression(long firmaID)
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
        public async Task<IActionResult> GetMusteri(int page = 1, int pageSize = 100, string sortColumn = "musteriID", string sortOrder = "asc", string filter = null)
        {
            var firmaID = _user.GetFirmaID();

            Expression<Func<TBL_Musteri, bool>> filterExpression = GetFilterExpression(firmaID);

            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<TBL_Musteri, bool>> baseFilter = f => f.MusteriAd.Contains(filter) || f.Firma.Unvan.Contains(filter);
                filterExpression = PredicateBuilder.And(filterExpression, baseFilter);
            }

            Func<IQueryable<TBL_Musteri>, IOrderedQueryable<TBL_Musteri>> orderBy = null;

            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "MusteriAd":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.MusteriAd) : q.OrderByDescending(f => f.MusteriAd);
                        break;
                    case "Firma.Unvan":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Firma.Unvan) : q.OrderByDescending(f => f.Firma.Unvan);
                        break;
                    default:
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.MusteriID) : q.OrderByDescending(f => f.MusteriID);
                        break;
                }
            }

            var query = _context.TBL_Musteriler.AsNoTracking();

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            int skipCount = (page - 1) * pageSize;

            var musteriler = await query.Skip(skipCount).Take(pageSize).Include(x => x.Firma).ToListAsync();
            var result = musteriler.Select(k => new MusteriDto
            {
                MusteriID = k.MusteriID,
                MusteriAd = k.MusteriAd,
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
        public async Task<IActionResult> Create(TBL_Musteri createData)
        {
            createData.MusteriAd = string.IsNullOrEmpty(createData.MusteriAd) ? "" : createData.MusteriAd.Trim();

            try
            {
                if (ModelState.IsValid)
                {
                    if (_user.GetFirmaID() != 1)
                    {
                        createData.FirmaID = _user.GetFirmaID();
                    }
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
            if (id == null || _context.TBL_Musteriler == null)
            {
                return NotFound();
            }

            var editData = await _context.TBL_Musteriler.Include(x => x.Firma).FirstOrDefaultAsync(x => x.MusteriID == id);
            if (editData == null)
            {
                return RedirectToAction("Index", "Musteri", new { error = Const.NotFound });
            }
            return View(editData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, TBL_Musteri editData)
        {
            if (id != editData.MusteriID)
            {
                return RedirectToAction("Index", "Musteri", new { error = Const.NotFound });
            }
            editData.MusteriAd = string.IsNullOrEmpty(editData.MusteriAd) ? "" : editData.MusteriAd.Trim();

            if (ModelState.IsValid)
            {
                try
                {
                    if (_user.GetFirmaID() != 1)
                    {
                        editData.FirmaID = _user.GetFirmaID();
                    }
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
                var deleteData = await _context.TBL_Musteriler.FindAsync(id);
                if (deleteData != null)
                {
                    _context.TBL_Musteriler.Remove(deleteData);
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
