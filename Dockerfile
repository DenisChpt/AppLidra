# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copie des fichiers projet
COPY ["AppLidra.Shared/AppLidra.Shared.csproj", "AppLidra.Shared/"]
COPY ["AppLidra.Client/AppLidra.Client.csproj", "AppLidra.Client/"]
COPY ["AppLidra.Server/AppLidra.Server.csproj", "AppLidra.Server/"]

# Restauration des dépendances
RUN dotnet restore "AppLidra.Server/AppLidra.Server.csproj"
RUN dotnet restore "AppLidra.Client/AppLidra.Client.csproj"

# Copie du reste des fichiers
COPY . .

# Publication
RUN dotnet publish "AppLidra.Server/AppLidra.Server.csproj" -c Release -o /app/publish/server
RUN dotnet publish "AppLidra.Client/AppLidra.Client.csproj" -c Release -o /app/publish/client

# Stage final
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/publish/server ./
COPY --from=build /app/publish/client ./client

# Configuration des variables d'environnement
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_HTTP_PORTS=5000
ENV ASPNETCORE_HTTPS_PORTS=5001

EXPOSE 5000
EXPOSE 5001

# Exécuter uniquement le serveur
ENTRYPOINT ["dotnet", "AppLidra.Server.dll"]