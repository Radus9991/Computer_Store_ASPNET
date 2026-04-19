# Computer Store - Fullstack E-Commerce Plattform

Dieses Projekt ist eine umfassende E-Commerce-Lösung für einen Computer-Onlineshop. Es wurde als Fullstack-Anwendung entwickelt, um moderne Software-Architekturen und Best Practices in der Web-Entwicklung zu demonstrieren – von einer robusten REST-API im Backend bis hin zu einem responsiven Frontend.

## 🚀 Key Features

### Backend (Core Logic)
* **RESTful API:** Strukturierte Endpunkte für Produkte, Benutzerverwaltung und Bestellungen.
* **Authentifizierung:** Sicherer Zugriff mittels **JWT (JSON Web Tokens)** und Rollenmanagement.
* **Zahlungsintegration:** Voll funktionsfähige **PayPal-Anbindung** für den Checkout-Prozess.
* **Hintergrundverarbeitung:** Einsatz von **Redis-Queues** für asynchrone Aufgaben (z. B. automatisierter E-Mail-Versand).
* **Datenmanagement:** Hybrid-Ansatz mit **Entity Framework Core** (SQL) und **MongoDB** für flexible Produktkataloge.
* **API-Dokumentation:** Integriertes **Swagger UI** zum einfachen Testen der Endpunkte.

### Frontend (User Experience)
* **Modernes UI:** Entwickelt mit **React.js** unter Nutzung von Hooks und funktionalen Komponenten.
* **Responsive Design:** Mobile-First Ansatz, optimiert für alle Endgeräte unter Einhaltung von W3C-Standards.
* **State Management:** Effiziente Verwaltung von Warenkorb, Benutzer-Sessions und Produktfiltern.

---

## 🛠 Tech Stack

| Bereich | Technologie |
| :--- | :--- |
| **Backend** | C# (.NET 6/7), ASP.NET Core Web API |
| **Frontend** | React.js, JavaScript (ES6+), CSS3 / Styled Components |
| **Datenbanken** | MongoDB (NoSQL), SQL Server (via EF Core) |
| **Caching/Queues** | Redis |
| **Tools & Security** | JWT, Swagger, NuGet, npm |

---

## 🏗 Architektur & Patterns

Das Projekt folgt professionellen Software-Standards, um Skalierbarkeit und Wartbarkeit zu gewährleisten:
* **Repository & Service Pattern:** Saubere Trennung von Datenzugriff und Geschäftslogik.
* **Dependency Injection:** Konsequente Nutzung des integrierten .NET Core DI-Containers.
* **DTOs (Data Transfer Objects):** Sicherer und entkoppelter Datenaustausch zwischen API und Frontend.

---

## ⚙️ Installation & Setup

### Voraussetzungen
* [.NET 6.0/7.0 SDK](https://dotnet.microsoft.com/download)
* [Node.js & npm](https://nodejs.org/)
* Laufende Instanzen von **MongoDB** und **Redis** (lokal oder via Docker)

### Konfiguration
Vor dem Start müssen die Verbindungseinstellungen in der `appsettings.json` im Backend-Verzeichnis konfiguriert werden:
- MongoDB Connection String
- Redis Configuration
- PayPal Client ID & Secret
- JWT Secret Key
