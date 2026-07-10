# ProvaWeb - Sistema de Gestão de Processos

Aplicação web para cadastro e acompanhamento de processos, com gestão de partes envolvidas (interessadas e contrárias) e histórico de andamentos.

## Tecnologias utilizadas

**Backend**
- .NET 6 / ASP.NET Core Web API
- Entity Framework Core 6 + Npgsql (PostgreSQL)
- AutoMapper
- Swagger / Swashbuckle (documentação da API, ambiente de desenvolvimento)

**Frontend**
- Angular 22 (standalone components)
- Angular Material
- RxJS

**Banco de dados**
- PostgreSQL

**Infraestrutura**
- Docker (build multi-stage: Angular + .NET + PostgreSQL em um único container, orquestrado via supervisord + nginx)

## Como rodar o projeto

### Opção 1: Docker (recomendado)

Pré-requisito: Docker Desktop instalado e rodando.

```bash
docker compose up --build -d
```

Isso builda o frontend (Angular), o backend (.NET) e sobe um container único com Postgres, API e frontend servidos via nginx. As migrations do banco são aplicadas automaticamente na inicialização.

Acessos após subir:
- Frontend: http://localhost
- API direta: http://localhost:8080
- Postgres: localhost:5433 (usuário `processos_api_user`, senha `processos_api_password`, banco `processos_api`)

Para parar:
```bash
docker compose down
```

Para resetar o banco (apaga os dados):
```bash
docker compose down -v
```

> Se a porta 5432 já estiver em uso no host (outro Postgres local), o mapeamento já está ajustado para expor em `5433` no host, sem conflito.

### Opção 2: Rodando localmente sem Docker

**Backend**

Pré-requisitos: .NET 6 SDK, PostgreSQL rodando localmente.

1. Crie um banco `processos_api` no Postgres e um usuário com acesso a ele.
2. Ajuste a connection string em `backend/ProcessosAPI/appsettings.json` (`ConnectionStrings:ProcessoConnection`) se necessário.
3. Rode as migrations e suba a API:

```bash
cd backend/ProcessosAPI
dotnet restore
dotnet ef database update
dotnet run
```

A API sobe em `https://localhost:7237` (ou porta configurada em `launchSettings.json`).

**Frontend**

Pré-requisitos: Node.js e npm.

```bash
cd frontend
npm install
npm start
```

O frontend sobe em `http://localhost:4200` e usa `proxy.config.mjs` para redirecionar chamadas `/api/**` para o backend local.

## Funcionalidades

- CRUD de processos (criar, listar, editar, excluir)
- Gestão de partes (nome e tipo), vinculação/desvinculação a processos
- Histórico de andamentos por processo, exibido em ordem cronológica (mais recente primeiro)
- Gestão de status de processo (cadastrável dinamicamente)
- Filtro de busca na listagem de processos
