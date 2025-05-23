# ﻿Luftborn Interview Task
This project consists of a .NET 6 Web API backend and an Angular 14 frontend.

## Prerequisites
- **.NET 6 SDK**
- **Node.js (for Angular)**
- **Angular CLI**
- **SQL Server**

## Backend Setup (.NET 6 Web API)
Database Setup:
Ensure SQL Server is running

Open Package Manager Console

Set default project to "Infrastructure"

Run migration command:
**Update-Database**

Run the API:
The API will run on http://localhost:5073

Swagger UI will be available at http://localhost:5073/swagger

## Frontend Setup (Angular 14)
Install dependencies:
npm install

Configure environment:

Verify server URL in environment file:

typescript

serverUrl: "http://localhost:5073/api"

Run the frontend:
**npm start**

The application will run on http://localhost:4200

## Project Structure
**Backend**
- Clean architecture approach
- Web API project as startup
- Code-first database approach
- Entity Framework Core for data access

**Frontend**
- Angular 14
- Requires Node.js and Angular CLI


**Database name** 
"InventoryDB"

Development Notes
Backend profilers configured for both direct run and IIS Express



For any issues during setup, please verify all prerequisites are installed and services are running.
