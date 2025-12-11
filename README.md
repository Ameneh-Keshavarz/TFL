# TfL Stats

A full-stack application for Transport for London (TfL) data with real-time tube line statuses, journey planning, and network visualization.

## Features

- **Line Status** - View current status of all TfL tube lines
- **Journey Planner** - Plan routes between stations with autocomplete
- **Line Selector** - Browse individual line diagrams
- **Map** - Interactive map visualization with arrival predictions
- **Data Recording** - Record and playback API responses for testing

## Tech Stack

- **Backend**: ASP.NET Core 8, C#, In-memory caching
- **Frontend**: React 19, Vite, Google Maps API, React Router
- **API Client**: Auto-generated from TfL API Swagger schema (NSwag)
- **Containerization**: Docker

## Prerequisites

- .NET 8 SDK
- Node.js 20+ and npm
- TfL API base URL in configuration

## Quick Start

### Backend

```bash
dotnet restore
dotnet run --project tfl-stats.Server
```

Configure `tfl-stats.Server/appsettings.json`:
```json
{
  "ApiSettings": {
    "BaseUrl": "https://api.tfl.gov.uk"
  }
}
```

### Frontend

```bash
cd tfl-stats.Client
npm install
npm run dev
```

## API Endpoints

- `GET /api/line` - Tube lines and statuses
- `GET /api/stoppoint` - Stations and stops
- `GET /api/journey` - Journey planning
- `GET /api/arrival` - Arrival predictions
- `GET /api/linediagram` - Line diagram data
- `GET /api/mapdata` - Network map data

## Development

- Linting: `npm run lint` (frontend)
- Swagger UI: `https://localhost:5000/swagger`

## License

MIT
