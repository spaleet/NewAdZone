FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Ad/Ad.Api/Ad.Api.csproj", "src/Services/Ad/Ad.Api/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Logging/BuildingBlocks.Logging.csproj", "src/BuildingBlocks/BuildingBlocks.Logging/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Core/BuildingBlocks.Core.csproj", "src/BuildingBlocks/BuildingBlocks.Core/"]
COPY ["src/Services/Ad/Ad.Infrastructure/Ad.Infrastructure.csproj", "src/Services/Ad/Ad.Infrastructure/"]
COPY ["src/Services/Ad/Ad.Application/Ad.Application.csproj", "src/Services/Ad/Ad.Application/"]
COPY ["src/Services/Ad/Ad.Domain/Ad.Domain.csproj", "src/Services/Ad/Ad.Domain/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Persistence.Ef/BuildingBlocks.Persistence.Ef.csproj", "src/BuildingBlocks/BuildingBlocks.Persistence.Ef/"]
RUN dotnet restore "src/Services/Ad/Ad.Api/Ad.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Ad/Ad.Api"
RUN dotnet build "Ad.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ad.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ad.Api.dll"]