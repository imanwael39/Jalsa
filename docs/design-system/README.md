# Jalsa Design System

## Overview

The Jalsa Design System establishes a consistent, accessible, and RTL-ready visual foundation for the healthcare platform. Built on Bootstrap 5.3 with native RTL support, it provides CSS custom properties, typography, and utility classes for all UI development.

## Quick Start

```html
<!-- Import the design system -->
<link rel="stylesheet" href="styles.css" />
```

## Design Tokens

### Colour Palette

| Token | Hex | Usage |
|---|---|---|
| `--primary` | `#0F172A` | Primary brand, navigation, headers |
| `--primary-light` | `#1E293B` | Hover state for primary elements |
| `--primary-dark` | `#020617` | Active state for primary elements |
| `--secondary` | `#3B82F6` | Accent colour, links, interactive elements |
| `--secondary-light` | `#60A5FA` | Hover state for secondary elements |
| `--secondary-dark` | `#2563EB` | Active state for secondary elements |
| `--success` | `#22C55E` | Success states, completed tasks |
| `--warning` | `#F59E0B` | Warning states, pending items |
| `--danger` | `#EF4444` | Error states, destructive actions |
| `--info` | `#3B82F6` | Informational states |
| `--background` | `#F8FAFC` | Page background |
| `--surface` | `#FFFFFF` | Card/surface background |
| `--text-primary` | `#0F172A` | Primary text colour |
| `--text-secondary` | `#475569` | Secondary text colour |
| `--text-muted` | `#94A3B8` | Muted/hint text |
| `--border-color` | `#E2E8F0` | Borders and dividers |

### Typography

| Token | Value | Usage |
|---|---|---|
| `--font-family-base` | `'Cairo', 'Inter', -apple-system, sans-serif` | Body and heading text |
| `--font-size-base` | `1rem` (16px) | Base body text |
| `--line-height-base` | `1.6` | Body text line height |
| `--h1-size` | `2.5rem` | Page title |
| `--h2-size` | `2rem` | Section title |
| `--h3-size` | `1.75rem` | Subsection title |
| `--h4-size` | `1.5rem` | Card title |
| `--h5-size` | `1.25rem` | Group title |
| `--h6-size` | `1rem` | Small heading |

### Spacing Scale

| Token | Value | Rem |
|---|---|---|
| `--spacer-0` | `0` | 0 |
| `--spacer-1` | `0.25rem` | xs |
| `--spacer-2` | `0.5rem` | sm |
| `--spacer-3` | `1rem` | md (base) |
| `--spacer-4` | `1.5rem` | lg |
| `--spacer-5` | `3rem` | xl |
| `--spacer-6` | `4rem` | 2xl |
| `--spacer-7` | `5rem` | 3xl |
| `--spacer-8` | `6rem` | 4xl |

### Border Radius

| Token | Value | Usage |
|---|---|---|
| `--border-radius-sm` | `0.25rem` | Small elements (badges) |
| `--border-radius` | `0.375rem` | Default (buttons, inputs) |
| `--border-radius-lg` | `0.5rem` | Cards, modals |
| `--border-radius-pill` | `50rem` | Pills, tags |

### Shadows

| Token | Value | Usage |
|---|---|---|
| `--shadow-sm` | Small shadow | Subtle elevation (hover) |
| `--shadow` | Default shadow | Cards, dropdowns |
| `--shadow-md` | Medium shadow | Modals, popovers |
| `--shadow-lg` | Large shadow | Sidebars, drawers |

### Z-Index Scale

| Token | Value | Usage |
|---|---|---|
| `--z-index-dropdown` | `1000` | Dropdown menus |
| `--z-index-sticky` | `1020` | Sticky headers |
| `--z-index-modal` | `1050` | Modal dialogs |
| `--z-index-toast` | `1060` | Toast notifications |
| `--z-index-loader` | `2000` | Full-screen loaders |

## Typography System

### Font Stack

```
'Cairo' (Arabic) → 'Inter' (Latin) → system fallback
```

### Heading Hierarchy

