
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KubernetesExample.DbMigratorRunner/KubernetesExample.DbMigratorRunner.csproj", "KubernetesExample.DbMigratorRunner/"]
COPY ["KubernetesExample.SharedDataStorage/KubernetesExample.SharedDataStorage.csproj", "KubernetesExample.SharedDataStorage/"]
RUN dotnet restore "./KubernetesExample.DbMigratorRunner/KubernetesExample.DbMigratorRunner.csproj"
COPY . .
WORKDIR "/src/KubernetesExample.DbMigratorRunner"
RUN dotnet build "./KubernetesExample.DbMigratorRunner.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KubernetesExample.DbMigratorRunner.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KubernetesExample.DbMigratorRunner.dll"]