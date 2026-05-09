using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerciApp.Data;
using EcommerciApp.Models;
using Microsoft.AspNetCore.Identity;

namespace EcommerciApp.Controllers
{
    public class PaniersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public PaniersController(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Paniers
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            // ✅ VIDER LE PANIER SI PLUS DE 7 JOURS
            var panier = await _context.Paniers
                .FirstOrDefaultAsync(p => p.UtilisateurId == userId);

            if (panier != null)
            {
                var joursEcoules = (DateTime.Now - panier.DateCreation).TotalDays;

                if (joursEcoules > 7)
                {
                    // Supprimer toutes les lignes du panier
                    var lignes = _context.LignePaniers.Where(l => l.PanierId == panier.Id);
                    _context.LignePaniers.RemoveRange(lignes);

                    // Réinitialiser la date du panier
                    panier.DateCreation = DateTime.Now;

                    await _context.SaveChangesAsync();

                    // Message pour l'utilisateur
                    TempData["Info"] = "Votre panier a été vidé car il était inactif depuis plus de 7 jours.";
                }
            }

            var lignesPanier = await _context.LignePaniers
                .Include(l => l.Produit)
                .Where(l => l.Panier.UtilisateurId == userId)
                .ToListAsync();

            return View(lignesPanier);
        }

        // POST: Paniers/Plus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Plus(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            var ligne = await _context.LignePaniers
                .Include(l => l.Produit)
                .FirstOrDefaultAsync(l => l.ProduitId == id && l.Panier.UtilisateurId == userId);

            if (ligne != null && ligne.Produit != null)
            {
                if (ligne.Quantite < ligne.Produit.Stock)
                {
                    ligne.Quantite++;
                    // ✅ METTRE À JOUR LA DATE DU PANIER (réinitialise le compteur de 7 jours)
                    ligne.Panier.DateCreation = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    TempData["Error"] = "Stock maximum atteint pour ce produit.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Paniers/Moins/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Moins(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            var ligne = await _context.LignePaniers
                .Include(l => l.Panier)
                .FirstOrDefaultAsync(l => l.ProduitId == id && l.Panier.UtilisateurId == userId);

            if (ligne != null)
            {
                if (ligne.Quantite > 1)
                {
                    ligne.Quantite--;
                }
                else
                {
                    _context.LignePaniers.Remove(ligne);
                }

                // ✅ METTRE À JOUR LA DATE DU PANIER
                ligne.Panier.DateCreation = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Paniers/Ajouter
        [HttpPost]
        public async Task<IActionResult> Ajouter(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            var produit = await _context.Produits.FindAsync(id);
            if (produit == null) return NotFound();

            var panier = await _context.Paniers.FirstOrDefaultAsync(p => p.UtilisateurId == userId);
            if (panier == null)
            {
                panier = new Panier { UtilisateurId = userId, DateCreation = DateTime.Now };
                _context.Paniers.Add(panier);
                await _context.SaveChangesAsync();
            }

            var ligne = await _context.LignePaniers
                .FirstOrDefaultAsync(l => l.PanierId == panier.Id && l.ProduitId == id);

            if (ligne == null)
            {
                _context.LignePaniers.Add(new LignePanier
                {
                    PanierId = panier.Id,
                    ProduitId = id,
                    Quantite = 1,
                    PrixUnitaire = produit.Prix
                });
            }
            else
            {
                if (ligne.Quantite < produit.Stock)
                    ligne.Quantite++;
                else
                    TempData["Error"] = "Stock épuisé.";
            }

            // ✅ METTRE À JOUR LA DATE DU PANIER (réinitialise le compteur)
            panier.DateCreation = DateTime.Now;
            await _context.SaveChangesAsync();

            var referer = Request.Headers["Referer"].ToString();
            return Redirect(string.IsNullOrEmpty(referer) ? "/" : referer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var userId = _userManager.GetUserId(User);
            var ligne = await _context.LignePaniers
                .Include(l => l.Panier)
                .FirstOrDefaultAsync(l => l.ProduitId == id && l.Panier.UtilisateurId == userId);

            if (ligne != null)
            {
                _context.LignePaniers.Remove(ligne);

                // ✅ METTRE À JOUR LA DATE DU PANIER
                ligne.Panier.DateCreation = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}