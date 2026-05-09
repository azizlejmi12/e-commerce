🛒 EcommerciApp
https://dotnet.microsoft.com/
https://dotnet.microsoft.com/apps/aspnet
https://docs.microsoft.com/ef/core/
https://www.microsoft.com/sql-server
https://getbootstrap.com/
LICENSE
Application e-commerce complète développée avec ASP.NET Core MVC 8.0, incluant la gestion des produits, panier, commandes, avis clients et programme de fidélité.
📋 Table des matières
Aperçu
Fonctionnalités
Architecture
Technologies
Installation
Configuration
Utilisation
Screenshots
Auteur
🎯 Aperçu
EcommerciApp est une plateforme e-commerce complète permettant :
La gestion de catalogue produits par catégories
Un système de panier intuitif
Un processus de commande avec validation
Un système d'avis et notation des produits
Un programme de fidélité avec points et récompenses
Un dashboard administrateur pour la gestion
✨ Fonctionnalités
🛍️ Espace Client
Table
Fonctionnalité	Description
🔐 Authentification	Inscription, connexion, gestion de profil
🏠 Accueil	Affichage des produits en vedette
🔍 Catalogue	Recherche et filtrage par catégorie
🛒 Panier	Ajout, modification, suppression d'articles
📦 Commandes	Passage de commande avec confirmation
⭐ Avis	Déposer un avis et une note sur les produits
🎁 Fidélité	Accumulation de points et suivi des récompenses
⚙️ Espace Administrateur
Table
Fonctionnalité	Description
📊 Dashboard	Vue d'ensemble des statistiques
📁 Catégories	CRUD complet des catégories
📦 Produits	Gestion du catalogue (images, prix, stock)
📋 Commandes	Suivi et gestion des commandes clients
💬 Avis	Modération des avis clients
🏗️ Architecture
plain
Copy
EcommerciApp/
├── Controllers/          # Contrôleurs MVC
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── AvisController.cs
│   ├── CategoriesController.cs
│   ├── CommandesController.cs
│   ├── FideliteController.cs
│   ├── HomeController.cs
│   ├── LigneCommandesController.cs
│   ├── LignePaniersController.cs
│   ├── PaniersController.cs
│   └── ProduitsController.cs
│
├── Models/               # Modèles de données (Entity Framework)
│   ├── Avis.cs
│   ├── Categorie.cs
│   ├── Commande.cs
│   ├── ErrorViewModel.cs
│   ├── LigneCommande.cs
│   ├── LignePanier.cs
│   ├── Panier.cs
│   ├── Produit.cs
│   └── Utilisateur.cs
│
├── Data/                 # Contexte de base de données
│   └── ApplicationDbContext.cs
│
├── Migrations/           # Migrations EF Core
│   └── ...
│
├── Views/                # Vues Razor
│   ├── Admin/
│   ├── Auth/
│   ├── Avis/
│   ├── Categories/
│   ├── Commandes/
│   ├── Compte/
│   ├── Fidelite/
│   ├── Home/
│   ├── LigneCommandes/
│   ├── LignePaniers/
│   ├── Paniers/
│   ├── Produits/
│   └── Shared/
│
├── wwwroot/              # Fichiers statiques
│   ├── css/
│   ├── images/produits/  # Images des produits
│   ├── js/
│   └── lib/              # Librairies (Bootstrap, jQuery)
│
├── appsettings.json      # Configuration
└── Program.cs            # Point d'entrée
🛠️ Technologies
Table
Technologie	Version	Utilisation
.NET	8.0	Framework principal
ASP.NET Core MVC	8.0	Architecture MVC
Entity Framework Core	8.0	ORM et accès données
SQL Server	2022	Base de données relationnelle
Bootstrap	5.3	Framework CSS responsive
jQuery	3.7	Manipulation DOM côté client
jQuery Validation	1.19	Validation des formulaires
🚀 Installation
Prérequis
.NET 8.0 SDK
SQL Server (ou SQL Server Express)
Visual Studio 2022 (recommandé) ou VS Code
Étapes d'installation
bash
Copy
# 1. Cloner le repository
git clone https://github.com/azizlejmi12/e-commerce.git
cd e-commerce

# 2. Restaurer les packages NuGet
dotnet restore

# 3. Configurer la chaîne de connexion (voir section Configuration)

# 4. Appliquer les migrations
dotnet ef database update

# 5. Lancer l'application
dotnet run
L'application sera accessible à l'adresse : https://localhost:7001 ou http://localhost:5001
⚙️ Configuration
Chaîne de connexion
Modifie le fichier appsettings.json avec tes informations SQL Server :
JSON
Copy
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TON_SERVEUR;Database=EcommerciAppDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
Paramètres de l'application
JSON
Copy
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
📖 Utilisation
Compte Administrateur par défaut
⚠️ À configurer lors du premier lancement via les migrations ou la seeding.
Parcours Client
Inscription → Créer un compte utilisateur
Navigation → Parcourir les catégories et produits
Panier → Ajouter des produits au panier
Commande → Valider et confirmer la commande
Avis → Noter et commenter les produits achetés
Fidélité → Consulter les points accumulés
Parcours Administrateur
Connexion au Dashboard
Gestion des Catégories (ajout, modification, suppression)
Gestion des Produits (avec upload d'images)
Suivi des Commandes
Modération des Avis
📸 Screenshots

Table
Page d'accueil	Catalogue	Panier
📁 Structure de la base de données
plain
Copy
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│  Categories │────<│  Produits   │────<│    Avis     │
└─────────────┘     └─────────────┘     └─────────────┘
                           │
                           ▼
                    ┌─────────────┐
                    │ LignePanier │
                    └─────────────┘
                           │
                           ▼
                    ┌─────────────┐     ┌─────────────┐
                    │   Paniers   │────<│ Utilisateurs│
                    └─────────────┘     └─────────────┘
                           │
                           ▼
                    ┌─────────────┐     ┌─────────────┐
                    │   Commandes │────<│LigneCommande│
                    └─────────────┘     └─────────────┘

👤 Auteur
Aziz Lejmi
GitHub: @azizlejmi12
Projet: EcommerciApp
<div align="center">
⭐ N'hésite pas à mettre une étoile si ce projet t'a été utile ! ⭐
</div>