using EcommerciApp.Data;
using EcommerciApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcommerciApp.Controllers
{
    public class ProduitsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProduitsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Produits avec recherche, filtrage et tri
        public async Task<IActionResult> Index(int? categorieId, string searchString, string sortOrder, decimal? prixMin, decimal? prixMax)
        {
            // Requête de base
            var query = _context.Produits.Include(p => p.Categorie).AsQueryable();

            // 🔍 RECHERCHE par nom ou description
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p =>
                    p.Nom.Contains(searchString) ||
                    (p.Description != null && p.Description.Contains(searchString)));
            }

            // 🏷️ FILTRE par catégorie
            if (categorieId.HasValue && categorieId > 0)
            {
                query = query.Where(p => p.CategorieId == categorieId.Value);
                var cat = await _context.Categories.FindAsync(categorieId);
                ViewData["FiltrerNom"] = cat?.Nom;
            }

            // 💰 FILTRE par prix min
            if (prixMin.HasValue)
            {
                query = query.Where(p => p.Prix >= prixMin.Value);
            }

            // 💰 FILTRE par prix max
            if (prixMax.HasValue)
            {
                query = query.Where(p => p.Prix <= prixMax.Value);
            }

            // 📊 TRI
            query = sortOrder switch
            {
                "prix_asc" => query.OrderBy(p => p.Prix),
                "prix_desc" => query.OrderByDescending(p => p.Prix),
                "nom_asc" => query.OrderBy(p => p.Nom),
                "nom_desc" => query.OrderByDescending(p => p.Nom),
                "stock" => query.OrderByDescending(p => p.Stock),
                _ => query.OrderByDescending(p => p.Id) // Par défaut: plus récent
            };

            // Passer les données aux vues
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.CategorieId = categorieId;
            ViewBag.SearchString = searchString;
            ViewBag.SortOrder = sortOrder;
            ViewBag.PrixMin = prixMin;
            ViewBag.PrixMax = prixMax;

            return View(await query.ToListAsync());
        }

        // GET: Produits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var produit = await _context.Produits.Include(p => p.Categorie).FirstOrDefaultAsync(m => m.Id == id);
            return produit == null ? NotFound() : View(produit);
        }

        // GET: Produits/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom");
            return View();
        }

        // POST: Produits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Nom,Description,Prix,Stock,CategorieId")] Produit produit, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                    string path = Path.Combine(_hostEnvironment.WebRootPath, "images/produits", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create)) { await ImageFile.CopyToAsync(fileStream); }
                    produit.ImageUrl = "/images/produits/" + fileName;
                }
                else
                {
                    produit.ImageUrl = "/images/produits/default.jpg";
                }
                _context.Add(produit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom", produit.CategorieId);
            return View(produit);
        }

        // GET: Produits/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null) return NotFound();
            ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom", produit.CategorieId);
            return View(produit);
        }

        // POST: Produits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Description,Prix,Stock,CategorieId,ImageUrl")] Produit produit, IFormFile? ImageFile)
        {
            if (id != produit.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                        string path = Path.Combine(_hostEnvironment.WebRootPath, "images/produits", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create)) { await ImageFile.CopyToAsync(fileStream); }
                        produit.ImageUrl = "/images/produits/" + fileName;
                    }
                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Produits.Any(e => e.Id == produit.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom", produit.CategorieId);
            return View(produit);
        }

        // GET: Produits/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var produit = await _context.Produits.Include(p => p.Categorie).FirstOrDefaultAsync(m => m.Id == id);
            return produit == null ? NotFound() : View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit != null) { _context.Produits.Remove(produit); await _context.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}