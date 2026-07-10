# AbySalto Cart Service

Minimal ASP.NET Core 8 Web API implementation of a shopping cart service created as part of the AbySalto Senior Developer technical assignment.

## Features

- Shopping cart CRUD operations
- SQL Server database with Entity Framework Core
- RESTful API
- Request validation
- Global exception handling middleware
- Structured logging
- Swagger / OpenAPI documentation

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI

## Project Structure

```
Controllers
Services
Models
DTOs
Data
Middleware
Migrations
```

## Prerequisites

Before running the application, make sure you have:

- .NET 8 SDK
- SQL Server (or SQL Server Express)
- Entity Framework Core CLI

If EF Core CLI is not installed:

```bash
dotnet tool install --global dotnet-ef
```

---

## Database Configuration

Update the SQL Server connection string in **appsettings.json** according to your local environment.

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=AbySaltoCartDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

After configuring the connection string, create the database using Entity Framework Core migrations.

---

## Running the Application

### 1. Clone the repository

```bash
git clone <repository-url>
```

### 2. Navigate to the project

```bash
cd AbySalto.CartService
```

### 3. Restore NuGet packages

```bash
dotnet restore
```

### 4. Create the database

```bash
dotnet ef database update
```

### 5. Run the application

```bash
dotnet run
```

### 6. Open Swagger

```
https://localhost:<port>/swagger
```

---

## API Endpoints

```
GET    /api/cart/{userId}

POST   /api/cart/{userId}/items

PUT    /api/cart/{userId}/items/{itemId}

DELETE /api/cart/{userId}/items/{itemId}

DELETE /api/cart/{userId}/clear
```

---

## Architecture

The project follows a simple layered architecture:

```
Client
   │
   ▼
CartController
   │
   ▼
ICartService
   │
   ▼
CartService
   │
   ▼
Entity Framework Core
   │
   ▼
SQL Server
```

Responsibilities are separated into:

- Controllers
- Services
- DTOs
- Data (EF Core)
- Middleware
- Models

---

## Validation & Error Handling

The application includes:

- Request validation using Data Annotations
- Automatic model validation through ASP.NET Core
- Global exception handling middleware
- Structured logging using ILogger

---

## Notes

This project represents the **minimal Web API implementation** requested in the technical assignment.

The accompanying **High-Level System Design** document describes the proposed production architecture, including:

- API Gateway
- Microservices communication
- Scalability strategy
- Authentication & Security
- Monitoring & Alerting
- CI/CD pipeline
- Branching strategy
- External service integrations



