using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerciApp.Data;
using EcommerciApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace EcommerciApp.Controllers
{
    [Authorize]
    public class CommandesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public CommandesController(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ==========================================
        // PARTIE CLIENT : PROCESSUS D'ACHAT
        // ==========================================

        // 1. Afficher le formulaire de livraison (Checkout)
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = _userManager.GetUserId(User);
            var panier = await _context.Paniers
                .Include(p => p.LignesPanier)
                .ThenInclude(l => l.Produit)
                .FirstOrDefaultAsync(p => p.UtilisateurId == userId);

            if (panier == null || !panier.LignesPanier.Any())
                return RedirectToAction("Index", "Paniers");

            var total = panier.LignesPanier.Sum(l => (l.PrixUnitaire ?? 0) * l.Quantite);
            var commande = new Commande { Total = total };

            // ✅ Envoyer les points à la vue
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.PointsDisponibles = user.PointsFidelite;
            ViewBag.ReductionMax = (Math.Min(user.PointsFidelite, 500) / 100) * 5m;

            return View(commande);
        }

        // 2. Créer la commande
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmerCommande(
            string AdresseLivraison, string Telephone, string Ville,
            int PointsAUtiliser = 0)
        {
            var userId = _userManager.GetUserId(User);

            var panier = await _context.Paniers
                .Include(p => p.LignesPanier)
                .ThenInclude(l => l.Produit)
                .FirstOrDefaultAsync(p => p.UtilisateurId == userId);

            if (panier == null || !panier.LignesPanier.Any())
                return RedirectToAction("Index", "Paniers");

            // Vérification du stock
            foreach (var item in panier.LignesPanier)
            {
                if (item.Produit.Stock < item.Quantite)
                {
                    TempData["Error"] = $"Stock insuffisant pour {item.Produit.Nom} (Disponible: {item.Produit.Stock})";
                    return RedirectToAction("Index", "Paniers");
                }
            }

            var totalBrut = panier.LignesPanier.Sum(l => (l.PrixUnitaire ?? 0) * l.Quantite);

            // ✅ Valider les points choisis
            var user = await _userManager.FindByIdAsync(userId);
            int pointsUtilises = 0;
            decimal reduction = 0;

            if (PointsAUtiliser >= 100
                && PointsAUtiliser <= 500
                && user.PointsFidelite >= PointsAUtiliser)
            {
                // Arrondir au multiple de 100 (sécurité)
                pointsUtilises = (PointsAUtiliser / 100) * 100;
                reduction = (pointsUtilises / 100) * 5m;
                // La réduction ne dépasse pas le total
                reduction = Math.Min(reduction, totalBrut);
            }

            var nouvelleCommande = new Commande
            {
                UtilisateurId = userId,
                DateCommande = DateTime.Now,
                Statut = "En attente",
                AdresseLivraison = AdresseLivraison,
                Telephone = Telephone,
                Ville = Ville,
                Total = totalBrut - reduction,
                PointsUtilises = pointsUtilises,
                Reduction = reduction,
                IsRead = true,
                LignesCommande = new List<LigneCommande>()
            };

            foreach (var item in panier.LignesPanier)
            {
                nouvelleCommande.LignesCommande.Add(new LigneCommande
                {
                    ProduitId = item.ProduitId,
                    Quantite = item.Quantite,
                    PrixUnitaire = item.PrixUnitaire ?? 0,
                    SousTotal = (item.PrixUnitaire ?? 0) * item.Quantite
                });
                item.Produit.Stock -= item.Quantite;
                _context.Update(item.Produit);
            }

            _context.Commandes.Add(nouvelleCommande);
            _context.LignePaniers.RemoveRange(panier.LignesPanier);
            await _context.SaveChangesAsync();

            // ✅ Points fidélité : gagner + retirer
            if (user != null)
            {
                var pointsGagnes = (int)Math.Floor(nouvelleCommande.Total);
                user.PointsFidelite += pointsGagnes;
                user.TotalPointsGagnes += pointsGagnes;
                user.PointsFidelite -= pointsUtilises;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("Confirmation", new { id = nouvelleCommande.Id });
        }

        // 3. Page de confirmation
        public async Task<IActionResult> Confirmation(int id)
        {
            var commande = await _context.Commandes
                .Include(c => c.LignesCommande)
                .ThenInclude(lc => lc.Produit)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (commande == null) return NotFound();
            return View(commande);
        }

        // 4. Liste personnelle du client
        public async Task<IActionResult> MesCommandes()
        {
            var userId = _userManager.GetUserId(User);

            var mesCommandes = await _context.Commandes
                .Where(c => c.UtilisateurId == userId)
                .OrderByDescending(c => c.DateCommande)
                .ToListAsync();

            var nonLues = mesCommandes.Where(c => !c.IsRead).ToList();
            if (nonLues.Any())
            {
                foreach (var cmd in nonLues)
                    cmd.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return View(mesCommandes);
        }

        // 5. Annuler la commande
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnnulerCommande(int id)
        {
            var userId = _userManager.GetUserId(User);

            var commande = await _context.Commandes
                .Include(c => c.LignesCommande)
                .ThenInclude(lc => lc.Produit)
                .FirstOrDefaultAsync(c => c.Id == id && c.UtilisateurId == userId);

            if (commande == null) return NotFound();

            var tempsEcoule = DateTime.Now - commande.DateCommande;
            if (tempsEcoule.TotalHours > 24)
                return BadRequest("Le délai d'annulation de 24h est dépassé.");

            if (commande.Statut == "Livree")
                return BadRequest("La commande a déjà été livrée.");

            if (commande.Statut == "Annulée" || commande.Statut == "Annulee")
                return BadRequest("Cette commande est déjà annulée.");

            // Remettre en stock
            foreach (var ligne in commande.LignesCommande)
            {
                if (ligne.Produit != null)
                {
                    ligne.Produit.Stock += ligne.Quantite;
                    _context.Update(ligne.Produit);
                }
            }

            // ✅ Rembourser les points utilisés
            if (commande.PointsUtilises > 0)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.PointsFidelite += commande.PointsUtilises;
                    await _userManager.UpdateAsync(user);
                }
            }

            commande.Statut = "Annulée";
            _context.Update(commande);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MesCommandes));
        }

        // ==========================================
        // PARTIE COMMUNE : DÉTAILS
        // ==========================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);

            var commande = await _context.Commandes
                .Include(c => c.Utilisateur)
                .Include(c => c.LignesCommande)
                .ThenInclude(lc => lc.Produit)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (commande == null) return NotFound();

            if (commande.UtilisateurId != userId && !User.IsInRole("Admin"))
                return Forbid();

            return View(commande);
        }

        // ==========================================
        // PARTIE ADMIN : GESTION DES VENTES
        // ==========================================

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var allCommandes = await _context.Commandes
                .Include(c => c.Utilisateur)
                .OrderByDescending(c => c.DateCommande)
                .ToListAsync();

            return View(allCommandes);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var commande = await _context.Commandes
                .Include(c => c.Utilisateur)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (commande == null) return NotFound();
            return View(commande);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, string Statut)
        {
            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null) return NotFound();

            commande.Statut = Statut;
            commande.IsRead = false;

            try
            {
                _context.Update(commande);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandeExists(commande.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CommandeExists(int id)
        {
            return _context.Commandes.Any(e => e.Id == id);
        }
    }
}