#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Saga.Orchestrator/Saga.Orchestrator.csproj", "Saga.Orchestrator/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Saga.Orchestrator/Saga.Orchestrator.csproj"
COPY . .
WORKDIR "/src/Saga.Orchestrator"
RUN dotnet build "Saga.Orchestrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Saga.Orchestrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Saga.Orchestrator.dll"]
