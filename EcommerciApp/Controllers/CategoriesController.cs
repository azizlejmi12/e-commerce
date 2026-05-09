using EcommerciApp.Data;
using EcommerciApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerciApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var categorie = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            return categorie == null ? NotFound() : View(categorie);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Nom")] Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categorie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categorie);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var categorie = await _context.Categories.FindAsync(id);
            return categorie == null ? NotFound() : View(categorie);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] Categorie categorie)
        {
            if (id != categorie.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(categorie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categorie);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var categorie = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            return categorie == null ? NotFound() : View(categorie);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categorie = await _context.Categories.FindAsync(id);
            if (categorie != null) { _context.Categories.Remove(categorie); await _context.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}