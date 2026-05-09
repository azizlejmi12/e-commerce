using Microsoft.AspNetCore.Identity;

namespace EcommerciApp.Models
{
    public class Utilisateur : IdentityUser
    {
        // ✅ SEULEMENT tes propriétés personnalisées
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Role { get; set; } = "Client";
        public int PointsFidelite { get; set; } = 0;
        public int TotalPointsGagnes { get; set; } = 0;
        // Navigation
        public Panier Panier { get; set; }
        public List<Commande> Commandes { get; set; } = new List<Commande>();
    }
}