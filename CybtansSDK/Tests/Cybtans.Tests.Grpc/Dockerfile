#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Tests/Cybtans.Tests.Grpc/Cybtans.Tests.Grpc.csproj", "Tests/Cybtans.Tests.Grpc/"]
RUN dotnet restore "Tests/Cybtans.Tests.Grpc/Cybtans.Tests.Grpc.csproj"
COPY . .
WORKDIR "/src/Tests/Cybtans.Tests.Grpc"
RUN dotnet build "Cybtans.Tests.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cybtans.Tests.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cybtans.Tests.Grpc.dll"]