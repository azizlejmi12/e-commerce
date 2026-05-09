using System.ComponentModel.DataAnnotations;

namespace EcommerciApp.Models
{
    public class Produit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string? Nom { get; set; } // Ajout du ?

        public string? Description { get; set; } // Ajout du ?

        public decimal Prix { get; set; }

        public int Stock { get; set; }

        public string? ImageUrl { get; set; } // Ajout du ?

        // Clé étrangère
        public int CategorieId { get; set; }

        // Relation de navigation
        public virtual Categorie? Categorie { get; set; } // Ajout du ?
    }
}