| Level | Size | Weight | Line Height | Usage |
|---|---|---|---|---|
| H1 | 2.5rem (40px) | 700 Bold | 1.2 | Page titles |
| H2 | 2rem (32px) | 700 Bold | 1.3 | Section headings |
| H3 | 1.75rem (28px) | 600 Semibold | 1.3 | Subsection headings |
| H4 | 1.5rem (24px) | 600 Semibold | 1.4 | Card titles |
| H5 | 1.25rem (20px) | 600 Semibold | 1.4 | Group headings |
| H6 | 1rem (16px) | 600 Semibold | 1.4 | Small headings |

### Body Text

- **Regular:** 1rem (16px), weight 400, line height 1.6
- **Small:** 0.875rem (14px), weight 400, line height 1.5
- **Caption:** 0.75rem (12px), weight 400, line height 1.4

### Font Weights

| Class | Weight | Usage |
|---|---|---|
| `fw-light` | 300 | Large display text |
| `fw-normal` | 400 | Body text |
| `fw-medium` | 500 | Emphasised body |
| `fw-semibold` | 600 | Subheadings |
| `fw-bold` | 700 | Headings |

## Utility Classes

### Spacing

- **Margin:** `.m-*`, `.mt-*`, `.mb-*`, `.ms-*`, `.me-*`, `.mx-*`, `.my-*`
- **Padding:** `.p-*`, `.pt-*`, `.pb-*`, `.ps-*`, `.pe-*`, `.px-*`, `.py-*`
- **Values:** 0–8 (e.g., `.p-3` = 1rem padding)

### Flexbox

- **Display:** `.d-flex`, `.d-inline-flex`
- **Direction:** `.flex-row`, `.flex-column`
- **Wrap:** `.flex-wrap`, `.flex-nowrap`
- **Justify:** `.justify-content-start`, `.justify-content-end`, `.justify-content-center`, `.justify-content-between`, `.justify-content-around`, `.justify-content-evenly`
- **Align Items:** `.align-items-start`, `.align-items-end`, `.align-items-center`, `.align-items-baseline`, `.align-items-stretch`
- **Align Self:** `.align-self-start`, `.align-self-end`, `.align-self-center`
- **Grow/Shrink:** `.flex-grow-0`, `.flex-grow-1`, `.flex-shrink-0`, `.flex-shrink-1`
- **Gap:** `.gap-0` to `.gap-5`

### Display

- `.d-block`, `.d-inline`, `.d-inline-block`, `.d-none`

### Position

- `.position-relative`, `.position-absolute`, `.position-fixed`, `.position-sticky`, `.position-static`
- `.top-0`, `.bottom-0`, `.start-0`, `.end-0`

### Overflow

- `.overflow-auto`, `.overflow-hidden`, `.overflow-visible`, `.overflow-scroll`

### Width/Height

- `.w-25`, `.w-50`, `.w-75`, `.w-100`
- `.h-100`

### Border

- `.border`, `.border-top`, `.border-bottom`, `.border-start`, `.border-end`, `.border-0`
- `.border-radius`, `.border-radius-lg`, `.border-radius-sm`, `.border-radius-pill`, `.border-radius-circle`

### Shadow

- `.shadow-sm`, `.shadow`, `.shadow-lg`, `.shadow-none`

### Background

- `.bg-white`, `.bg-transparent`, `.bg-primary`, `.bg-secondary`, `.bg-success`, `.bg-danger`, `.bg-warning`, `.bg-info`, `.bg-light`, `.bg-dark`

### Cursor

- `.cursor-pointer`, `.cursor-default`, `.cursor-not-allowed`

### Text Utilities

- **Colour:** `.text-muted`, `.text-secondary`, `.text-primary`, `.text-success`, `.text-danger`, `.text-warning`
- **Alignment:** `.text-center`, `.text-start`, `.text-end`
- **Weight:** `.fw-light`, `.fw-normal`, `.fw-medium`, `.fw-semibold`, `.fw-bold`
- **Size:** `.fs-1` (2.5rem) to `.fs-8` (0.75rem)
- **Decoration:** `.text-uppercase`, `.text-lowercase`, `.text-capitalize`, `.text-decoration-none`, `.text-decoration-underline`
- **Truncation:** `.text-truncate`

### Accessibility

