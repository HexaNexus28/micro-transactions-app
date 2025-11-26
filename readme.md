# API Micro-Transactions RPG

Architecture minimaliste. Logique métier encapsulée. Aucune tolérance pour le flou.

Cette API simule un système réel de micro-transactions façon RPG : achats d’items, gestion des tokens d’authentification, transactions, cohérence métier et validations strictes.  
Objectif : un service qui se comporte comme un backend “production-ready”, pas une démo.

---

## 🧩 Architecture (vue d’ensemble)

```mermaid
classDiagram

    class User {
        +email
        +password
    }

    class AuthToken {
        +emissionDate
        +expirationDate
    }

    class Transaction {
        +transactionDate
        +getTotalAmount()
    }

    class Items {
        +name
        +description
        +price
    }

    User "1" --> "*" AuthToken
    User "1" --> "*" Transaction
    Transaction "*" --> "*" Items
```

---

## 🔧 Stack & Tech

- **Langage / Framework :** C# / ASP.NET Core
- **Architecture en couches :** Controllers, Business, Core, Data
- **Validations strictes :** règles métier + intégrité
- **Gestion d’erreurs :** centralisée
- **Documentation :** Swagger clair et complet

**Prêt à évoluer vers :**

- Authentification robuste (JWT, OAuth…)
- Persistance réelle (EF Core, SQL)

---

### 🚀 Fonctionnalités

- Gestion complète des utilisateurs
- Authentification via tokens éphémères
- Création et suivi des transactions
- Ajout et gestion des items
- Calcul automatique des montants
- Règles métier isolées et testables
- Architecture pensée pour la charge et l’évolution

---

### 📂 Structure du projet

```bash
/Core
    Configuration

    Entities

    DTOs

    /Interfaces
        Repositories
        Services

    Mapping

/Business
    Services


/Data
    /Dtos
        Requests
        Responses

    Repositories

    Context

    UnitofWork


/API
    Controllers
    Middleware


```

---

### 🔗 Liens

- **Code complet :** [GitHub](https://github.com/HexaNexus28/micro-transactions-app.git)
- **Contact technique / discussions archi :** [LinkedIn](https://www.linkedin.com/in/yawozoglo)

---

### 🎯 Vision

Créer un service simple, robuste, contrôlé.  
Évoluer vers un backend **production-grade** : stabilité, observabilité, scalabilité et discipline.

Ouvert aux retours et critiques d’architecture.
