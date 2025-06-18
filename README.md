# PostgreSQL Console CRUD App 🚀

A modern C# Console application that demonstrates **CRUD operations** using **PostgreSQL** database with **Entity Framework Core** and **Code First** approach.

## 🎯 Features

- ✅ **Full CRUD Operations** (Create, Read, Update, Delete)
- ✅ **PostgreSQL Integration** with Entity Framework Core
- ✅ **Code First Approach** with automatic database creation
- ✅ **Async/Await Pattern** for better performance
- ✅ **Interactive Console Menu** with user-friendly interface
- ✅ **Data Validation** and error handling
- ✅ **Email Uniqueness** constraint
- ✅ **Formatted Output** with clean table display

## 🛠️ Tech Stack

- **Language:** C# (.NET 8.0)
- **Database:** PostgreSQL 17
- **ORM:** Entity Framework Core 8.0
- **Driver:** Npgsql (PostgreSQL .NET Driver)
- **IDE:** JetBrains Rider
- **Platform:** macOS (Cross-platform compatible)

## 📋 Prerequisites

Before running this application, make sure you have:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 17](https://www.postgresql.org/download/) installed and running
- [pgAdmin 4](https://www.pgadmin.org/) (optional, for database management)

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/Glyberax/PostgreSQL-Console-CRUD.git
cd PostgreSQL-Console-CRUD
```

### 2. Navigate to Project Directory
```bash
cd PostgreSQLCrudDemo
```

### 3. Install Dependencies
```bash
dotnet restore
```

### 4. Configure Database Connection
Update the connection string in `Program.cs`:
```csharp
optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TestDB;Username=postgres;Password=YOUR_PASSWORD");
```

### 5. Run the Application
```bash
dotnet run
```

## 📱 Usage

When you run the application, you'll see an interactive menu:

```
🚀 PostgreSQL Console CRUD Uygulaması
=====================================

📦 Veritabanı oluşturuluyor...
✅ Veritabanı hazır!

📋 İŞLEM MENÜSÜ:
1. Kullanıcı Ekle
2. Tüm Kullanıcıları Listele
3. Kullanıcı Ara (ID)
4. Kullanıcı Güncelle
5. Kullanıcı Sil
0. Çıkış

Seçiminiz:
```

### Available Operations:

- **Add User (1):** Create a new user with name, email, and age
- **List All Users (2):** Display all users in a formatted table
- **Search User (3):** Find a user by their ID
- **Update User (4):** Modify existing user information
- **Delete User (5):** Remove a user from the database
- **Exit (0):** Close the application

## 📊 Database Schema

The application creates a `Users` table with the following structure:

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Auto Increment |
| Name | varchar(100) | Required |
| Email | varchar(150) | Required, Unique |
| Age | int | Required |
| CreatedDate | timestamp | Auto-generated |

## 🏗️ Project Structure

```
PostgreSQLCrudDemo/
├── Program.cs              # Main application file
├── PostgreSQLCrudDemo.csproj  # Project configuration
└── README.md              # This file
```

## 📦 Dependencies

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.0" />
```

## 🔧 Configuration

### PostgreSQL Setup (macOS)
1. Install PostgreSQL using the official installer
2. Start PostgreSQL service
3. Create a database user (default: `postgres`)
4. Set a password for the user
5. Ensure PostgreSQL is running on port 5432

### Connection String Format
```
Host=localhost;Port=5432;Database=DATABASE_NAME;Username=USERNAME;Password=PASSWORD
```

## 🎨 Sample Output

```
📋 Tüm Kullanıcılar:
ID | İsim           | Email                    | Yaş | Oluşturma Tarihi
---|----------------|--------------------------|-----|------------------
 1 | Arda Çalışkan  | alkanlardal@icloud.com   |  18 | 17.06.2025 13:45
 2 | John Doe       | john@example.com         |  25 | 17.06.2025 13:47
```

## 👨‍💻 Author

**Arda Çalışkan**
- GitHub: [@Glyberax](https://github.com/Glyberax)
- Email: alkanlardal@icloud.com

## 🙏 Acknowledgments

- Entity Framework Core team for the excellent ORM
- PostgreSQL team for the robust database system
- JetBrains for Rider IDE

---

⭐ **If you found this project helpful, please give it a star!** ⭐
