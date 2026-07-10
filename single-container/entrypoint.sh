#!/bin/bash
set -e

# Garante que o diretório de dados do Postgres pertence ao usuário postgres
chown -R postgres:postgres /var/lib/postgresql/data

# Sobe o Postgres brevemente em background para criar o banco/usuário, se necessário
su postgres -c "/usr/lib/postgresql/15/bin/pg_ctl -D /var/lib/postgresql/data -l /tmp/pg_boot.log start"

until su postgres -c "pg_isready -q"; do
  sleep 1
done

su postgres -c "psql -tc \"SELECT 1 FROM pg_database WHERE datname = '${POSTGRES_DB}'\"" | grep -q 1 || \
  su postgres -c "createdb ${POSTGRES_DB}"

su postgres -c "/usr/lib/postgresql/15/bin/pg_ctl -D /var/lib/postgresql/data stop"

# A partir daqui, o supervisord assume o controle de todos os processos (postgres, backend, nginx)
exec /usr/bin/supervisord -c /etc/supervisor/conf.d/supervisord.conf
