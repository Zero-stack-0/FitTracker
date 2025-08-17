# ============================
# Build stage
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything from repo root (since Build Context = repo root)
COPY . .

RUN dotnet restore ./server/WebService/WebService.csproj
RUN dotnet publish ./server/WebService/WebService.csproj -c Release -o /app/publish

# ============================
# Runtime stage
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "WebService.dll"]
