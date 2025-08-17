# ----------------------------
# STEP 1: Build the application
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ../../HealthiFy.sln ./

# Copy project files
COPY ../../server/Entity/Entity.csproj Entity/
COPY ../../server/Data/Data.csproj Data/
COPY ../../server/Service/Service.csproj Service/
COPY ../../server/WebService/WebService.csproj WebService/

# Restore dependencies
RUN dotnet restore HealthiFy.sln

# Copy everything else
COPY ../../ ./

# Build and publish
RUN dotnet publish server/WebService/WebService.csproj -c Release -o /app/publish

# ----------------------------
# STEP 2: Run the application
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebService.dll"]
