# Step 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY WebService.csproj .
RUN dotnet restore WebService.csproj

# Copy everything else
COPY . .

# Build and publish
RUN dotnet publish -c Release -o /app/publish

# Step 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebService.dll"]
