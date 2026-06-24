# Design Tokens

## Colour Palette

| Token | Value | Usage |
|---|---|---|
| `--primary` | `#0F172A` | Primary brand colour, navigation, headers |
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

### Accessibility Compliance

All colour combinations meet WCAG 2.1 AA contrast ratios (≥ 4.5:1 for normal text, ≥ 3:1 for large text).

## Typography

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

## Spacing Scale

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

## Border Radius

| Token | Value | Usage |
|---|---|---|
| `--border-radius-sm` | `0.25rem` | Small elements (badges) |
| `--border-radius` | `0.375rem` | Default (buttons, inputs) |
| `--border-radius-lg` | `0.5rem` | Cards, modals |
| `--border-radius-pill` | `50rem` | Pills, tags |

## Shadows

| Token | Value | Usage |
|---|---|---|
| `--shadow-sm` | Small shadow | Subtle elevation (hover) |
| `--shadow` | Default shadow | Cards, dropdowns |
| `--shadow-md` | Medium shadow | Modals, popovers |
| `--shadow-lg` | Large shadow | Sidebars, drawers |

## Z-Index Scale

| Token | Value | Usage |
|---|---|---|
| `--z-index-dropdown` | `1000` | Dropdown menus |
| `--z-index-sticky` | `1020` | Sticky headers |
| `--z-index-modal` | `1050` | Modal dialogs |
| `--z-index-toast` | `1060` | Toast notifications |
| `--z-index-loader` | `2000` | Full-screen loaders |
