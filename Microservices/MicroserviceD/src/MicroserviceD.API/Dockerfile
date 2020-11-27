#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Microservices/MicroserviceD/src/MicroserviceD.API/MicroserviceD.API.csproj", "Microservices/MicroserviceD/src/MicroserviceD.API/"]
COPY ["Microservices/MicroserviceD/src/MicroserviceD.Application/MicroserviceD.Application.csproj", "Microservices/MicroserviceD/src/MicroserviceD.Application/"]
COPY ["Microservices/Common/Microservice.Common.EventBus/Microservice.Common.EventBus.csproj", "Microservices/Common/Microservice.Common.EventBus/"]
COPY ["Microservices/MicroserviceD/src/MicroserviceD.Business/MicroserviceD.Business.csproj", "Microservices/MicroserviceD/src/MicroserviceD.Business/"]
RUN dotnet restore "Microservices/MicroserviceD/src/MicroserviceD.API/MicroserviceD.API.csproj"
COPY . .
WORKDIR "/src/Microservices/MicroserviceD/src/MicroserviceD.API"
RUN dotnet build "MicroserviceD.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceD.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceD.API.dll"]
