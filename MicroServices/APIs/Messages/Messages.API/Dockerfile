#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroServices/APIs/Messages/Messages.API/Messages.API.csproj", "MicroServices/APIs/Messages/Messages.API/"]
COPY ["MicroServices/APIs/Messages/Messages.DataAccess/Messages.DataAccess.csproj", "MicroServices/APIs/Messages/Messages.DataAccess/"]
COPY ["LendStuff/Shared/LendStuff.Shared.csproj", "LendStuff/Shared/"]
RUN dotnet restore "./MicroServices/APIs/Messages/Messages.API/./Messages.API.csproj"
COPY . .
WORKDIR "/src/MicroServices/APIs/Messages/Messages.API"
RUN dotnet build "./Messages.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Messages.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messages.API.dll"]