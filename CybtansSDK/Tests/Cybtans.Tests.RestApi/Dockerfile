#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Tests/Cybtans.Tests.RestApi/Cybtans.Tests.RestApi.csproj", "Tests/Cybtans.Tests.RestApi/"]
COPY ["Tests/Cybtans.Tests.Services/Cybtans.Tests.Services.csproj", "Tests/Cybtans.Tests.Services/"]
COPY ["Tests/Cybtans.Tests.Domain/Cybtans.Tests.Domain.csproj", "Tests/Cybtans.Tests.Domain/"]
COPY ["Cybtans.Entities/Cybtans.Entities.csproj", "Cybtans.Entities/"]
COPY ["Cybtans.Entities.Proto/Cybtans.Entities.Proto.csproj", "Cybtans.Entities.Proto/"]
COPY ["Cybtans.Refit/Cybtans.Refit.csproj", "Cybtans.Refit/"]
COPY ["Cybtans.Serialization/Cybtans.Serialization.csproj", "Cybtans.Serialization/"]
COPY ["Tests/Cybtans.Tests.Models/Cybtans.Tests.Models.csproj", "Tests/Cybtans.Tests.Models/"]
COPY ["Cybtans.Messaging/Cybtans.Messaging.csproj", "Cybtans.Messaging/"]
COPY ["Cybtans.Services/Cybtans.Services.csproj", "Cybtans.Services/"]
COPY ["Cybtans.Entities.EventLog/Cybtans.Entities.EventLog.csproj", "Cybtans.Entities.EventLog/"]
COPY ["Cybtans.Expressions/Cybtans.Expressions.csproj", "Cybtans.Expressions/"]
COPY ["Tests/Cybtans.Tests.Domain.EF/Cybtans.Tests.Domain.EF.csproj", "Tests/Cybtans.Tests.Domain.EF/"]
COPY ["Cybtans.Entities.EntityFrameworkCore/Cybtans.Entities.EntityFrameworkCore.csproj", "Cybtans.Entities.EntityFrameworkCore/"]
COPY ["Cybtans.Messaging.RabbitMQ/Cybtans.Messaging.RabbitMQ.csproj", "Cybtans.Messaging.RabbitMQ/"]
COPY ["Cybtans.AspNetCore/Cybtans.AspNetCore.csproj", "Cybtans.AspNetCore/"]
RUN dotnet restore "Tests/Cybtans.Tests.RestApi/Cybtans.Tests.RestApi.csproj"
COPY . .
WORKDIR "/src/Tests/Cybtans.Tests.RestApi"
RUN dotnet build "Cybtans.Tests.RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cybtans.Tests.RestApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cybtans.Tests.RestApi.dll"]