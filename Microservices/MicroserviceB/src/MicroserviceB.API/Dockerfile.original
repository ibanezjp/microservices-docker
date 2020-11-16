#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Microservices/MicroserviceB.API/MicroserviceB.API.csproj", "Microservices/MicroserviceB.API/"]
RUN dotnet restore "Microservices/MicroserviceB.API/MicroserviceB.API.csproj"
COPY . .
WORKDIR "/src/Microservices/MicroserviceB.API"
RUN dotnet build "MicroserviceB.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceB.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceB.API.dll"]