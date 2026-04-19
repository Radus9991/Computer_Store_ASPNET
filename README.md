# Computer Store - Fullstack E-Commerce Plattform

Dieses Projekt ist eine umfassende E-Commerce-Lösung für einen Computer-Onlineshop. Es wurde als Fullstack-Anwendung entwickelt, um moderne Software-Architekturen und Best Practices in der Web-Entwicklung zu demonstrieren – von einer robusten REST-API im Backend bis hin zu einem responsiven Frontend.

## 🚀 Features

### Backend (Core Logic)
* **RESTful API:** Strukturierte Endpunkte für Produkte, Benutzer und Bestellungen.
* **Authentifizierung:** Sicherer Login und Registrierung mittels **JWT (JSON Web Tokens)**.
* **Zahlungsintegration:** Voll funktionsfähige **PayPal-Anbindung** für den Checkout-Prozess.
* **Hintergrundverarbeitung:** Automatisierter E-Mail-Versand (z. B. Bestellbestätigungen) über **Redis-Queues**.
* **Datenmanagement:** Einsatz von **Entity Framework Core** für relationale Abläufe und **MongoDB** für flexible Datenstrukturen.

### Frontend (User Experience)
* **Modernes UI:** Entwickelt mit **React.js** für eine performante Single-Page-Application (SPA).
* **Responsive Design:** Optimiert für alle Endgeräte (Desktop, Tablet, Mobile) unter Einhaltung von W3C-Standards.
* **State Management:** Effiziente Verwaltung von Warenkorb und Benutzerstatus.

---

## 🛠 Tech Stack

| Bereich | Technologie |
| :--- | :--- |
| **Backend** | C#, ASP.NET Core, Entity Framework Core |
| **Frontend** | React.js, JavaScript (ES6+), CSS3 (Responsive) |
| **Datenbanken** | MongoDB, SQL (via EF Core) |
| **Caching/Queues** | Redis |
| **Sicherheit** | JWT (Stateless Authentication) |
| **Payment** | PayPal API |

---

## 🏗 Architektur

Das Projekt folgt dem **Clean Architecture** Ansatz und verwendet bewährte Entwurfsmuster (Design Patterns):
* **Repository Pattern:** Zur Entkopplung der Datenzugriffslogik von der Business-Logik.
* **Service Layer:** Zentralisierung der Geschäftslogik für bessere Testbarkeit und Wartbarkeit.
* **Dependency Injection:** Konsequente Nutzung des integrierten .NET DI-Containers.

---

## ⚙️ Installation & Setup

### Voraussetzungen
* [.NET 6.0/7.0 SDK](https://dotnet.microsoft.com/download)
* [Node.js & npm](https://nodejs.org/)
* [MongoDB](https://www.mongodb.com/) & [Redis](https://redis.io/) (lokal oder via Docker)

### Schritte
1.  **Repository klonen:**
    ```bash
    git clone https://github.com/Radus9991/Computer_Store_ASPNET.git
    ```
2.  **Backend starten:**
    ```bash
    cd backend/WebAPI
    dotnet restore
    dotnet run
    ```
3.  **Frontend starten:**
    ```bash
    cd frontend
    npm install
    npm start
    ```
