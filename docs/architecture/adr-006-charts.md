# ADR-006: Use Chart.js for Dashboard Charts

## Context

Jalsa's dashboard requires data visualisation: trend lines for patient activity, session counts, exercise completion rates, and other metrics.

## Decision

Use **Chart.js** (via `ng2-charts` Angular wrapper) for all dashboard chart visualisations.

## Alternatives Considered

1. **D3.js:** Most powerful and flexible but has a steep learning curve and requires manual DOM manipulation.
2. **Highcharts:** Feature-rich but requires a commercial license for non-open-source use.
3. **ApexCharts:** Modern, interactive, but heavier than Chart.js for the needed chart types.
4. **ECharts (Apache):** Excellent performance for large datasets but larger bundle size.
5. **Custom SVG/Canvas:** Full control but would require building chart rendering from scratch.

## Consequences

### Positive

- Lightweight (~60 KB gzipped).
- Good documentation and community support.
- `ng2-charts` provides Angular-native directive wrappers.
- Supports all required chart types (line, bar, doughnut, area).
- Responsive by default.
- Animations included.

### Negative

- Less interactive than D3.js or ApexCharts for complex drill-downs.
- Customisation beyond built-in options requires manual Chart.js plugin development.
- Not tree-shakeable — importing a chart type brings its features.
