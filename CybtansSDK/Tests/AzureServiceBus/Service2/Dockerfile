#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Tests/AzureServiceBus/Service2/Service2.csproj", "Tests/AzureServiceBus/Service2/"]
RUN dotnet restore "Tests/AzureServiceBus/Service2/Service2.csproj"
COPY . .
WORKDIR "/src/Tests/AzureServiceBus/Service2"
RUN dotnet build "Service2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service2.dll"]