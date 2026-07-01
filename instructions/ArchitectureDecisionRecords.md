
# Architecture Decision Records (Summary)

## Frontend

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

## Backend

- **ADR-011: Use ASP.NET Core 8 Web API** – modern, high-performance backend framework with long-term support.
- **ADR-012: Follow Clean Architecture** – separates Domain, Application, Infrastructure, and API layers for maintainability.
- **ADR-013: Use Entity Framework Core** – ORM for database access with Fluent API configurations and migrations.
- **ADR-014: Use Repository and Service Patterns** – separates data access from business logic.
- **ADR-015: Use FluentValidation** – centralized request validation outside controllers.
- **ADR-016: Use JWT Authentication with Role-Based Authorization** – secure stateless authentication and access control.
- **ADR-017: Use Global Exception Handling Middleware** – consistent error handling and API responses.
- **ADR-018: Use Dependency Injection Throughout** – promotes loose coupling and testability.
- **ADR-019: Use Swagger/OpenAPI** – automatic API documentation and endpoint testing.
- **ADR-020: Use SignalR for Real-time Communication** – supports live chat and real-time notifications.
- **ADR-021: Use Asynchronous Programming** – improve scalability using async/await for I/O operations.
- **ADR-022: Never Place Business Logic in Controllers** – controllers coordinate requests; business logic belongs in the Application layer.