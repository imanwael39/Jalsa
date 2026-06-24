# ADR-003: Lazy Load All Features

## Context

The Jalsa frontend has 7 feature modules (auth, patients, sessions, exercises, reports, dashboard, chatbot). The team needs to decide whether to eagerly or lazily load these features.

## Decision

Lazy load **all features** using Angular's `loadChildren` in route configuration.

## Alternatives Considered

1. **Eager loading:** All features bundled together. Simpler setup but larger initial bundle.
2. **Selective lazy loading:** Only load some features lazily. Inconsistent and adds decision complexity.

## Consequences

### Positive

- Smaller initial bundle — users only download what they need.
- Faster time-to-interactive (TTI).
- Better Lighthouse performance scores.
- Features can be developed and deployed independently.

### Negative

- Slight delay (network + compile) when navigating to a lazy-loaded feature for the first time.
- Slightly more complex route configuration.
- Need to manage preloading strategy (PreloadAllModules or custom).
