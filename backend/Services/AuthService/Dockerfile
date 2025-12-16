FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Services/AuthService/AuthService.API/AuthService.API.csproj", "Services/AuthService/AuthService.API/"]
COPY ["Services/AuthService/AuthService.Business/AuthService.Business.csproj", "Services/AuthService/AuthService.Business/"]
COPY ["Services/AuthService/AuthService.DataAccess/AuthService.DataAccess.csproj", "Services/AuthService/AuthService.DataAccess/"]
COPY ["Services/AuthService/AuthService.Entities/AuthService.Entities.csproj", "Services/AuthService/AuthService.Entities/"]
COPY ["Shared/Core/Core.csproj", "Shared/Core/"]

RUN dotnet restore "Services/AuthService/AuthService.API/AuthService.API.csproj"

COPY . .

WORKDIR "/src/Services/AuthService/AuthService.API"
RUN dotnet build "AuthService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.API.dll"]

