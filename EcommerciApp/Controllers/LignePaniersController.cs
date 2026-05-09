using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerciApp.Data;
using EcommerciApp.Models;

namespace EcommerciApp.Controllers
{
    // Changement de nom de classe pour éviter le conflit CS0101
    public class LignePaniersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LignePaniersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LignePaniers
        // Cette vue afficherait toutes les lignes de tous les paniers en BDD
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LignePaniers.Include(l => l.Panier).Include(l => l.Produit);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LignePaniers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var lignePanier = await _context.LignePaniers
                .Include(l => l.Panier)
                .Include(l => l.Produit)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lignePanier == null) return NotFound();

            return View(lignePanier);
        }

        // GET: LignePaniers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var lignePanier = await _context.LignePaniers
                .Include(l => l.Panier)
                .Include(l => l.Produit)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lignePanier == null) return NotFound();

            return View(lignePanier);
        }

        // POST: LignePaniers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lignePanier = await _context.LignePaniers.FindAsync(id);
            if (lignePanier != null)
            {
                _context.LignePaniers.Remove(lignePanier);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}