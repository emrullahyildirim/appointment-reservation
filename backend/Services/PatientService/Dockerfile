FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Services/PatientService/PatientService.API/PatientService.API.csproj", "Services/PatientService/PatientService.API/"]
COPY ["Services/PatientService/PatientService.Business/PatientService.Business.csproj", "Services/PatientService/PatientService.Business/"]
COPY ["Services/PatientService/PatientService.DataAccess/PatientService.DataAccess.csproj", "Services/PatientService/PatientService.DataAccess/"]
COPY ["Services/PatientService/PatientService.Entities/PatientService.Entities.csproj", "Services/PatientService/PatientService.Entities/"]
COPY ["Shared/Core/Core.csproj", "Shared/Core/"]

RUN dotnet restore "Services/PatientService/PatientService.API/PatientService.API.csproj"

COPY . .

WORKDIR "/src/Services/PatientService/PatientService.API"
RUN dotnet build "PatientService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PatientService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "PatientService.API.dll"]