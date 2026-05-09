using System.Diagnostics;
using EcommerciApp.Data; // Assure-tu que ce namespace correspond à ton dossier Data
using EcommerciApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerciApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // 1. On ajoute la variable pour la base de données
        private readonly ApplicationDbContext _context;

        // 2. On injecte le DbContext dans le constructeur
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 3. Maintenant _context est reconnu et fonctionne
            var categories = await _context.Categories.Take(6).ToListAsync();
            return View(categories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}