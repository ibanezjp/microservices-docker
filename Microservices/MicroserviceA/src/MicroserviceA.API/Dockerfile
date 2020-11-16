#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Microservices/MicroserviceA/src/MicroserviceA.API/MicroserviceA.API.csproj", "Microservices/MicroserviceA/src/MicroserviceA.API/"]
COPY ["Microservices/MicroserviceA/src/MicroserviceA.Application/MicroserviceA.Application.csproj", "Microservices/MicroserviceA/src/MicroserviceA.Application/"]
COPY ["Microservices/Common/Microservice.Common.EventBus/Microservice.Common.EventBus.csproj", "Microservices/Common/Microservice.Common.EventBus/"]
COPY ["Microservices/MicroserviceA/src/MicroserviceA.Business/MicroserviceA.Business.csproj", "Microservices/MicroserviceA/src/MicroserviceA.Business/"]
COPY ["Microservices/Common/Microservice.Common.EventBus.RabbitMQ/Microservice.Common.EventBus.RabbitMQ.csproj", "Microservices/Common/Microservice.Common.EventBus.RabbitMQ/"]
RUN dotnet restore "Microservices/MicroserviceA/src/MicroserviceA.API/MicroserviceA.API.csproj"
COPY . .
WORKDIR "/src/Microservices/MicroserviceA/src/MicroserviceA.API"
RUN dotnet build "MicroserviceA.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceA.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceA.API.dll"]
