# 🖥️ Computer Store – Fullstack E-Commerce Plattform

Dieses Projekt ist eine umfassende E-Commerce-Lösung für einen Computer-Onlineshop. Es wurde als Fullstack-Anwendung entwickelt, um moderne Software-Architekturen und Best Practices in der Web-Entwicklung zu demonstrieren – von einer robusten REST-API im Backend bis hin zu einem responsiven Frontend.

---

## ✨ Key Features

### Backend (Core Logic)
- **RESTful API:** Strukturierte Endpunkte für Produkte, Benutzerverwaltung und Bestellungen.
- **Authentifizierung:** Sicherer Zugriff mittels **JWT (JSON Web Tokens)** und Rollenmanagement.
- **Zahlungsintegration:** Voll funktionsfähige **PayPal-Anbindung** für den Checkout-Prozess.
- **Hintergrundverarbeitung:** Einsatz von **Redis-Queues** für asynchronen E-Mail-Versand (separater `EmailWorker`-Dienst).
- **Echtzeit-Kommunikation:** **SignalR**-Hub (`OrderHub`) für Live-Updates des Bestellstatus.
- **Datenmanagement:** Hybrid-Ansatz mit **Entity Framework Core** (SQL) und **MongoDB** für flexible Produktkataloge.
- **API-Dokumentation:** Integriertes **Swagger UI** mit JWT-Authentifizierung zum einfachen Testen der Endpunkte.

### Frontend (User Experience)
- **Modernes UI:** Entwickelt mit **React 18** unter Nutzung von Hooks und funktionalen Komponenten.
- **Responsive Design:** Mobile-First-Ansatz mit **Tailwind CSS**, optimiert für alle Endgeräte.
- **Echtzeit-Updates:** Integration von **SignalR** (`@microsoft/signalr`) für Live-Benachrichtigungen.
- **Zahlungsabwicklung:** Direktintegration des **PayPal SDKs** (`@paypal/react-paypal-js`) im Frontend.
- **Routing & Navigation:** Seitennavigation mit **React Router v6**.
- **Benachrichtigungen:** Nutzerfreundliche Toast-Meldungen via **react-hot-toast**.
- **HTTP-Kommunikation:** API-Anfragen über **Axios**.

---

## 🗂️ Tech Stack

| Bereich | Technologie |
| :--- | :--- |
| **Backend** | C# (.NET), ASP.NET Core Web API |
| **Frontend** | React 18, JavaScript (ES6+), Tailwind CSS |
| **Datenbanken** | MongoDB (NoSQL), SQL Server (via EF Core) |
| **Echtzeit** | SignalR |
| **Caching / Queues** | Redis |
| **Sicherheit & Tools** | JWT, Swagger UI, PayPal REST API, Axios |

---

## 🏗️ Architektur & Patterns

Das Projekt folgt professionellen Software-Standards, um Skalierbarkeit und Wartbarkeit zu gewährleisten:

- **Repository & Service Pattern:** Klare Trennung von Datenzugriff (`Repositories`) und Geschäftslogik (`Services`) im `ApplicationCore`-Projekt.
- **Dependency Injection:** Konsequente Nutzung des integrierten .NET DI-Containers.
- **DTOs (Data Transfer Objects):** Entkoppelter, sicherer Datenaustausch zwischen API-Schicht und Frontend (`ApplicationCore/DTO`, `WebAPI/DTO`).
- **Clean Architecture:** Aufteilung in drei Projekte – `ApplicationCore` (Domain), `Infrastructure` (Datenzugriff), `WebAPI` (Präsentationsschicht).

### Projektstruktur

```
Computer_Store_ASPNET/
├── backend/
│   ├── ApplicationCore/        # Domain-Logik: Entities, DTOs, Repositories-Interfaces, Services
│   ├── Infrastructure/
│   │   ├── EF/                 # Entity Framework Core (SQL) – Repositories, Services, Migrationen
│   │   └── Mongo/              # MongoDB – Repositories, Services, Mapper
│   ├── WebAPI/                 # ASP.NET Core API – Controller, Hubs (SignalR), Middleware
│   └── EmailWorker/            # Hintergrunddienst für asynchronen E-Mail-Versand via Redis
└── frontend/                   # React 18 SPA mit Tailwind CSS
```

---

## ⚙️ Installation & Setup

### Voraussetzungen

- [Docker](https://www.docker.com/get-started) und [Docker Compose](https://docs.docker.com/compose/install/) installiert

### 1. Repository klonen

```bash
git clone https://github.com/Radus9991/Computer_Store_ASPNET.git
cd Computer_Store_ASPNET
```

### 2. Umgebungsvariablen konfigurieren

Öffne die `docker-compose.yml` und trage deine eigenen Werte ein:

#### 🔧 `webapi`-Dienst

| Variable | Beschreibung | Beispiel |
| :--- | :--- | :--- |
| `ConnectionStrings__Redis` | Redis-Verbindungszeichenfolge | `redis:6379` |
| `Redis__ConnectionString` | Redis-Verbindungszeichenfolge (alternativer Config-Key) | `redis:6379` |
| `PayPal__ClientId` | PayPal REST API Client-ID | `AaBbCc...` |
| `PayPal__ClientSecret` | PayPal REST API Client-Secret | `EeFfGg...` |
| `Jwt__Key` | Geheimer Schlüssel für JWT-Token-Signierung (min. 32 Zeichen) | `mein-geheimer-schluessel-123456` |

> **MongoDB** ist vollständig über Docker vorkonfiguriert – kein externer Connection-String erforderlich. Die API verbindet sich automatisch mit dem `mongo`-Container unter `mongodb://mongo:27017`, Datenbankname: `PcShop`.

#### 📧 `emailworker`-Dienst

| Variable | Beschreibung | Beispiel |
| :--- | :--- | :--- |
| `REDIS_URL__ConnectionString` | Redis-Verbindungszeichenfolge für den E-Mail-Worker | `redis:6379` |
| `MAILSENDER` | Absender-E-Mail-Adresse | `noreply@example.com` |
| `MAILPASSWORD` | Passwort oder App-Passwort des Absenderkontos | `dein-mail-passwort` |
| `MAILHOST` | SMTP-Hostname | `smtp.gmail.com` |
| `MAILPORT` | SMTP-Port | `587` |

### 3. Anwendung starten

```bash
docker compose up --build
```

| Dienst | URL |
| :--- | :--- |
| **Frontend (React)** | http://localhost:3000 |
| **Web API** | http://localhost:8080 |
| **Swagger UI** | http://localhost:8080/swagger |
| **MongoDB** | `localhost:27018` (nur lokaler Zugriff) |

---

## 🐳 Docker-Übersicht

```
Dienste:
  webapi       → ASP.NET Core Web API    (Ports: 5012, 80, 5000, 8080)
  emailworker  → Asynchroner E-Mail-Dienst via Redis-Queue
  frontend     → React SPA via Nginx     (Port: 3000)
  mongo        → MongoDB 8               (Daten in Docker-Volume gespeichert)
  redis        → Redis 7 Alpine          (nur intern erreichbar)
```

> MongoDB wird vollständig von Docker verwaltet. Die Daten werden im Volume `mongo_data` persistiert – keine externe MongoDB-Instanz oder Atlas-Verbindung erforderlich.

---

## 🛑 Anwendung beenden

```bash
docker compose down
```

Um auch die persistenten Daten (MongoDB-Volume) zu löschen:

```bash
docker compose down -v
```
