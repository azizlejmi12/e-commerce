using EcommerciApp.Data;
using EcommerciApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. CONFIGURATION DES SERVICES
// ============================================================

// Connexion à la Base de Données SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration d'ASP.NET Core Identity (Utilisateurs & Rôles)
builder.Services.AddIdentity<Utilisateur, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configuration des Cookies d'authentification
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Activation du Cache et de la Session (Indispensable pour le Panier)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Le panier expire après 30 min d'inactivité
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ============================================================
// 2. PIPELINE DE REQUÊTE (MIDDLEWARES)
// ============================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Activation de la Session (Doit être avant l'autorisation)
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ============================================================
// 3. INITIALISATION DES DONNÉES DE BASE (ADMIN & RÔLES)
// ============================================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<Utilisateur>>();

    // Création des rôles de sécurité
    string[] roles = { "Admin", "Client" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Création du compte Administrateur par défaut (si inexistant)
    var adminEmail = "admin@ecommerciapp.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new Utilisateur
        {
            UserName = adminEmail,
            Email = adminEmail,
            Nom = "Admin",
            Prenom = "System",
            EmailConfirmed = true,
            Role = "Admin"
        };

        await userManager.CreateAsync(adminUser, "Admin123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

// Lancement de l'application
app.Run();