using EcommerciApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EcommerciApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly SignInManager<Utilisateur> _signInManager;

        public AccountController(
            UserManager<Utilisateur> userManager,
            SignInManager<Utilisateur> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View("~/Views/Auth/Index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password, bool RememberMe)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                TempData["EmailError"] = "Cet email n'est pas associé à un compte.";
                TempData["EmailValue"] = Email;
                return RedirectToAction("Index");
            }

            var result = await _signInManager.PasswordSignInAsync(user, Password, RememberMe, false);

            if (!result.Succeeded)
            {
                TempData["PasswordError"] = "Le mot de passe est incorrect.";
                TempData["EmailValue"] = Email;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string Nom, string Prenom, string Email, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                TempData["RegisterError"] = "Les mots de passe ne correspondent pas.";
                return RedirectToAction("Index", new { register = 1 });
            }

            var user = new Utilisateur
            {
                UserName = Email,
                Email = Email,
                Nom = Nom,
                Prenom = Prenom,
                Role = "Client",
                PointsFidelite = 50,
                TotalPointsGagnes = 50
            };

            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Client");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            TempData["RegisterError"] = string.Join(", ", result.Errors.Select(e => e.Description));
            return RedirectToAction("Index", new { register = 1 });
        }

        [Authorize]
        public async Task<IActionResult> Profil()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            return View("~/Views/Compte/Profil.cshtml", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}