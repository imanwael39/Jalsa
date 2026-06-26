# ADR-004: Use Bootstrap 5 with RTL and Plain CSS

## Context

Jalsa requires a styling approach that supports Arabic (RTL) layout, is maintainable across a large team, and meets WCAG 2.1 AA accessibility standards.

## Decision

Use **Bootstrap 5 with RTL support** and **plain CSS** (no preprocessors). Design tokens are defined as CSS variables in `styles.css`.

## Alternatives Considered

1. **SCSS/Sass with Bootstrap:** More powerful (mixins, functions, nesting) but adds a build step and deviates from the project's constraint to use plain CSS.
2. **Tailwind CSS:** Utility-first approach already partially present in the project; however, Bootstrap provides more consistent component-level styling for a healthcare platform.
3. **Custom CSS only:** Full control but would require rebuilding a design system from scratch.
4. **Material Design (Angular CDK):** Comprehensive component library but heavier and more opinionated.

## Consequences

### Positive

- Bootstrap RTL provides native Arabic layout support (`dir="rtl"`).
- Plain CSS means no build preprocessing — simpler toolchain.
- CSS variables allow runtime theme switching if needed.
- Bootstrap's accessibility baseline meets WCAG 2.1 AA.
- Logical properties (`margin-inline-start`, `padding-inline-end`) ensure RTL correctness.

### Negative

- No nesting, mixins, or functions (workarounds needed for complex styles).
- Bootstrap is heavier than a utility-only framework if unused styles are not purged.
- Team needs to know Bootstrap 5's RTL API.
