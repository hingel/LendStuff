#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LendStuff/Server/LendStuff.Server.csproj", "LendStuff/Server/"]
COPY ["DataAccess/LendStuff.DataAccess.csproj", "DataAccess/"]
COPY ["LendStuff/Shared/LendStuff.Shared.csproj", "LendStuff/Shared/"]
COPY ["LendStuff/Client/LendStuff.Client.csproj", "LendStuff/Client/"]
RUN dotnet restore "./LendStuff/Server/./LendStuff.Server.csproj"
COPY . .
WORKDIR "/src/LendStuff/Server"
RUN dotnet build "./LendStuff.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LendStuff.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LendStuff.Server.dll"]