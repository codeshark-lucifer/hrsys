# HRSYSTEM

HRSYSTEM is a Windows Forms human resources management application for CODESHARK. It uses C#/.NET, MySQL, and BCrypt password verification.

## Features

- User login with BCrypt password validation
- Dashboard navigation for HR modules
- Employee management
- Attendance check-in and check-out tracking
- Payroll records and payment status updates
- System overview metrics
- MySQL database schema and seed data in `schemas.sql`

## Tech Stack

- C# Windows Forms
- .NET 10 Windows target framework
- MySQL
- `MySql.Data`
- `BCrypt.Net-Next`

## Project Structure

```text
HRSYSTEM/
  HRSYSTEM.csproj          # Application project file
  Program.cs               # Application entry point
  Form1.cs                 # Login form logic
  DashboardForm.cs         # Main dashboard shell
  DatabaseHelper.cs        # MySQL helper
  EmployeesView.cs         # Employee management
  AttendanceView.cs        # Attendance management
  PayrollView.cs           # Payroll management
  OverviewView.cs          # Dashboard metrics
  SettingsView.cs          # Database/settings screen
schemas.sql                # MySQL database schema and demo data
HRSYSTEM.slnx              # Visual Studio solution file
```

## Requirements

- Windows
- Visual Studio with .NET desktop development workload
- .NET SDK that supports `net10.0-windows`
- MySQL Server or compatible local MySQL instance

## Database Setup

1. Start MySQL.
2. Import `schemas.sql` using phpMyAdmin, MySQL Workbench, or the MySQL CLI.
3. Confirm that the database `hrsys_db` was created.
4. Update the connection string in `HRSYSTEM/DatabaseHelper.cs` if your local MySQL username or password is different.

Current connection string:

```csharp
Server=localhost;Database=hrsys_db;Uid=codeshark;Pwd=sovann@1029;
```

For real use, move database credentials out of source code and use local configuration or environment variables.

## Demo Login

The seed data in `schemas.sql` includes demo users. Example:

```text
Email: mormleapsovann@gmail.com
Password: sovann@1029
Role: Admin
```

Other seeded users use `password123` as shown in the comments near the `users` insert statements in `schemas.sql`.

## Run the Application

From Visual Studio:

1. Open `HRSYSTEM.slnx`.
2. Restore NuGet packages if prompted.
3. Build the solution.
4. Run the `HRSYSTEM` project.

From the command line:

```powershell
dotnet restore .\HRSYSTEM\HRSYSTEM.csproj
dotnet build .\HRSYSTEM\HRSYSTEM.csproj
dotnet run --project .\HRSYSTEM\HRSYSTEM.csproj
```

## Notes

- `bin/`, `obj/`, `.vs/`, and user-specific IDE files are intentionally ignored by Git.
- `schemas.sql` drops and recreates tables, so do not run it against a database that contains data you need to keep.
- The application currently expects a local MySQL database and a valid `users` record with a BCrypt password hash.

