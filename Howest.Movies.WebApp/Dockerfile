FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Howest.Movies.WebApp/Howest.Movies.WebApp.csproj", "Howest.Movies.WebApp/"]
COPY ["Howest.Movies.Sdk/Howest.Movies.Sdk.csproj", "Howest.Movies.Sdk/"]
COPY ["Howest.Movies.Dtos/Howest.Movies.Dtos.csproj", "Howest.Movies.Dtos/"]
RUN dotnet restore "Howest.Movies.WebApp/Howest.Movies.WebApp.csproj"
COPY . .
WORKDIR "/src/Howest.Movies.WebApp"
RUN dotnet build "Howest.Movies.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Howest.Movies.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Howest.Movies.WebApp.dll"]
