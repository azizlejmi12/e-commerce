using EcommerciApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerciApp.Controllers
{
    [Authorize]
    public class FideliteController : Controller
    {
        private readonly UserManager<Utilisateur> _userManager;

        public FideliteController(UserManager<Utilisateur> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var niveau = user.TotalPointsGagnes switch
            {
                >= 5000 => "Platine",
                >= 2000 => "Or",
                >= 500 => "Argent",
                _ => "Bronze"
            };

            var reduction = (user.PointsFidelite / 100) * 5m;

            ViewBag.Niveau = niveau;
            ViewBag.Reduction = reduction;

            return View(user);
        }
    }
}