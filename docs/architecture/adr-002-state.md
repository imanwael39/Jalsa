# ADR-002: Use Signals + Services for State Management

## Context

Jalsa requires a state management strategy. The options range from simple component-level state to full NgRx with Actions, Reducers, and Effects.

## Decision

Use **Angular Signals + Services** for state management. Do not use NgRx.

## Alternatives Considered

1. **NgRx:** Full Redux pattern with store, actions, reducers, effects. Powerful but adds significant boilerplate and complexity.
2. **RxJS BehaviorSubjects:** Traditional Angular service pattern. Works but harder to compose and lacks fine-grained reactivity.
3. **Signals only (no services):** State lives in components. Works for local state but doesn't scale to shared state.

## Consequences

### Positive

- Simpler than NgRx — less boilerplate, easier to understand.
- Signals provide fine-grained reactivity and automatic change detection.
- Services provide a natural home for business logic and API calls.
- No external dependencies beyond Angular core.
- Easy to migrate to NgRx later if complexity grows.

### Negative

- No DevTools for state debugging (unlike NgRx).
- No strict unidirectional data flow enforcement.
- Team must self-discipline to avoid anti-patterns.

## Migration Path

If the project grows significantly and cross-cutting state concerns become unmanageable, NgRx can be introduced incrementally by replacing individual services with store slices.
