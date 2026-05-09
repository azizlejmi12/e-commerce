namespace EcommerciApp.Models
{
    public class LignePanier
    {
        public int Id { get; set; }
        public int PanierId { get; set; }
        public int ProduitId { get; set; }
        public int Quantite { get; set; }
        public decimal? PrixUnitaire { get; set; } // Prix au moment de l'ajout

        // Navigation
        public Panier Panier { get; set; }
        public Produit Produit { get; set; }
    }
}
