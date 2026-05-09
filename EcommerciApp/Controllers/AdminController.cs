using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerciApp.Data;
using Microsoft.AspNetCore.Authorization;

namespace EcommerciApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            // 1. TOP 5 PRODUITS
            var topProduits = await _context.LigneCommandes
                .GroupBy(l => l.Produit.Nom)
                .Select(g => new { Nom = g.Key, Total = g.Sum(l => l.Quantite) })
                .OrderByDescending(g => g.Total)
                .Take(5)
                .ToListAsync();

            // 2. TOP 5 CLIENTS
            var topClients = await _context.Commandes
                .Include(c => c.Utilisateur)
                .GroupBy(c => c.Utilisateur.Nom + " " + c.Utilisateur.Prenom)
                .Select(g => new { NomComplet = g.Key, Somme = g.Sum(c => c.Total) })
                .OrderByDescending(g => g.Somme)
                .Take(5)
                .ToListAsync();

            // 3. TAUX DE FINALISATION = CA commandes / CA paniers
            double caCommandes = await _context.Commandes
                .SumAsync(c => (double?)c.Total) ?? 0;

            double caPaniers = await _context.LignePaniers
                .SumAsync(l => (double?)(l.Quantite * l.Produit.Prix)) ?? 0;

            double taux = caPaniers > 0
                ? Math.Min((caCommandes / caPaniers) * 100, 100)
                : 0;

            ViewBag.TauxFinalisation = Math.Round(taux, 1);
            ViewBag.ChiffreAffaires = caCommandes;

            // 4. NOMBRE TOTAL COMMANDES
            ViewBag.TotalCommandes = await _context.Commandes.CountAsync();

            // 5. NOMBRE TOTAL CLIENTS (sans Admin)
            var allUsers = await _context.Users.ToListAsync();
            int totalClients = 0;
            foreach (var user in allUsers)
            {
                var roles = await _context.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToListAsync();
                if (!roles.Contains("Admin"))
                    totalClients++;
            }
            ViewBag.TotalClients = totalClients;

            // 6. TAUX DE SATISFACTION = avis >= 3 / total avis
            double totalAvis = await _context.Avis.CountAsync();
            double avisSatisfaits = await _context.Avis
                .CountAsync(a => a.Note >= 3);

            ViewBag.TauxSatisfaction = totalAvis > 0
                ? Math.Round((avisSatisfaits / totalAvis) * 100, 1)
                : 0;
            ViewBag.TotalAvis = (int)totalAvis;

            // 7. MOYENNE DES NOTES
            double moyenneNotes = totalAvis > 0
                ? Math.Round(await _context.Avis.AverageAsync(a => (double)a.Note), 1)
                : 0;
            ViewBag.MoyenneNotes = moyenneNotes;

            ViewBag.TopProduits = topProduits;
            ViewBag.TopClients = topClients;

            return View();
        }
    }
}