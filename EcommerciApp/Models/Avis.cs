namespace EcommerciApp.Models
{
    public class Avis
    {
        public int Id { get; set; }
        public int Note { get; set; }
        public string? Commentaire { get; set; }
        public DateTime DateAvis { get; set; } = DateTime.Now;

        public int CommandeId { get; set; }
        public Commande Commande { get; set; } = null!;

        public string UtilisateurId { get; set; } = null!;
        public Utilisateur Utilisateur { get; set; } = null!;
    }
}