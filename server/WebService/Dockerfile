# ============================
# Build stage
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution and projects
COPY ../.. .

# Restore dependencies
RUN dotnet restore "WebService/WebService.csproj"

# Build and publish release
RUN dotnet publish "WebService/WebService.csproj" -c Release -o /app/publish

# ============================
# Runtime stage
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (Render automatically maps it, but good to declare)
EXPOSE 8080

# Run the app
ENTRYPOINT ["dotnet", "WebService.dll"]
