#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Apps/WebApp/WebApp.csproj", "Apps/WebApp/"]
COPY ["Ordering/Ordering.Clients/Ordering.Clients.csproj", "Ordering/Ordering.Clients/"]
COPY ["Ordering/Ordering.Models/Ordering.Models.csproj", "Ordering/Ordering.Models/"]
COPY ["../../Cybtans.Serialization/Cybtans.Serialization.csproj", "../../Cybtans.Serialization/"]
COPY ["../../Cybtans.Refit/Cybtans.Refit.csproj", "../../Cybtans.Refit/"]
COPY ["Catalog/Catalog.Clients/Catalog.Clients.csproj", "Catalog/Catalog.Clients/"]
COPY ["Catalog/Catalog.Models/Catalog.Models.csproj", "Catalog/Catalog.Models/"]
RUN dotnet restore "Apps/WebApp/WebApp.csproj"
COPY . .
WORKDIR "/src/Apps/WebApp"
RUN dotnet build "WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.dll"]
