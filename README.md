# E-Commerce Backend (.NET + SQL Server)

Backend بسيط لمتجر إلكتروني يبيع Hardware (لابتوبات، GPU، شاشات...) باستخدام **ASP.NET Core Web API** و **SQL Server** بنهج **Database-First**.  
المشروع معمول كـ MVP وبيغطي Products → Categories → Auth (لاحقًا) → Cart → Orders → Payments (Mock).

---

## ✨ Features (MVP)
- Products: CRUD كامل + ربط بـ Categories.
- Categories: CRUD وربط المنتجات.
- (لاحقًا) Auth (JWT): Register/Login + Roles (Admin/Customer).
- Cart: إضافة/تعديل/حذف عناصر الكارت لكل يوزر.
- Orders: إنشاء Order من الكارت + تتبّع الحالة.
- Payments (Mock): تسجيل عملية دفع وتحديث حالة الأوردر.
- Reviews, Wishlist (اختياري بعد الـ MVP).
- Concurrency بـ `RowVersion` لمنع الكتابة فوق تعديلات الآخرين.
- DTOs + (AutoMapper لاحقًا) لردود API نظيفة وآمنة.

---

## 🧱 Tech Stack
- **Runtime**: .NET 8 (أو 7)
- **Framework**: ASP.NET Core Web API
- **ORM**: Entity Framework Core (Database-First)
- **DB**: SQL Server
- **Docs/Testing**: Swagger / Postman
- **Logging**: ILogger (Serilog لاحقًا)

---

## 🧭 Architecture (مختصر)
- **Controllers**: تستقبل/ترجع Requests.
- **Services** (قريبًا): Business Logic.
- **Repositories** (اختياري).
- **Entities (Scaffolded)**: مولّدة من قاعدة البيانات.
- **DTOs**: Data Transfer Objects للـ API responses/requests.
- **Validation**: Data Annotations / FluentValidation (لاحقًا).

---

## 🗃️ Database Schema (مختصر)
الجداول الأساسية:
- `Users (UserId, Name, Email, PasswordHash VARBINARY, Role, CreatedAtUtc, IsActive, RowVersion)`
- `Categories (CategoryId, Name, Description, CreatedAtUtc, RowVersion)`
- `Products (ProductId, Name, Description, Price, StockQuantity, CategoryId FK, CreatedAtUtc, RowVersion)`
- `Carts (CartId, UserId UNIQUE FK, CreatedAtUtc, RowVersion)`
- `CartItems (CartItemId, CartId FK, ProductId FK, Quantity, AddedAtUtc, RowVersion)`
- `Orders (OrderId, UserId FK, Status, TotalAmount, ShippingAddress, OrderDateUtc, RowVersion)`
- `OrderItems (OrderItemId, OrderId FK, ProductId FK, Quantity, UnitPrice, RowVersion)`
- `Payments (PaymentId, OrderId UNIQUE FK, Amount, Status, Method, PaymentDateUtc, RowVersion)`
- `Reviews (UserId+ProductId UNIQUE, Rating 1..5, Comment, CreatedAtUtc, RowVersion)`
- `Wishlists (UserId+ProductId UNIQUE, CreatedAtUtc, RowVersion)`

> **مهم**: أعمدة `RowVersion ROWVERSION` لعمل Optimistic Concurrency.

---

## 🚀 Getting Started

### 1) Requirements
- .NET SDK 8 (أو 7)
- SQL Server (LocalDB/Express/Developer)
- Visual Studio 2022 أو VS Code

### 2) Clone & Restore
```bash
git clone <your-repo-url>
cd <repo-folder>
dotnet restore
