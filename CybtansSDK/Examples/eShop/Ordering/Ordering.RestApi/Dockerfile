#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Ordering/Ordering.RestApi/Ordering.RestApi.csproj", "Ordering/Ordering.RestApi/"]
COPY ["Ordering/Ordering.Services/Ordering.Services.csproj", "Ordering/Ordering.Services/"]
COPY ["Ordering/Ordering.Models/Ordering.Models.csproj", "Ordering/Ordering.Models/"]
COPY ["../../Cybtans.Serialization/Cybtans.Serialization.csproj", "../../Cybtans.Serialization/"]
COPY ["../../Cybtans.AspNetCore/Cybtans.AspNetCore.csproj", "../../Cybtans.AspNetCore/"]
RUN dotnet restore "Ordering/Ordering.RestApi/Ordering.RestApi.csproj"
COPY . .
WORKDIR "/src/Ordering/Ordering.RestApi"
RUN dotnet build "Ordering.RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.RestApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.RestApi.dll"]
