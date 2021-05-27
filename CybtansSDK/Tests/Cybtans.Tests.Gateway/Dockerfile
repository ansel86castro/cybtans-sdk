#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Tests/Cybtans.Tests.Gateway/Cybtans.Tests.Gateway.csproj", "Tests/Cybtans.Tests.Gateway/"]
COPY ["Cybtans.Messaging.RabbitMQ/Cybtans.Messaging.RabbitMQ.csproj", "Cybtans.Messaging.RabbitMQ/"]
COPY ["Cybtans.Messaging/Cybtans.Messaging.csproj", "Cybtans.Messaging/"]
COPY ["Cybtans.Serialization/Cybtans.Serialization.csproj", "Cybtans.Serialization/"]
COPY ["Tests/Cybtans.Tests.Models/Cybtans.Tests.Models.csproj", "Tests/Cybtans.Tests.Models/"]
COPY ["Cybtans.AspNetCore/Cybtans.AspNetCore.csproj", "Cybtans.AspNetCore/"]
COPY ["Cybtans.Refit/Cybtans.Refit.csproj", "Cybtans.Refit/"]
COPY ["Cybtans.Services/Cybtans.Services.csproj", "Cybtans.Services/"]
COPY ["Cybtans.Entities.EventLog/Cybtans.Entities.EventLog.csproj", "Cybtans.Entities.EventLog/"]
COPY ["Cybtans.Entities/Cybtans.Entities.csproj", "Cybtans.Entities/"]
COPY ["Cybtans.Expressions/Cybtans.Expressions.csproj", "Cybtans.Expressions/"]
COPY ["Tests/Cybtans.Tests.Clients/Cybtans.Tests.Clients.csproj", "Tests/Cybtans.Tests.Clients/"]
RUN dotnet restore "Tests/Cybtans.Tests.Gateway/Cybtans.Tests.Gateway.csproj"
COPY . .
WORKDIR "/src/Tests/Cybtans.Tests.Gateway"
RUN dotnet build "Cybtans.Tests.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cybtans.Tests.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cybtans.Tests.Gateway.dll"]