- `.visually-hidden` — Screen-reader-only text
- `.skip-link` — Skip to main content link (visible on focus)

### Responsive

- `.d-sm-none`, `.d-sm-block` (max-width: 576px)
- `.d-md-none`, `.d-md-block` (min-width: 768px)

## RTL Implementation

### Setup

- `index.html` sets `lang="ar" dir="rtl"` on the root `<html>` element
- Bootstrap RTL (`bootstrap.rtl.min.css`) is imported after Bootstrap core
- All custom utilities use logical CSS properties

### Logical Properties

| Logical Property | LTR | RTL |
|---|---|---|
| `margin-inline-start` | `margin-left` | `margin-right` |
| `margin-inline-end` | `margin-right` | `margin-left` |
| `padding-inline-start` | `padding-left` | `padding-right` |
| `padding-inline-end` | `padding-right` | `padding-left` |
| `border-inline-start` | `border-left` | `border-right` |
| `border-inline-end` | `border-right` | `border-left` |
| `inset-inline-start` | `left` | `right` |
| `inset-inline-end` | `right` | `left` |

### Text Alignment

- `text-start` — Aligns left in LTR, right in RTL
- `text-end` — Aligns right in LTR, left in RTL

### Testing RTL

1. Toggle `dir="rtl"` and `dir="ltr"` on the `<html>` element
2. Verify all margins, paddings, and borders flip correctly
3. Verify text alignment respects the direction
4. Verify icons and images mirror when needed

## Accessibility

### WCAG 2.1 AA Compliance

- **Colour Contrast:** All colour combinations meet ≥ 4.5:1 for normal text, ≥ 3:1 for large text
- **Font Sizes:** Base font size 1rem (16px), minimum 0.75rem (12px)
- **Keyboard Navigation:** All interactive elements are focusable and operable via keyboard
- **Semantic HTML:** Uses `<header>`, `<main>`, `<footer>`, `<nav>`, `<button>`, etc.
- **ARIA Attributes:** Added where necessary for screen reader support
- **Touch Targets:** Minimum 44×44px on mobile devices

### Accessibility Classes

- `.visually-hidden` — Content hidden visually but accessible to screen readers
- `.skip-link` — Skip to main content link that appears on focus

## File Structure

```
frontend/src/
├── styles/
│   ├── variables.css      # CSS custom properties (tokens)
│   ├── typography.css     # Heading and text styles
│   └── utilities.css      # Utility classes
└── styles.css             # Import orchestrator
```

## Import Order

```css
/* 1. Custom variables (must be first) */
@import './styles/variables.css';

/* 2. Bootstrap with native RTL support */
@import 'bootstrap/dist/css/bootstrap.rtl.min.css';

/* 3. Custom typography */
@import './styles/typography.css';

/* 4. Custom utilities */
@import './styles/utilities.css';
```

## Usage Examples

### Basic Card

```html
<div class="card border shadow">
  <div class="p-4">
    <h3 class="h3 fw-bold">Card Title</h3>
    <p class="text-secondary">Card description text.</p>
  </div>
</div>
```

### Flex Layout

```html
<div class="d-flex gap-3 align-items-center">
  <div class="flex-shrink-0">
    <img src="avatar.png" alt="User avatar" />
  </div>
  <div class="flex-grow-1">
    <h4 class="h4">User Name</h4>
    <p class="text-muted">User role</p>
  </div>
</div>
```

### RTL-Aware Spacing

```html
<div class="p-4">
  <button class="ms-2">Start margin (flips in RTL)</button>
  <button class="me-2">End margin (flips in RTL)</button>
</div>
```

## Documentation

- [Design Tokens](./tokens.md) — Complete token reference
- [Typography](./typography.md) — Font families, heading hierarchy, weights
- [Components](./components.md) — Component styling guidelines

## Maintenance

- All values must use CSS custom properties from `variables.css`
- Never hardcode colours, spacing, or typography values
- Use utility classes before writing custom CSS
- Test all components in both RTL and LTR modes
- Maintain WCAG 2.1 AA compliance for all new components

---

**Last Updated:** Phase 3 — RTL & Design System
**Status:** Complete (FE-RTL-001 to FE-RTL-005)
