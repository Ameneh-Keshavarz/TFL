# ---------- Build React SPA (Vite) ----------
FROM node:20 AS webbuild
WORKDIR /app/client

# Install deps using lockfile if present
COPY tfl-stats.Client/package*.json ./
RUN npm ci

# Copy source and build (Vite -> dist)
COPY tfl-stats.Client/ .
RUN npm run build

# ---------- Build .NET (with all referenced projects) ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the entire repo so project references are available
COPY . .

# ✅ Install NSwag CLI for code generation
RUN dotnet tool install --global NSwag.ConsoleCore
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy built SPA into Server's wwwroot (Vite default output = dist)
COPY --from=webbuild /app/client/dist ./tfl-stats.Server/wwwroot

# Restore & publish the main web project
WORKDIR /src/tfl-stats.Server
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# ---------- Runtime image ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# ✅ Changed port from 8080 to 5000 to avoid conflicts
ENV ASPNETCORE_URLS=http://+:5000 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_GCServer=1

EXPOSE 5000

# Copy published output
COPY --from=build /app/publish .

# Start the app
ENTRYPOINT ["dotnet", "tfl-stats.Server.dll"]
