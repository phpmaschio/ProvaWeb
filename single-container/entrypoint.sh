#!/bin/bash
set -e

# Garante que o diretório de dados do Postgres pertence ao usuário postgres
chown -R postgres:postgres /var/lib/postgresql/data

# Sobe o Postgres brevemente em background para provisionar usuário/banco, se necessário
su postgres -c "pg_ctl -D /var/lib/postgresql/data -l /tmp/pg_boot.log start"

until su postgres -c "pg_isready -q"; do
  sleep 1
done

# Cria a role da aplicação, se ainda não existir
su postgres -c "psql -tc \"SELECT 1 FROM pg_roles WHERE rolname = '${APP_DB_USER}'\"" | grep -q 1 || \
  su postgres -c "psql -c \"CREATE ROLE ${APP_DB_USER} WITH LOGIN PASSWORD '${APP_DB_PASSWORD}';\""

# Cria o banco da aplicação, já com o usuário acima como dono, se ainda não existir
su postgres -c "psql -tc \"SELECT 1 FROM pg_database WHERE datname = '${APP_DB_NAME}'\"" | grep -q 1 || \
  su postgres -c "psql -c \"CREATE DATABASE ${APP_DB_NAME} OWNER ${APP_DB_USER};\""

su postgres -c "pg_ctl -D /var/lib/postgresql/data stop"

# A partir daqui, o supervisord assume o controle de todos os processos (postgres, backend, nginx)
exec /usr/bin/supervisord -c /etc/supervisor/conf.d/supervisord.conf
