# =========================================================
# Etapa 1: build do frontend Angular
# =========================================================
FROM node:22-alpine AS frontend-build
WORKDIR /frontend
COPY frontend/package*.json ./
RUN npm ci
COPY frontend/ .
RUN npm run build -- --configuration production

# =========================================================
# Etapa 2: build do backend .NET 6
# =========================================================
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS backend-build
WORKDIR /backend
COPY backend/ProcessosAPI/*.csproj ./
RUN dotnet restore
COPY backend/ProcessosAPI/ .
RUN dotnet publish -c Release -o /app/publish --no-restore

# =========================================================
# Etapa 3: imagem final (Ubuntu + .NET runtime + Postgres + Nginx + Supervisor)
# =========================================================
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Instala Postgres, Nginx e Supervisor sobre a base Debian do .NET runtime
RUN apt-get update && apt-get install -y --no-install-recommends \
    postgresql postgresql-contrib \
    nginx \
    supervisor \
    curl \
    && rm -rf /var/lib/apt/lists/*

# ---- Configura o Postgres ----
ENV PG_VERSION=15
ENV POSTGRES_DB=minhaapp
ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=postgres
USER postgres
RUN /usr/lib/postgresql/*/bin/initdb -D /var/lib/postgresql/data
USER root

# ---- Copia artefatos das etapas anteriores ----
COPY --from=backend-build /app/publish /app/backend
COPY --from=frontend-build /frontend/dist/Front /var/www/html

# ---- Configs do Nginx e Supervisor ----
COPY single-container/nginx.conf /etc/nginx/sites-enabled/default
COPY single-container/supervisord.conf /etc/supervisor/conf.d/supervisord.conf
COPY single-container/entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=minhaapp;Username=postgres;Password=postgres"

EXPOSE 80 8080 5432

ENTRYPOINT ["/entrypoint.sh"]
