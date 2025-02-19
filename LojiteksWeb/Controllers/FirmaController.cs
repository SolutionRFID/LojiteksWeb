using LojiteksDataAccess.DBContext;
using LojiteksEntity.Entities;
using LojiteksWeb.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LojiteksWeb.Controllers
{
    [Authorize(Roles = "0")]
    public class FirmaController : Controller
    {
        private readonly Context _context;

        public FirmaController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFirma(int page = 1, int pageSize = 100, string sortColumn = "firmaID", string sortOrder = "asc", string filter = null)
        {
            Expression<Func<TBL_Firma, bool>> filterExpression = null;

            if (!string.IsNullOrEmpty(filter))
            {
                filterExpression = f => f.Unvan.Contains(filter);
            }

            Func<IQueryable<TBL_Firma>, IOrderedQueryable<TBL_Firma>> orderBy = null;

            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "Unvan":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Unvan) : q.OrderByDescending(f => f.Unvan);
                        break;
                    default:
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.FirmaID) : q.OrderByDescending(f => f.FirmaID);
                        break;
                }
            }

            var query = _context.TBL_Firmalar.AsNoTracking();

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            int skipCount = (page - 1) * pageSize;

            var firmalar = await query.Skip(skipCount).Take(pageSize).ToListAsync();
            return Ok(firmalar);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirmaID,Unvan")] TBL_Firma createData)
        {
            createData.Unvan = string.IsNullOrEmpty(createData.Unvan) ? "" : createData.Unvan.Trim();

            try
            {
                if (ModelState.IsValid)
                {
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
            var editData = await _context.TBL_Firmalar.FindAsync(id);
            if (editData == null)
            {
                return RedirectToAction("Index", "Firma", new { error = Const.NotFound });
            }
            return View(editData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("FirmaID,Unvan")] TBL_Firma editData)
        {
            if (id != editData.FirmaID)
            {
                return RedirectToAction("Index", "Firma", new { error = Const.NotFound });
            }

            editData.Unvan = string.IsNullOrEmpty(editData.Unvan) ? "" : editData.Unvan.Trim();
            if (ModelState.IsValid)
            {
                try
                {
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
                var deleteData = await _context.TBL_Firmalar.FindAsync(id);
                if (deleteData != null)
                {
                    _context.TBL_Firmalar.Remove(deleteData);
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
