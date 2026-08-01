# Krisma: memoria corta

- Producto: analítica de ingeniería para equipos. Ingiere GitHub; presenta tendencias de equipo y contexto individual. No usar métricas como ranking automático ni decisión laboral única.
- Estado: `Krisma.Domain` (`net10.0`) solo. Sin API, Infrastructure, DB, tests ni repo Git.
- Arquitectura objetivo: Domain + Application + Infrastructure + API. Domain no depende de EF/GitHub/HTTP.
- Fuente GitHub: GitHub App, webhooks (`push`, `pull_request`, `pull_request_review`) + sincronización incremental idempotente. Nunca PAT personal en producción.
- Leer `docs/ARCHITECTURE.md` antes de añadir entidades, integraciones o cálculos. Mantenerlo actualizado al cambiar decisiones, modelo o estado.
- Convenciones: UTC/`DateTimeOffset`; IDs internos `Guid`; IDs externos GitHub `long` + `node_id`; DTOs separados de entidades; `CancellationToken`; `IHttpClientFactory`; validación en Application; tests para reglas del dominio.
