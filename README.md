# Public Service Request

> A full-stack enterprise application built with .NET 10, Angular, and Docker.

This repository contains a containerized web application designed for performance and scalability. The backend is powered by a **.NET 10** Web API, the frontend is a modern **Angular** application, and the entire stack (including the database) is orchestrated using **Docker Compose**.

---

## Tech Stack

- **Backend:** .NET 10 (MVC), Entity Framework Core
- **Frontend:** Angular, TypeScript, TailwindCSS
- **Database:** PostgreSQL
- **Infrastructure:** Docker, Docker Compose

---

## Prerequisites

Before you begin, ensure you have the following installed on your local machine:

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org/) (v20+ recommended)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)

---

## Project Structure

```text
PublicServiceRequest/
├── Backend/                                             # .NET Backend
│   ├── PublicServiceRequestBackend/                     # Main project
│   │   ├── Controllers/                                 # API controllers
│   │   ├── Data/                                        # DbContext
│   │   ├── Middleware/                                  # Global exception handler
│   │   ├── Migrations/                                  # EF Core migrations
│   │   ├── Models/                                      # Entity models
│   │   ├── Properties/                                  # Launch settings
│   │   ├── Services/                                    # Business logic
│   │   ├── appsettings.json                             # App configuration
│   │   ├── Program.cs                                   # Entry point & DI setup
│   │   └── PublicServiceRequestBackend.csproj           # Project file
│   ├── PublicServiceRequestBackend.Tests/               # Unit tests
│   └── Dockerfile                                       # Backend Docker build
├── public-service-request-frontend/                     # Angular Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/                              # Login & task manager components
│   │   │   ├── models/                                  # Task & DTO models
│   │   │   ├── services/                                # Task service
│   │   │   ├── app.config.ts                            # App providers
│   │   │   ├── app.routes.ts                            # Route definitions
│   │   │   ├── app.ts                                   # Root component
│   │   │   └── app.html                                 # Root template
│   │   ├── environments/
│   │   │   ├── environment.ts                           # Local dev config
│   │   │   └── environment.production.ts                # Production config
│   │   └── main.ts                                      # Browser entry point
│   └── Dockerfile                                       # Frontend Docker build
├── .dockerignore
├── .env                                                 # Environment variables (do not commit)
├── docker-compose.yml                                   # Multi-container setup
└── README.md
```

---

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/kevyang267/PublicServiceRequest.git
   cd PublicServiceRequest
   ```

2. Create a `.env` file in the root directory:

   ```env
   DB_CONNECTION=Host=postgres;Database=ServiceRecordDb;Username=dbadmin;Password=yourpassword
   Username=dbadmin
   Password=yourpassword
   Database=ServiceRecordDb
   ```

3. Start the application:

   ```bash
   docker compose up --build -d
   ```

4. Access the app at `http://localhost:4200`

The API will be available at `http://localhost:5000` and Swagger at `http://localhost:5000/swagger`.

---

## Environment Variables

| Variable        | Description                                    |
| --------------- | ---------------------------------------------- |
| `DB_CONNECTION` | Full EF Core connection string for the backend |
| `Username`      | PostgreSQL username                            |
| `Password`      | PostgreSQL password                            |
| `Database`      | PostgreSQL database name                       |

> **Note:** Never commit your `.env` file. It is listed in `.dockerignore` to prevent it from being included in Docker builds.

---

## API Endpoints

| Method | Route               | Description      |
| ------ | ------------------- | ---------------- |
| GET    | `/api/records`      | Get all records  |
| GET    | `/api/records/{id}` | Get record by ID |
| POST   | `/api/records`      | Create a record  |
| PATCH  | `/api/records/{id}` | Update a record  |
| DELETE | `/api/records/{id}` | Delete a record  |

---

## Running Tests

**Backend:**

```bash
cd Backend
dotnet test
```

---
