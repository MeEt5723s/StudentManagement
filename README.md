# Student Management System - ASP.NET Core Web API

A RESTful Web API for managing student records, built with ASP.NET Core (.NET 8), Dapper, and SQL Server. APIs are secured using JWT Authentication.

---

## Features

- Get all students
- Get student by ID
- Add new student
- Update student
- Delete student
- JWT Authentication
- Global Exception Handling Middleware
- Logging with Serilog
- Swagger API Documentation
- Layered Architecture (Controller -> Service -> Repository -> Database)
- Dapper ORM
- SQL Server Database

---

## Technologies Used

| Technology | Purpose |
|---|---|
| ASP.NET Core Web API (.NET 8) | Backend framework |
| Dapper | Micro ORM for database operations |
| SQL Server | Database |
| JWT Authentication | API security |
| Serilog | Logging |
| Swagger (Swashbuckle) | API documentation |

---

## Database Setup

### Students Table Schema

| Column | Type |
|---|---|
| Id | int (Primary Key, Identity) |
| Name | nvarchar(100) |
| Email | nvarchar(100) |
| Age | int |
| Course | nvarchar(100) |
| CreatedDate | datetime |

### SQL Script

```sql
CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Age INT,
    Course NVARCHAR(100),
    CreatedDate DATETIME
);
```

---

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server
- Visual Studio 2022 (or VS Code)

### Steps

**1. Clone the repository**
```bash
git clone https://github.com/yourusername/StudentManagementSystem.git
cd StudentManagementSystem
```

**2. Open the solution**

Open `StudentManagement.sln` in Visual Studio 2022.

**3. Update the connection string**

In `appsettings.json`, update the connection string with your SQL Server details:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True"
}
```

**4. Add JWT settings**

In `appsettings.json`, add the following section:

```json
"Jwt": {
  "Key": "ThisIsMySuperSecretKeyForJwtAuthentication12345",
  "Issuer": "StudentApi",
  "Audience": "StudentApiUsers"
}
```

**5. Create the database table**

Run the SQL script from the Database Setup section above in SQL Server Management Studio (SSMS).

**6. Run the project**

Press F5 in Visual Studio, or run from the terminal:

```bash
dotnet run --project StudentManagement
```

**7. Open Swagger**

Once the project is running, navigate to:
```
https://localhost:44354/swagger
```

**8. Get a JWT token**

Call the login endpoint:

```
POST /api/auth/login
```

Request body:
```json
{
  "username": "admin",
  "password": "password"
}
```

Copy the token from the response.

**9. Authorize in Swagger**

Click the "Authorize" button in Swagger UI and enter the token in this format:

```
Bearer YOUR_TOKEN_HERE
```

**10. Test the Student APIs**

All student endpoints are now accessible.

---

## API Endpoints

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| POST | /api/auth/login | Generate JWT Token | No |
| GET | /api/student | Get all students | Yes |
| GET | /api/student/{id} | Get student by ID | Yes |
| POST | /api/student | Add new student | Yes |
| PUT | /api/student | Update student | Yes |
| DELETE | /api/student/{id} | Delete student | Yes |

---

## Sample API Responses

### GET /api/student

```json
[
  {
    "id": 1,
    "name": "John Doe",
    "email": "john@gmail.com",
    "age": 22,
    "course": "Computer Science",
    "createdDate": "2025-04-01T10:00:00"
  }
]
```

### Error Response

```json
{
  "statusCode": 500,
  "message": "An unexpected error occurred.",
  "details": "Error details here"
}
```

---

## Project Structure

```
StudentManagementSolution/
│
├── StudentManagement/        # Web API (Controllers, Middleware, Program.cs)
├── Application/              # Services (Business Logic)
├── Domain/                   # Models / Entities
├── Infrastructure/           # Repositories (Dapper + SQL Server)
└── README.md
```

### Architecture Flow

```
Controller -> Service -> Repository -> Database
```

---

## Logging

Logs are written daily to:

```
Logs/log-yyyy-mm-dd.txt
```

Serilog handles all logging, including request details and exceptions caught by the Global Exception Handling Middleware.