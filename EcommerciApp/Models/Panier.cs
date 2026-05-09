namespace EcommerciApp.Models
{
    public class Panier
    {
        public int Id { get; set; }
        public string UtilisateurId { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateModification { get; set; }

        // Navigation
        public Utilisateur Utilisateur { get; set; }
        public List<LignePanier> LignesPanier { get; set; }
    }
}
