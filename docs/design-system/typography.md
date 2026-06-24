# Typography System

## Font Stack

The application uses a dual-font strategy to support Arabic (primary) and Latin text:

```
'Cairo' (Arabic) → 'Inter' (Latin) → system fallback
```

### Cairo
- Designed for Arabic and Latin scripts
- Available in weights 200–900
- Used as the primary font for Arabic text
- Rendered as: **جلسة العلاج** (therapy session)

### Inter
- Designed for high legibility on screens
- Used as the primary font for Latin text
- Available in weights 100–900
- Rendered as: **Patient Dashboard**

## Heading Hierarchy

| Level | Size | Weight | Line Height | Usage |
|---|---|---|---|---|
| H1 | 2.5rem (40px) | 700 Bold | 1.2 | Page titles |
| H2 | 2rem (32px) | 700 Bold | 1.3 | Section headings |
| H3 | 1.75rem (28px) | 600 Semibold | 1.3 | Subsection headings |
| H4 | 1.5rem (24px) | 600 Semibold | 1.4 | Card titles |
| H5 | 1.25rem (20px) | 600 Semibold | 1.4 | Group headings |
| H6 | 1rem (16px) | 600 Semibold | 1.4 | Small headings |

## Body Text

- **Regular:** 1rem (16px), weight 400, line height 1.6
- **Small:** 0.875rem (14px), weight 400, line height 1.5
- **Caption:** 0.75rem (12px), weight 400, line height 1.4

## Font Weights

| Class | Weight | Usage |
|---|---|---|
| `fw-light` | 300 | Large display text |
| `fw-normal` | 400 | Body text |
| `fw-medium` | 500 | Emphasised body |
| `fw-semibold` | 600 | Subheadings |
| `fw-bold` | 700 | Headings |

## RTL Considerations

- Arabic text reads right-to-left. All text-align utilities use logical properties.
- `text-start` aligns left in LTR, right in RTL.
- `text-end` aligns right in LTR, left in RTL.
- Line height for Arabic may need adjustment — Arabic text is typically taller than Latin.
- Font weight 900 (Black) renders well for Arabic headings due to its calligraphic nature.

## Implementation

Typography is defined in `src/styles/typography.css` using CSS custom properties. Apply heading styles either with semantic tags (`<h1>`–`<h6>`) or utility classes (`.h1`–`.h6`).
