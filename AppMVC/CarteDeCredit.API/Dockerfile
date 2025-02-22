# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
# No need to copy the /src directory, as it's not part of your repository structure.
WORKDIR /CarteDeCredit.API
COPY ["CarteDeCredit.API/CarteDeCredit.API.csproj", "CarteDeCredit.API/"]
RUN dotnet restore "CarteDeCredit.API/CarteDeCredit.API.csproj" --configuration $BUILD_CONFIGURATION
COPY CarteDeCredit.API/ ./CarteDeCredit.API/
WORKDIR "/CarteDeCredit.API"
RUN dotnet build "CarteDeCredit.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CarteDeCredit.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "/app/CarteDeCredit.API.dll"]
