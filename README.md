# 🛒 E-Commerce Web API

A full-featured **E-Commerce RESTful API** built with **ASP.NET Core**, following clean architecture principles (Controllers, Services, Repositories).  
This API handles user authentication, product management, orders, payments, and much more — built for scalability and maintainability.

---

## 🚀 Features

### 🧱 Core Modules
- **Product Management** – CRUD operations, search, pagination, sorting.
- **Category Management** – Admin-only access with role-based control.
- **Cart & Wishlist** – Add, update, or remove items with user tracking.
- **Orders & Payments** – Linked entities with relational mapping.
- **Reviews & Ratings** – User feedback system integrated with products.

### 🔐 Authentication & Authorization
- **ASP.NET Identity** for secure user management.
- **JWT Token** authentication.
- **External Login (Google OAuth,Facebook OAuth)** integration.
- Role-based access control (Admin / User).

### ⚙️ Architecture & Patterns
- **Clean Layered Architecture** (Controllers → Services → Repositories).
- **Entity Framework Core (Code-First + Migrations)**.
- **Repository & Unit of Work Patterns** for clean data access.
- **DTOs (Data Transfer Objects)** for safe communication between layers.
- **Dependency Injection** for better maintainability and testing.

### 🛠️ Other Highlights
- **Exception Handling Middleware** for clean error responses.
- **Swagger UI** documentation with file upload support.
- **Async/Await** for non-blocking operations.
- **Logging & Validation** using ASP.NET built-in features.

---

## 🧩 Tech Stack

| Category | Technologies |
|-----------|---------------|
| **Framework** | ASP.NET Core Web API |
| **Language** | C# |
| **Database** | MS SQL Server |
| **ORM** | Entity Framework Core |
| **Auth** | ASP.NET Identity, JWT, Google OAuth , Facebook OAuth |
| **Documentation** | Swagger / Swashbuckle |
| **Architecture** | Clean Layered (Controller, Service, Repository) |

---

## 📁 Project Structure

E_Commerce/
│
├── Controllers/ # Handle HTTP requests
├── Services/ # Business logic layer
├── Repositories/ # Data access layer (EF Core)
├── DTOs/ # Data transfer models
├── Models/ # Entity models (EF Core)
├── Migrations/ # Code-first migrations
├── wwwroot/images/ # Uploaded product images
└── Program.cs / Startup.cs

---

## 🧰 Setup Instructions

### 1️⃣ Clone the repo
```bash
git clone https://github.com/ezzeldine22/GearWare
