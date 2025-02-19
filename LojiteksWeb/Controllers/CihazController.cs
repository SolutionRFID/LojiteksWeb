using LinqKit;
using LojiteksWeb.Entities.Dtos;
using LojiteksWeb.Entities.Enums;
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
    [Authorize(Roles = "0, 1")]
    public class CihazController : Controller
    {
        private readonly Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        public CihazController(Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = httpContextAccessor.HttpContext.User;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public Expression<Func<TBL_Cihaz, bool>> GetFilterExpression(long firmaID)
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
        public async Task<IActionResult> GetCihaz(int page = 1, int pageSize = 100, string sortColumn = "cihazID", string sortOrder = "asc", string filter = null)
        {
            var firmaID = _user.GetFirmaID();

            Expression<Func<TBL_Cihaz, bool>> filterExpression = GetFilterExpression(firmaID);

            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<TBL_Cihaz, bool>> baseFilter = f => f.Kod.Contains(filter) || f.Tanim.Contains(filter) || f.SeriNo.Contains(filter)
                || f.Firma.Unvan.Contains(filter);

                filterExpression = PredicateBuilder.And(filterExpression, baseFilter);
            }

            Func<IQueryable<TBL_Cihaz>, IOrderedQueryable<TBL_Cihaz>> orderBy = null;

            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "Kod":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Kod) : q.OrderByDescending(f => f.Kod);
                        break;
                    case "Tanim":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Tanim) : q.OrderByDescending(f => f.Tanim);
                        break;
                    case "SeriNo":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.SeriNo) : q.OrderByDescending(f => f.SeriNo);
                        break;
                    case "Firma.Unvan":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Firma.Unvan) : q.OrderByDescending(f => f.Firma.Unvan);
                        break;
                    case "CihazTip":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.CihazTip) : q.OrderByDescending(f => f.CihazTip);
                        break;
                    default:
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.CihazID) : q.OrderByDescending(f => f.CihazID);
                        break;
                }
            }

            var query = _context.TBL_Cihazlar.AsNoTracking();

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            int skipCount = (page - 1) * pageSize;

            var cihazlar = await query.Skip(skipCount).Take(pageSize).Include(x => x.Firma).ToListAsync();
            var result = cihazlar.Select(k => new CihazDto
            {
                CihazID = k.CihazID,
                Kod = k.Kod,
                Tanim = k.Tanim,
                SeriNo = k.SeriNo,
                CihazTip = ((CihazTipiEnum)k.CihazTip).ToString(),
                ApiUrl = k.ApiUrl,
                ApiKey = k.ApiKey,
                Firma = k.Firma?.Unvan,
            }).ToList();

            return Ok(result);
        }

        public IActionResult Create()
        {
            ViewBag.CihazTip = EnumHelper.GetEnumAsSelectList<CihazTipiEnum>();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CihazID,Kod,Tanim,SeriNo,CihazTip,UygulamaTipi,ApiUrl,ApiKey")] TBL_Cihaz createData)
        {
            var firmaID = _user.GetFirmaID();

            createData.Kod = string.IsNullOrEmpty(createData.Kod) ? "" : createData.Kod.Trim();
            createData.Tanim = string.IsNullOrEmpty(createData.Tanim) ? "" : createData.Tanim.Trim();
            createData.SeriNo = string.IsNullOrEmpty(createData.SeriNo) ? "" : createData.SeriNo.Trim();
            createData.ApiUrl = string.IsNullOrEmpty(createData.ApiUrl) ? "" : createData.ApiUrl.Trim();
            createData.ApiKey = string.IsNullOrEmpty(createData.ApiKey) ? "" : createData.ApiKey.Trim();

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
            ViewBag.CihazTip = EnumHelper.GetEnumAsSelectList<CihazTipiEnum>();

            return View(createData);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            var editData = await _context.TBL_Cihazlar.Include(x => x.Firma).FirstOrDefaultAsync(x => x.CihazID == id);
            if (editData == null)
            {
                return RedirectToAction("Index", "Cihaz", new { error = Const.NotFound });
            }
            ViewBag.CihazTip = EnumHelper.GetEnumAsSelectList<CihazTipiEnum>();
            return View(editData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, TBL_Cihaz editData)
        {
            if (id != editData.CihazID)
            {
                return RedirectToAction("Index", "Cihaz", new { error = Const.NotFound });
            }
            var firmaID = _user.GetFirmaID();

            editData.Kod = string.IsNullOrEmpty(editData.Kod) ? "" : editData.Kod.Trim();
            editData.Tanim = string.IsNullOrEmpty(editData.Tanim) ? "" : editData.Tanim.Trim();
            editData.SeriNo = string.IsNullOrEmpty(editData.SeriNo) ? "" : editData.SeriNo.Trim();
            editData.ApiUrl = string.IsNullOrEmpty(editData.ApiUrl) ? "" : editData.ApiUrl.Trim();
            editData.ApiKey = string.IsNullOrEmpty(editData.ApiKey) ? "" : editData.ApiKey.Trim();

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
            ViewBag.CihazTip = EnumHelper.GetEnumAsSelectList<CihazTipiEnum>();
            return View(editData);
        }

        // GET: TBL_Cihaz/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.TBL_Cihazlar == null)
            {
                return NotFound();
            }

            var tBL_Cihaz = await _context.TBL_Cihazlar
                .Include(t => t.Firma)
                .FirstOrDefaultAsync(m => m.CihazID == id);
            if (tBL_Cihaz == null)
            {
                return NotFound();
            }

            return View(tBL_Cihaz);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            try
            {
                var deleteData = await _context.TBL_Cihazlar.FindAsync(id);
                if (deleteData != null)
                {
                    _context.TBL_Cihazlar.Remove(deleteData);
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
