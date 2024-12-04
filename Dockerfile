FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["src/Svintus.Microservices.Movies/Svintus.Microservices.Movies.csproj", "Svintus.Microservices.Movies/"]
COPY ["src/Svintus.Movies.Application/Svintus.Movies.Application.csproj", "Svintus.Movies.Application/"]
COPY ["src/Svintus.Movies.DataAccess/Svintus.Movies.DataAccess.csproj", "Svintus.Movies.DataAccess/"]
COPY ["src/Svintus.Movies.Integrations/Svintus.Movies.Integrations.csproj", "Svintus.Movies.Integrations/"]
RUN dotnet restore "Svintus.Microservices.Movies/Svintus.Microservices.Movies.csproj"

COPY src ./
WORKDIR "/src/Svintus.Microservices.Movies"
RUN dotnet build "Svintus.Microservices.Movies.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Svintus.Microservices.Movies.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Svintus.Microservices.Movies.dll"]
