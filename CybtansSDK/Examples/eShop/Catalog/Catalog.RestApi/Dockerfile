#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Catalog/Catalog.RestApi/Catalog.RestApi.csproj", "Catalog/Catalog.RestApi/"]
COPY ["Catalog/Catalog.Services/Catalog.Services.csproj", "Catalog/Catalog.Services/"]
COPY ["Catalog/Catalog.Models/Catalog.Models.csproj", "Catalog/Catalog.Models/"]
RUN dotnet restore "Catalog/Catalog.RestApi/Catalog.RestApi.csproj"
COPY . .
WORKDIR "/src/Catalog/Catalog.RestApi"
RUN dotnet build "Catalog.RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.RestApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.RestApi.dll"]