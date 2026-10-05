# SkogsInsikt

SkogsInsikt is a full-stack decision support application for forest owners. The application combines forest data with current weather information to provide risk assessments and recommendations for registered forest areas.

The project was developed as a portfolio project focused on modern Microsoft technologies, cloud deployment, API development, integrations, testing and user-oriented web development.

## Live application

Frontend:
https://hossein2551.github.io/SkogsInsikt/

Backend API:
https://skogsinsikt-api.azurewebsites.net

## Features

- User registration and login
- JWT-based authentication and authorization
- User-specific forest areas
- Create, edit and delete forest areas
- Interactive map with Leaflet
- Integration with external weather data
- Automated risk assessment
- Recommendations based on weather and forest data
- Analysis history for each forest area
- Protected API endpoints
- Server-side ownership validation
- Responsive React interface

## Technology

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Core Identity
- JWT authentication
- SQL Server / Azure SQL
- xUnit

### Frontend

- React
- TypeScript
- Vite
- Leaflet
- REST API integration

### Cloud and DevOps

- Microsoft Azure App Service
- Azure SQL Database
- GitHub Pages
- GitHub Actions
- Automated build and test workflow

## Architecture

The solution is divided into separate projects to keep responsibilities separated:

- `SkogsInsikt.Api` - API endpoints, authentication and application configuration
- `SkogsInsikt.Application` - application services and business logic
- `SkogsInsikt.Domain` - domain entities and abstractions
- `SkogsInsikt.Infrastructure` - database access, Identity and external integrations
- `SkogsInsikt.Tests` - unit and integration tests
- `skogsinsikt-web` - React and TypeScript frontend

The frontend communicates with the ASP.NET Core API through REST. Authentication is handled using JWT bearer tokens. Data is stored in Azure SQL through Entity Framework Core.

## Risk analysis

A forest owner can request an analysis for a registered forest area. The backend retrieves current weather information and combines it with forest data such as planting year.

The application evaluates conditions including:

- wind speed
- precipitation
- temperature
- forest age

The result is classified into a risk level and accompanied by a recommendation. Each analysis is stored so the user can view previous assessments.

The current rules are transparent demonstration rules designed for the application and should not be interpreted as scientifically validated forestry recommendations.

## Security

API endpoints that handle forest data require authentication.

Forest areas are connected to the authenticated user's identity, and ownership is verified on the server. This prevents one user from accessing or modifying another user's forest areas.

Passwords are handled through ASP.NET Core Identity and are not stored directly by the application.

Secrets and production connection strings are configured through Azure application settings and are not committed to the repository.

## Testing

The solution contains both unit and integration tests.

Unit tests verify the risk analysis rules, while integration tests verify API behavior and application infrastructure using an isolated test database.

Tests are also executed automatically through GitHub Actions.

## CI/CD

GitHub Actions is used to automatically:

1. restore backend dependencies
2. build the .NET solution
3. run automated tests
4. install frontend dependencies
5. build the React application

The frontend is deployed to GitHub Pages and the backend runs in Azure App Service.

## Purpose

The goal of SkogsInsikt is to explore how modern software development can support forest owners through accessible digital decision support.

The project demonstrates full-stack development across frontend, backend, cloud infrastructure, databases, authentication, external integrations, automated testing and CI/CD.

## Screenshots

### Dashboard

The dashboard gives the forest owner an overview of registered forest areas, total area and current risk information.

![SkogsInsikt dashboard](docs/screenshots/dashboard.png)

### GIS overview

Registered forest areas are displayed on an interactive Leaflet map. Markers visualize the current risk level for each area.

![SkogsInsikt GIS overview](docs/screenshots/map.png)

### Forest risk analysis

The application combines forest information with current weather data to calculate a risk level, provide a recommendation and store the result in the analysis history.

![SkogsInsikt risk analysis](docs/screenshots/analysis.png)

