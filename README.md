# 🛒 EcommerciApp

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core%20MVC-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/apps/aspnet)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4?logo=nuget)](https://docs.microsoft.com/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/sql-server)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap&logoColor=white)](https://getbootstrap.com/)

> **Application e-commerce complete developpee avec ASP.NET Core MVC 8.0**, incluant la gestion des produits, panier, commandes, avis clients et programme de fidelite.

---

## 📋 Table des matieres

- [Apercu](#-apercu)
- [Fonctionnalites](#-fonctionnalites)
- [Architecture](#-architecture)
- [Technologies](#-technologies)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Utilisation](#-utilisation)
- [Auteur](#-auteur)

---

## 🎯 Apercu

**EcommerciApp** est une plateforme e-commerce complete permettant :

- La **gestion de catalogue produits** par categories
- Un **systeme de panier** intuitif
- Un **processus de commande** avec validation
- Un **systeme d'avis et notation** des produits
- Un **programme de fidelite** avec points et recompenses
- Un **dashboard administrateur** pour la gestion

---

## ✨ Fonctionnalites

### 🛍️ Espace Client

| Fonctionnalite | Description |
|----------------|-------------|
| 🔐 **Authentification** | Inscription, connexion, gestion de profil |
| 🏠 **Accueil** | Affichage des produits en vedette |
| 🔍 **Catalogue** | Recherche et filtrage par categorie |
| 🛒 **Panier** | Ajout, modification, suppression d'articles |
| 📦 **Commandes** | Passage de commande avec confirmation |
| ⭐ **Avis** | Deposer un avis et une note sur les produits |
| 🎁 **Fidelite** | Accumulation de points et suivi des recompenses |

### ⚙️ Espace Administrateur

| Fonctionnalite | Description |
|----------------|-------------|
| 📊 **Dashboard** | Vue d'ensemble des statistiques |
| 📁 **Categories** | CRUD complet des categories |
| 📦 **Produits** | Gestion du catalogue (images, prix, stock) |
| 📋 **Commandes** | Suivi et gestion des commandes clients |
| 💬 **Avis** | Moderation des avis clients |

---

## 🏗️ Architecture

```
EcommerciApp/
├── Controllers/          # Controleurs MVC
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
├── Models/               # Modeles de donnees (Entity Framework)
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
├── Data/                 # Contexte de base de donnees
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
└── Program.cs            # Point d'entree
```

---

## 🛠️ Technologies

| Technologie | Version | Utilisation |
|-------------|---------|-------------|
| **.NET** | 8.0 | Framework principal |
| **ASP.NET Core MVC** | 8.0 | Architecture MVC |
| **Entity Framework Core** | 8.0 | ORM et acces donnees |
| **SQL Server** | 2022 | Base de donnees relationnelle |
| **Bootstrap** | 5.3 | Framework CSS responsive |
| **jQuery** | 3.7 | Manipulation DOM cote client |
| **jQuery Validation** | 1.19 | Validation des formulaires |

---

## 🚀 Installation

### Prerequis

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) (ou SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recommande) ou VS Code

### Etapes d'installation

```bash
# 1. Cloner le repository
git clone https://github.com/azizlejmi12/e-commerce.git
cd e-commerce

# 2. Restaurer les packages NuGet
dotnet restore

# 3. Configurer la chaine de connexion (voir section Configuration)

# 4. Appliquer les migrations
dotnet ef database update

# 5. Lancer l'application
dotnet run
```

L'application sera accessible a l'adresse : `https://localhost:7001` ou `http://localhost:5001`

---

## ⚙️ Configuration

### Chaine de connexion

Modifie le fichier `appsettings.json` avec tes informations SQL Server :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TON_SERVEUR;Database=EcommerciAppDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

---

## 📖 Utilisation

### Parcours Client

1. **Inscription** → Creer un compte utilisateur
2. **Navigation** → Parcourir les categories et produits
3. **Panier** → Ajouter des produits au panier
4. **Commande** → Valider et confirmer la commande
5. **Avis** → Noter et commenter les produits achetes
6. **Fidelite** → Consulter les points accumules

### Parcours Administrateur

1. Connexion au **Dashboard**
2. Gestion des **Categories** (ajout, modification, suppression)
3. Gestion des **Produits** (avec upload d'images)
4. Suivi des **Commandes**
5. Moderation des **Avis**

---

## 📸 Screenshots

> *Screenshots a ajouter dans le dossier `docs/screenshots/`*

| Page d'accueil | Catalogue | Panier |
|----------------|-----------|--------|
| ![Home](docs/screenshots/home.png) | ![Catalog](docs/screenshots/catalogue.png) | ![Cart](docs/screenshots/panier.png) |

---

## 🗺️ Feuille de route

- [ ] Integration d'un systeme de paiement (Stripe/PayPal)
- [ ] Envoi d'emails de confirmation
- [ ] Systeme de recherche avancee
- [ ] API REST pour application mobile
- [ ] Tests unitaires et d'integration
- [ ] Deploiement sur Azure

---

## 🤝 Contribution

Les contributions sont les bienvenues !

1. Fork le projet
2. Cree une branche (`git checkout -b feature/nouvelle-fonctionnalite`)
3. Commit tes changements (`git commit -m 'Ajout de...'`)
4. Push sur la branche (`git push origin feature/nouvelle-fonctionnalite`)
5. Ouvre une Pull Request

---

## 📝 License

Ce projet est sous licence **MIT**.

---

## 👤 Auteur

**Aziz Lejmi**

- GitHub: [@azizlejmi12](https://github.com/azizlejmi12)
- Projet: [EcommerciApp](https://github.com/azizlejmi12/e-commerce)

---

<div align="center">

⭐ **N'hesite pas a mettre une etoile si ce projet t'a ete utile !** ⭐

</div>
