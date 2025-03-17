# TfL Journey Planner

This project uses the TfL API to show tube line status and plan journeys between locations. It is built with **ASP.NET Core (C#)** for the backend and **React** for the frontend. The app allows users to find journeys between stations using location names, and it retrieves stop point IDs automatically from the TfL API.

## Features

- View tube line status (e.g., delayed, good service).
- Plan journeys between two stations using location names.
- **Future plans**:
  - Add current location support.
  - Customize journey options based on user preferences.
  - Work on the design and improve the user interface (UI) for a better user experience.

## Setup

### 1. Clone the Repository

```bash
git clone https://github.com/Ameneh-Keshavarz/TFL.git
cd TFL
```
### 2. Backend Setup

- Install **.NET 6+**.
- Restore packages:
```bash
dotnet restore
```
- Add TfL API credentials to appsettings.json:
```bash

"TflApi": {
  "AppId": "123",
  "AppKey": "cb52c92815b94cabb22449624d95e007",
  "BaseUrl": "https://api.tfl.gov.uk/"
}
```
### 3. Frontend Setup
- Go to the client folder and install dependencies:
```bash
cd tfl-stats.client
npm install
```
-Start the frontend:
```bash
npm start
```
### 4. API Usage

The `/journey` endpoint accepts `from` and `to` locations (station names) and returns a list of possible journeys.

### Example Request:

```json
{
  "from": "Kingston",
  "to": "Piccadilly"
}
```
