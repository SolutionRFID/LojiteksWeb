using LinqKit;
using LojiteksDataAccess.DBContext;
using LojiteksEntity.Entities;
using LojiteksWeb.Entities.Dtos;
using LojiteksWeb.Entities.Enums;
using LojiteksWeb.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace LojiteksWeb.Controllers
{
    [Authorize(Roles = "0")]
    public class KullaniciController : Controller
    {
        private readonly Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        public KullaniciController(Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = httpContextAccessor.HttpContext.User;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public Expression<Func<TBL_Kullanici, bool>> GetFilterExpression(long firmaID)
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
        public async Task<IActionResult> GetKullanici(int page = 1, int pageSize = 100, string sortColumn = "kullaniciID", string sortOrder = "asc", string filter = null)
        {
            var firmaID = _user.GetFirmaID();

            Expression<Func<TBL_Kullanici, bool>> filterExpression = GetFilterExpression(firmaID);

            if (!string.IsNullOrEmpty(filter))
            {
                Expression<Func<TBL_Kullanici, bool>> baseFilter = f => f.KullaniciAdi.Contains(filter) || f.AdSoyad.Contains(filter) || f.Firma.Unvan.Contains(filter);

                filterExpression = PredicateBuilder.And(filterExpression, baseFilter);
            }

            Func<IQueryable<TBL_Kullanici>, IOrderedQueryable<TBL_Kullanici>> orderBy = null;

            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "KullaniciAdi":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.KullaniciAdi) : q.OrderByDescending(f => f.KullaniciAdi);
                        break;
                    case "AdSoyad":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.AdSoyad) : q.OrderByDescending(f => f.AdSoyad);
                        break;
                    case "Firma.Unvan":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Firma.Unvan) : q.OrderByDescending(f => f.Firma.Unvan);
                        break;
                    case "Yetki":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Yetki) : q.OrderByDescending(f => f.Yetki);
                        break;
                    case "Email":
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.Email) : q.OrderByDescending(f => f.Email);
                        break;
                    default:
                        orderBy = q => sortOrder == "asc" ? q.OrderBy(f => f.KullaniciID) : q.OrderByDescending(f => f.KullaniciID);
                        break;
                }
            }

            var query = _context.TBL_Kullanicilar.AsNoTracking();

            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            int skipCount = (page - 1) * pageSize;

            var kullanicilar = await query.Skip(skipCount).Take(pageSize).Include(x => x.Firma).ToListAsync();
            var result = kullanicilar.Select(k => new KullaniciDto
            {
                KullaniciID = k.KullaniciID,
                KullaniciAdi = k.KullaniciAdi,
                AdSoyad = k.AdSoyad,
                Email = k.Email,
                Firma = k.Firma?.Unvan,
                Yetki = ((YetkiEnum)k.Yetki).ToString()
            }).ToList();

            return Ok(result);
        }

        public IActionResult Create()
        {
            ViewBag.YetkiList = EnumHelper.GetEnumAsSelectList<YetkiEnum>();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TBL_Kullanici createData)
        {
            createData.KullaniciAdi = string.IsNullOrEmpty(createData.KullaniciAdi) ? "" : createData.KullaniciAdi.Trim();
            createData.AdSoyad = string.IsNullOrEmpty(createData.AdSoyad) ? "" : createData.AdSoyad.Trim();
            createData.Sifre = string.IsNullOrEmpty(createData.Sifre) ? "" : createData.Sifre.Trim();
            createData.Email = string.IsNullOrEmpty(createData.Email) ? "" : createData.Email.Trim();

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
            ViewBag.YetkiList = EnumHelper.GetEnumAsSelectList<YetkiEnum>();

            return View(createData);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            var editData = await _context.TBL_Kullanicilar.Include(x => x.Firma).FirstOrDefaultAsync(x => x.KullaniciID == id);
            if (editData == null)
            {
                return RedirectToAction("Index", "Kullanici", new { error = Const.NotFound });
            }
            ViewBag.YetkiList = EnumHelper.GetEnumAsSelectList<YetkiEnum>();
            return View(editData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, TBL_Kullanici editData)
        {
            if (id != editData.KullaniciID)
            {
                return RedirectToAction("Index", "Kullanici", new { error = Const.NotFound });
            }

            editData.KullaniciAdi = string.IsNullOrEmpty(editData.KullaniciAdi) ? "" : editData.KullaniciAdi.Trim();
            editData.AdSoyad = string.IsNullOrEmpty(editData.AdSoyad) ? "" : editData.AdSoyad.Trim();
            editData.Sifre = string.IsNullOrEmpty(editData.Sifre) ? "" : editData.Sifre.Trim();
            editData.Email = string.IsNullOrEmpty(editData.Email) ? "" : editData.Email.Trim();

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
            ViewBag.YetkiList = EnumHelper.GetEnumAsSelectList<YetkiEnum>();
            return View(editData);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            try
            {
                var deleteData = await _context.TBL_Kullanicilar.FindAsync(id);
                if (deleteData != null)
                {
                    _context.TBL_Kullanicilar.Remove(deleteData);
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
