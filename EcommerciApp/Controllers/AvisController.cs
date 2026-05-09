using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EcommerciApp.Data;
using EcommerciApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerciApp.Controllers
{
    [Authorize]
    public class AvisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public AvisController(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET : Afficher le formulaire d'avis
        public async Task<IActionResult> Donner(int id)
        {
            var userId = _userManager.GetUserId(User);

            var commande = await _context.Commandes
                .FirstOrDefaultAsync(c => c.Id == id && c.UtilisateurId == userId);

            if (commande == null) return NotFound();
            if (commande.Statut != "Livree") return Forbid();

            bool dejaAvis = await _context.Avis
                .AnyAsync(a => a.CommandeId == id && a.UtilisateurId == userId);

            if (dejaAvis) return RedirectToAction("MesCommandes", "Commandes");

            ViewBag.CommandeId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Donner(int id, int note, string? commentaire)
        {
            var userId = _userManager.GetUserId(User);

            var commande = await _context.Commandes
                .FirstOrDefaultAsync(c => c.Id == id && c.UtilisateurId == userId);

            if (commande == null) return NotFound();
            if (commande.Statut != "Livree") return Forbid();

            bool dejaAvis = await _context.Avis
                .AnyAsync(a => a.CommandeId == id && a.UtilisateurId == userId);

            if (dejaAvis) return RedirectToAction("MesCommandes", "Commandes");

            var avis = new Avis
            {
                CommandeId = id,
                UtilisateurId = userId!,
                Note = Math.Clamp(note, 1, 5),
                Commentaire = commentaire,
                DateAvis = DateTime.Now
            };

            _context.Avis.Add(avis);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Merci pour votre avis !";
            return RedirectToAction("MesCommandes", "Commandes");
        }
    }
}