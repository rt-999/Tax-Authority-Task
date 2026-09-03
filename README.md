Fullstack Project (Angular + .NET API + SQL Server)
This repository contains a fullstack application environment built with C# (.NET Web API) Backend, Angular Frontend, and SQL Server Database.

🛠️ Prerequisites
Ensure you have the following installed on your machine:

.NET SDK 8.0+

Node.js (v18+) and npm

Angular CLI (npm install -g @angular/cli)

SQL Server (Local instance or via Docker)

Git

📁 Project Structure
├── backend/          # C# (.NET Web API) project
├── frontend/         # Angular application
├── .gitignore        # Git ignore rules
└── README.md         # Project documentation

🚀 Getting Started
1. Backend (.NET Web API)
Navigate to the backend directory:
cd backend/MyProject.Api

Update the ConnectionString in appsettings.json with your local SQL Server details.

Apply database migrations:
dotnet ef database update

Run the API application:
dotnet run

The API will run locally at https://localhost:7xxx.

2. Frontend (Angular)
Navigate to the frontend directory:
cd frontend

Install dependencies:
npm install

Start the Angular development server:
ng serve

Open your browser and navigate to: http://localhost:4200

⚙️ Git Setup
Clone the repository:
git clone https://github.com/rt-999/Tax-Authority-Task
cd Tax-Authority-Task

Commit and Push changes:
git status
git add .
git commit -m "Initial commit: Setup project structure"
git push origin main