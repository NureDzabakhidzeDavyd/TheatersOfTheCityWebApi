FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ["./TheatersOfTheCity/TheatersOfTheCity.Business/TheatersOfTheCity.Business.csproj", "./TheatersOfTheCity.Business/"]
COPY ["./TheatersOfTheCity/TheatersOfTheCity.Contracts/TheatersOfTheCity.Contracts.csproj", "./TheatersOfTheCity.Contracts/"]
COPY ["./TheatersOfTheCity/TheatersOfTheCity.Core/TheatersOfTheCity.Core.csproj", "./TheatersOfTheCity.Core/"]
COPY ["./TheatersOfTheCity/TheatersOfTheCity.Data/TheatersOfTheCity.Data.csproj", "./TheatersOfTheCity.Data/"]
COPY ["./TheatersOfTheCity/TheatersOfTheCity.Api/TheatersOfTheCity.Api.csproj", "./TheatersOfTheCity.Api/"]
COPY . .
RUN dotnet restore "./TheatersOfTheCity/TheatersOfTheCity.Api/TheatersOfTheCity.Api.csproj"
RUN dotnet build "./TheatersOfTheCity/TheatersOfTheCity.Api/TheatersOfTheCity.Api.csproj" -c Release -o /app
FROM build AS publish
RUN dotnet publish "./TheatersOfTheCity/TheatersOfTheCity.Api/TheatersOfTheCity.Api.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheatersOfTheCity.Api.dll"]

##Build Stage
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /app
#COPY ["./TheatersOfTheCity/TheatersOfTheCity.Business/TheatersOfTheCity.Business.csproj", "./TheatersOfTheCity.Business/"]
#COPY ["./TheatersOfTheCity/TheatersOfTheCity.Contracts/TheatersOfTheCity.Contracts.csproj", "./TheatersOfTheCity.Contracts/"]
#COPY ["./TheatersOfTheCity/TheatersOfTheCity.Core/TheatersOfTheCity.Core.csproj", "./TheatersOfTheCity.Core/"]
#COPY ["./TheatersOfTheCity/TheatersOfTheCity.Data/TheatersOfTheCity.Data.csproj", "./TheatersOfTheCity.Data/"]
#COPY ["./TheatersOfTheCity/TheatersOfTheCity.Api/TheatersOfTheCity.Api.csproj", "./TheatersOfTheCity.Api/"]
#COPY . .
#
#RUN dotnet restore "./TheatersOfTheCity/TheatersOfTheCity.Api/TheatersOfTheCity.Api.csproj"
#RUN dotnet publish "./TheatersOfTheCity/TheatersOfTheCity.Api/TheatersOfTheCity.Api.csproj" -c Release -o /app/publish
#
#
##Serve stage
#FROM mcr.microsoft.com/dotnet/sdk:6.0
#WORKDIR /app
#COPY --from=build /app/publish .
#EXPOSE 5000
#
#ENTRYPOINT ["dotnet", "TheatersOfTheCity.dll"]
#
#
