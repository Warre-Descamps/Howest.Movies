FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Howest.Movies.WebApi/Howest.Movies.WebApi.csproj", "Howest.Movies.WebApi/"]
COPY ["Howest.Movies.Data/Howest.Movies.Data.csproj", "Howest.Movies.Data/"]
COPY ["Howest.Movies.Models/Howest.Movies.Models.csproj", "Howest.Movies.Models/"]
COPY ["Howest.Movies.AccessLayer/Howest.Movies.AccessLayer.csproj", "Howest.Movies.AccessLayer/"]
COPY ["Howest.Movies.Dtos/Howest.Movies.Dtos.csproj", "Howest.Movies.Dtos/"]
RUN dotnet restore "Howest.Movies.WebApi/Howest.Movies.WebApi.csproj"
COPY . .
WORKDIR "/src/Howest.Movies.WebApi"
RUN dotnet build "Howest.Movies.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Howest.Movies.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Howest.Movies.WebApi.dll"]
