# ADR-001: Use Angular 17 Standalone Components

## Context

The Jalsa frontend needs to choose a component architecture. The options are traditional NgModule-based components or Angular 17's standalone components (introduced as stable in v15, matured in v17).

## Decision

Use **Angular 17 Standalone Components** for all components, directives, and pipes. NgModules will not be used except where absolutely required by third-party libraries.

## Alternatives Considered

1. **NgModule-based components:** Traditional approach with `@NgModule` declarations. Requires more boilerplate and adds cognitive overhead.
2. **Hybrid approach:** Mix standalone and NgModule components. Creates inconsistency and confusion about which pattern to follow.

## Consequences

### Positive

- Reduced boilerplate — no `declarations`, no `NgModule` classes.
- Better tree-shaking — only imported components are bundled.
- Simpler lazy loading — components can be loaded directly in routes.
- Easier testing — no need for `TestBed.configureTestingModule` in many cases.
- Aligns with Angular's future direction.

### Negative

- Requires Angular 15+ (we use 17, so this is fine).
- Some third-party libraries may still require NgModules.
- Team needs to learn standalone patterns.
