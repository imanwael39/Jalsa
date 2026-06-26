# ADR-005: Use SignalR for Real-time Communication

## Context

Jalsa requires real-time communication between therapists and patients (chat feature). The backend uses ASP.NET Core, which has native WebSocket support.

## Decision

Use **SignalR** (Microsoft's real-time web framework) for all real-time features, including chat, crisis alerts, and live updates.

## Alternatives Considered

1. **WebSocket (raw):** More control but requires implementing protocol negotiation, reconnection, and message serialisation from scratch.
2. **Socket.IO:** Popular Node.js library but requires a Node.js server — not compatible with ASP.NET Core backend.
3. **Long polling / SSE:** Simpler but less efficient for bidirectional communication.
4. **Firebase Realtime Database:** External dependency, adds latency, and couples us to Firebase infrastructure.
5. **Polling (periodic HTTP requests):** Simplest to implement but wasteful and slower.

## Consequences

### Positive

- Native ASP.NET Core integration — same tech stack as backend.
- Automatic transport fallback (WebSocket → Server-Sent Events → Long Polling).
- Built-in reconnection with exponential backoff.
- Strong typing with TypeScript client (`@microsoft/signalr`).
- Supports groups for therapist-specific channels.
- Scalable with Azure SignalR Service if needed.

### Negative

- Requires installing `@microsoft/signalr` npm package.
- WebSocket connections may be blocked by some firewalls/proxies.
- Connection stability in low-bandwidth environments may need tuning.
- Load balancing requires sticky sessions or a backplane (Redis).
