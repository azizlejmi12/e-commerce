namespace EcommerciApp.Models
{
    public class Commande
    {
        public int Id { get; set; }
        public string UtilisateurId { get; set; }
        public DateTime DateCommande { get; set; }
        public string Statut { get; set; } // "EnAttente", "Acceptee", "Annulee", "Livree"
        public decimal Total { get; set; }
        public string Telephone { get; set; }
        public string Ville { get; set; }
        public string AdresseLivraison { get; set; }
        public bool IsRead { get; set; } = true;

        // Navigation
        public Utilisateur Utilisateur { get; set; }
        
        public List<LigneCommande> LignesCommande { get; set; }
    }
}
