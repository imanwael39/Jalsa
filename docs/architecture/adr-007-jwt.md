# ADR-007: Use JWT for Authentication

## Context

Jalsa requires a stateless authentication mechanism that works with an SPA frontend and ASP.NET Core backend. Users include therapists, patients, and administrators.

## Decision

Use **JSON Web Tokens (JWT)** for authentication. Tokens are stored in `localStorage` and sent as `Authorization: Bearer <token>` headers on every API request.

## Alternatives Considered

1. **Session-based auth (cookies):** Server-side sessions require sticky sessions or a shared session store. More complex to scale horizontally.
2. **OAuth2 / OpenID Connect:** Full identity provider (e.g., IdentityServer, Auth0). Adds significant infrastructure complexity for this project's scope.
3. **API Key:** Simple but not suitable for user-based authentication and lacks expiration/rotation.

## Consequences

### Positive

- Stateless — no server-side session storage needed.
- Scalable — any server can handle any request without session affinity.
- Works well with SPA architecture.
- Self-contained — user identity and claims are in the token payload.
- Easy to implement token refresh flow.

### Negative

- Token revocation requires a blocklist or short expiry times.
- JWT size can grow with many claims (affects request headers).
- `localStorage` is vulnerable to XSS — requires proper CSP headers and input sanitisation.
- Token refreshing requires additional frontend logic.

## Security Measures

1. **Short-lived access tokens** (15 minutes).
2. **Long-lived refresh tokens** (7 days) stored in `localStorage`.
3. **Automatic token refresh** via interceptor (retry on 401).
4. **Logout on 401** — intercept 401 responses and redirect to login.
5. **Role-based claims** in JWT payload for frontend RBAC.
6. **CSP headers** to mitigate XSS risks.
7. **Sanitize all user input** — never render un-sanitised content.
