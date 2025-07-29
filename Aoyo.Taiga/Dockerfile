FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Aoyo.Taiga/Aoyo.Taiga.csproj", "Aoyo.Taiga/"]
RUN dotnet restore "Aoyo.Taiga/Aoyo.Taiga.csproj"
COPY . .
WORKDIR "/src/Aoyo.Taiga"
RUN dotnet build "./Aoyo.Taiga.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Aoyo.Taiga.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && \
    apt-get install -y curl && \
    rm -rf /var/lib/apt/lists/* \
    
HEALTHCHECK --interval=30s --timeout=3s --retries=3 CMD curl -f http://localhost:8080/aoyo/health || exit 1

ENTRYPOINT ["dotnet", "Aoyo.Taiga.dll"]

LABEL org.opencontainers.image.title="Aoyo.Taiga" \
      org.opencontainers.image.description="Taiga`s webhook catcher" \
      org.opencontainers.image.version="1.0.0" \
      org.opencontainers.image.url="https://github.com/yawaflua/Aoyo.Taiga" \
      org.opencontainers.image.source="https://github.com/yawaflua/Aoyo.Taiga" \
      org.opencontainers.image.licenses="Apache-2.0" \
      org.opencontainers.image.authors="Dmitrii Shimanskii <yawaflua@outlook.co.il>" \
      org.opencontainers.image.created="2025-07-29T12:00:00Z" \
      org.opencontainers.image.vendor="Diamond Studio"