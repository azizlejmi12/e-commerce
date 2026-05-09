using EcommerciApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcommerciApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilisateur>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Panier> Paniers { get; set; }
        public DbSet<LignePanier> LignePaniers { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<LigneCommande> LigneCommandes { get; set; }
        public DbSet<Avis> Avis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // IMPORTANT pour Identity

            // Précision pour les prix
            modelBuilder.Entity<Produit>()
                .Property(p => p.Prix)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LignePanier>()
                .Property(lp => lp.PrixUnitaire)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Commande>()
                .Property(c => c.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LigneCommande>()
                .Property(lc => lc.PrixUnitaire)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LigneCommande>()
                .Property(lc => lc.SousTotal)
                .HasPrecision(18, 2);

            // Relation 1-to-1 Utilisateur-Panier
            modelBuilder.Entity<Utilisateur>()
                .HasOne(u => u.Panier)
                .WithOne(p => p.Utilisateur)
                .HasForeignKey<Panier>(p => p.UtilisateurId);
            
            
            
            
            modelBuilder.Entity<Avis>()
                .HasIndex(a => new { a.CommandeId, a.UtilisateurId })
                 .IsUnique();

            modelBuilder.Entity<Avis>()
                .HasOne(a => a.Commande)
                .WithMany()
                .HasForeignKey(a => a.CommandeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Avis>()
                .HasOne(a => a.Utilisateur)
                .WithMany()
                .HasForeignKey(a => a.UtilisateurId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}