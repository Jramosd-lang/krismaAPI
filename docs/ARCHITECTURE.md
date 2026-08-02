# Krisma: arquitectura y mapa de trabajo

## Propósito y límite ético

Krisma analiza flujo de entrega desde GitHub. Sirve para conversación, capacidad y mejora del proceso. No produce un ranking de “mejores desarrolladores” ni una decisión automática de RR. HH.: número de commits, líneas y tiempo de PR son proxies manipulables y dependen de rol, guardias, complejidad, revisiones y trabajo no visible.

Mostrar rangos, periodo, tamaño de muestra y contexto. Restringir acceso, retención y exportación; auditar consultas. Requerir revisión humana y permitir corregir identidad/atribución.

## Estado actual (2026-07-30)

- Solución `net10.0`: Domain, Application, Infrastructure, Api y Tests.
- Flujo inicial: `POST /api/developers` usa MediatR, `ValidationBehavior`, FluentValidation, `Result` y ProblemDetails.
- Infrastructure usa `InMemoryDeveloperRepository` solo como adaptador temporal; no es persistencia de producción.
- Sin DB, migraciones, auth ni GitHub. Build y tests pasan.

## Problemas detectados

| Prioridad | Hallazgo | Riesgo / acción |
|---|---|---|
| P0 | Métrica individual como ranking | Incentivos perversos y daño laboral. Diseñar métricas de flujo/equipo con contexto. |
| P0 | Sin tests ni capas de aplicación/integración | No hay evidencia de reglas ni base para crecer. Crear estructura antes de endpoints. |
| P1 | `Developer.Technologies` es campo público, no propiedad | EF Core no la trata como navegación convencional; encapsular colección y configurar relación. |
| P1 | `Email` existe pero `Developer` persiste `string` | Regla de email no queda en modelo. Usar value object con conversión EF. |
| P1 | Entidades sin comportamiento ni configuración | Estado inválido y esquema implícito. Añadir métodos, constructores EF privados y `IEntityTypeConfiguration`. |
| P2 | `Class1.cs`, usings sobrantes, `default!` en value types | Ruido; eliminar al estructurar. |
| P2 | Excepciones para validación de entrada | En Application usar FluentValidation; Domain devuelve `Result` para reglas de negocio esperables. |

## Modelo inicial recomendado

```text
Organization 1--* Team 1--* TeamMembership *--1 Developer
Organization 1--* Repository *--1 Project
Repository 1--* Commit
Repository 1--* PullRequest 1--* PullRequestReview
Developer 1--* Commit
Developer 1--* PullRequest (author)
Developer *--* Technology (DeveloperTechnology)
```

Entidades mínimas:

- `Organization`: identidad GitHub, nombre, zona horaria; delimita los datos de developers, equipos y proyectos.
- `Developer`: pertenece a una organización; conservar `GitHubUserId` y `GitHubNodeId` como identidad externa estable y tratar `GitHubLogin` como dato actualizable.
- `Team`: organización, nombre. `TeamMembership`: desarrollador, equipo, rol, rango de fechas.
- `Project`: unidad de negocio; relaciona repositorios. No confundir con repositorio GitHub.
- `Repository`: `GitHubRepositoryId`, `NodeId`, owner/name, estado de sincronización.
- `Commit`: SHA único por repositorio, autor GitHub y Git, `AuthoredAt`/`CommittedAt` UTC, additions/deletions solo como datos de contexto.
- `PullRequest`: número único por repositorio, autor, estado, `CreatedAt`, `MergedAt`, `ClosedAt`, `FirstReviewAt`, `MergedBy`.
- `PullRequestReview`: PR, revisor, estado, enviada en UTC. Conservar revisiones individuales; calcular la primera aprobación por consulta.

Índices: IDs GitHub únicos por organización/repositorio, `(RepositoryId, Sha)`, `(RepositoryId, Number)`, fechas de PR/commit y claves de pertenencia al equipo. Guardar `GitHubNodeId` y el ID numérico; no usar login como identidad estable.

## Arquitectura objetivo

```text
Krisma.Domain          reglas, entidades, value objects, eventos
Krisma.Application     casos de uso, DTOs, FluentValidation, interfaces
Krisma.Infrastructure  EF Core, PostgreSQL, GitHub client/webhooks, migraciones
Krisma.Api             HTTP, auth, OpenAPI, rate limiting, health checks
Krisma.Tests           unitarios Domain/Application + integración API/DB
```

Inicio recomendado: PostgreSQL + EF Core. No añadir repositorio genérico, CQRS/MediatR, Redis ni Dapper hasta que un caso medido lo justifique.

## GitHub

Producción: GitHub App instalada por organización, permisos mínimos y secretos fuera del código. Ingerir webhooks firmados; verificar firma antes de procesar. Procesamiento asíncrono, idempotente y con registro de entrega. Añadir sincronización incremental para recuperar eventos perdidos, paginación y reintentos con backoff.

Eventos iniciales: `push`, `pull_request`, `pull_request_review`. REST permite completar/backfill; GraphQL solo si reduce llamadas para consultas agregadas. Persistir payload bruto de forma temporal solo si hay política de retención y protección de PII.

## Métricas v1 útiles y defendibles

- Tiempo hasta primera revisión: `FirstReviewAt - CreatedAt`.
- Tiempo de ciclo PR: `MergedAt - CreatedAt`.
- Tamaño de PR: archivos/cambios como indicador, nunca calidad.
- PRs abiertas y edad, por repositorio/equipo.
- Throughput de PRs fusionadas, siempre periodo y equipo.

Excluir o etiquetar bots, commits de merge, autores no vinculados, vacaciones y periodos con muestra insuficiente. No convertir estas métricas a un único score.

## Próximo orden de implementación

1. Infrastructure: EF Core/PostgreSQL, configuraciones y migración inicial; reemplazar repositorio en memoria.
2. Application: más comandos/consultas, contratos de GitHub/reloj/usuario.
3. API: auth/RBAC, rate limit, logs estructurados.
6. GitHub App + webhook seguro + worker idempotente + backfill.
7. Dashboard de métricas contextualizadas; pruebas de integración y observabilidad.

## Cómo mantener esta memoria

Actualizar este archivo al cambiar modelo, decisiones, dependencias o estado. `AGENTS.md` debe quedar breve; contiene solo ruta de carga. Para ahorrar contexto, leer secciones concretas de este documento (`rg -n "## ..." docs/ARCHITECTURE.md`) en vez de cargarlo entero.
