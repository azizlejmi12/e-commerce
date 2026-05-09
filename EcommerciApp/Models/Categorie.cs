using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcommerciApp.Models
{
    public class Categorie
    {
        [Key] // Définit l'ID comme clé primaire
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string? Nom { get; set; }

        public string? Description { get; set; }

        // Navigation : Liste des produits liés à cette catégorie
        // Initialisé à 'new List<Produit>()' pour éviter les erreurs de type "NullReference"
        // Le '?' est crucial ici pour que ModelState ne demande pas de produits lors du "Create"
        public virtual List<Produit>? Produits { get; set; } = new List<Produit>();
    }
}