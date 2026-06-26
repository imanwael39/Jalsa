
# Architecture Decision Records (Summary)

- **ADR-001: Use Angular 17 Standalone Components** – avoids NgModule boilerplate, improves tree‑shaking.
- **ADR-002: Use Signals + Services for State Management** – simpler than NgRx, sufficient for current complexity.
- **ADR-003: Lazy Load All Features** – reduces initial bundle size.
- **ADR-004: Use Bootstrap 5 with RTL and Plain CSS** – no preprocessors, consistent and accessible UI, native RTL support.
- **ADR-005: Use SignalR for Real‑time Chat** – integrates well with ASP.NET Core backend.
- **ADR-006: Use Functional Guards and Interceptors** – aligns with Angular 15+ best practices.
- **ADR-007: Use JWT for Authentication** – stateless, works with SPA.
- **ADR-008: Use Plain CSS with logical properties** – for RTL support and maintainability (no preprocessors).
- **ADR-009: Use Chart.js for Dashboards** – lightweight and flexible.
- **ADR-010: Do not use NgRx** – keep state management simple with Signals.