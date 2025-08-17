# ----------------------------
# STEP 1: Build the application
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY HealthiFy.sln

# Copy project files
COPY ../../Entity/Entity.csproj Entity/
COPY ../../Data/Data.csproj Data/
COPY ../../Service/Service.csproj Service/
COPY ../../WebService/WebService.csproj WebService/

# Restore dependencies
RUN dotnet restore HealthiFy.sln

# Copy everything else
COPY ../../ ./

# Build and publish
RUN dotnet publish WebService/WebService.csproj -c Release -o /app/publish

# ----------------------------
# STEP 2: Run the application
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebService.dll"]
