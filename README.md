# Foreign Nationals Appointment System
## سامانه نوبت‌دهی اتباع خارجی

A RESTful API for managing appointments of foreign nationals, built with ASP.NET Core and SQL Server.

## Features
- JWT Authentication
- Office & Province Management
- Holiday Management (official & emergency)
- Plan Management with dependencies
- Appointment scheduling with validations:
  - Foreign code validation (Yekta, Passport, Faragir, etc.)
  - Holiday checking
  - Working days validation
  - Office capacity control
  - Plan dependency validation
  - Date range validation

## Tech Stack
- **Backend:** ASP.NET Core 10 Web API
- **Database:** SQL Server
- **ORM:** Entity Framework Core 9
- **Authentication:** JWT Bearer Token
- **Password Hashing:** BCrypt

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server

### Installation
1. Clone the repository
```bash
   git clone https://github.com/AmirhosseinBarzegari/foreign-nationals-appointment.git
```
2. Update connection string in `appsettings.json`
3. Run database migrations
```bash
   dotnet ef database update
```
4. Run the project
```bash
   dotnet run
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/auth/login | Login |
| GET/POST/PUT/DELETE | /api/province | Province management |
| GET/POST/PUT/DELETE | /api/office | Office management |
| GET/POST/PUT/DELETE | /api/officesettings | Office settings |
| GET/POST/PUT/DELETE | /api/employee | Employee management |
| GET/POST/PUT/DELETE | /api/holiday | Holiday management |
| GET/POST/DELETE | /api/officeholidayexception | Holiday exceptions |
| GET/POST/PUT/DELETE | /api/plan | Plan management |
| GET/POST/DELETE | /api/plandependency | Plan dependencies |
| GET/POST/DELETE | /api/appointment | Appointment management |