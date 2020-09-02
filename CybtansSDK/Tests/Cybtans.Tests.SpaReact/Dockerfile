#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

WORKDIR /src
COPY ["Tests/Cybtans.Tests.SpaReact/Cybtans.Tests.SpaReact.csproj", "Tests/Cybtans.Tests.SpaReact/"]
RUN dotnet restore "Tests/Cybtans.Tests.SpaReact/Cybtans.Tests.SpaReact.csproj"
COPY . .
WORKDIR "/src/Tests/Cybtans.Tests.SpaReact"
RUN dotnet build "Cybtans.Tests.SpaReact.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cybtans.Tests.SpaReact.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cybtans.Tests.SpaReact.dll"]