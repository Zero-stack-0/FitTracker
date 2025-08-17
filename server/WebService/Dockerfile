# ============================
# Build stage
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything from the build context (server/)
COPY . .

# Restore dependencies
RUN dotnet restore ./WebService/WebService.csproj

# Build and publish release
RUN dotnet publish ./WebService/WebService.csproj -c Release -o /app/publish

# ============================
# Runtime stage
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "WebService.dll"]
