# Vibe Coding to Production Template

Tech stack:

- VS Code
- C# Minimal API
- PostgreSQL
- Chinook sample database
- GitHub Actions + Postgres service container

## Milestones

- M0 repo-template-bootstrap
- M1 api+chinook-postgres-local
- M2 data-access-and-api-basics
- M3 genai-provider-agnostic + structured-summary
- M4 genai-text-to-sql-with-guardrails
- M5 ci-postgres-service-container

## Run locally

```bash
docker compose -f infra/docker-compose.yml up -d
dotnet run --project src.Api


---

## 9) .editorconfig

根目錄 `.editorconfig`

```ini
root = true

[*]
charset = utf-8
end_of_line = lf
insert_final_newline = true
indent_style = space
indent_size = 4

[*.md]
trim_trailing_whitespace = false

M2 verification
- GET /health
- GET /api/artists?limit=5
- GET /api/albums/1
- GET /api/tracks/search?q=love&limit=5