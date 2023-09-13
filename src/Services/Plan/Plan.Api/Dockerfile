#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Plan/Plan.Api/Plan.Api.csproj", "src/Services/Plan/Plan.Api/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Logging/BuildingBlocks.Logging.csproj", "src/BuildingBlocks/BuildingBlocks.Logging/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Core/BuildingBlocks.Core.csproj", "src/BuildingBlocks/BuildingBlocks.Core/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "src/BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["src/Services/Plan/Plan.Application/Plan.Application.csproj", "src/Services/Plan/Plan.Application/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Payment/BuildingBlocks.Payment.csproj", "src/BuildingBlocks/BuildingBlocks.Payment/"]
COPY ["src/Services/Plan/Plan.Domain/Plan.Domain.csproj", "src/Services/Plan/Plan.Domain/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Persistence.Mongo/BuildingBlocks.Persistence.Mongo.csproj", "src/BuildingBlocks/BuildingBlocks.Persistence.Mongo/"]
COPY ["src/Services/Plan/Plan.Infrastructure/Plan.Infrastructure.csproj", "src/Services/Plan/Plan.Infrastructure/"]
RUN dotnet restore "src/Services/Plan/Plan.Api/Plan.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Plan/Plan.Api"
RUN dotnet build "Plan.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Plan.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Plan.Api.dll"]