# Jalsa — UI Redesign Plan

## Design Philosophy

Jalsa is a **clinical workspace**, not a dashboard. Every design decision reduces cognitive load. The therapist opens this app between sessions — sometimes emotionally heavy ones. The UI should feel like a clean desk in a quiet office: everything in its place, nothing shouting.

---

## Color Palette

```css
--color-bg:           #F4F5F7    /* Cool gray background */
--color-surface:      #FFFFFF    /* White cards/panels */
--color-primary:      #2B6B5E    /* Deep sage teal */
--color-primary-hover: #1F524A   /* Darker on interaction */
--color-primary-light: #E8F0EE  /* Tinted background for selected states */
--color-text:         #1A1D23    /* Near-black with warmth */
--color-text-muted:   #6B7280    /* Secondary info, timestamps */
--color-border:       #E2E5EA    /* Visible but quiet */
--color-accent:       #4A9E8E    /* Lighter teal for highlights */
--color-success:      #16A34A
--color-error:        #DC2626
--color-warning:      #D97706
```

### Sidebar

```css
--color-sidebar-bg:           #1A2332    /* Dark slate, NOT green */
--color-sidebar-text:         #94A3B8
--color-sidebar-active-text:  #FFFFFF
--color-sidebar-active-border: #2B6B5E  /* 3px teal accent bar */
--color-sidebar-active-bg:    rgba(43, 107, 94, 0.08)
--color-sidebar-hover:        rgba(255, 255, 255, 0.04)
```

---

## Typography

- **Headings**: Cairo — weight 600/700
- **Body**: IBM Plex Sans Arabic — weight 400/500
- **Fallback**: Tajawal (last resort only)

### Font Size Scale

```css
--text-xs:    0.75rem    /* 12px — badges, captions */
--text-sm:    0.8125rem  /* 13px — help text, metadata */
--text-base:  0.875rem   /* 14px — body, inputs, cells */
--text-md:    0.9375rem  /* 15px — nav items */
--text-lg:    1.125rem   /* 18px — section headings */
--text-xl:    1.375rem   /* 22px — page titles */
--text-2xl:   1.75rem    /* 28px — dashboard welcome */
```

---

## Spacing (4px base)

```css
--space-1:   0.25rem    /* 4px */
--space-2:   0.5rem     /* 8px */
--space-3:   0.75rem    /* 12px */
--space-4:   1rem       /* 16px */
--space-5:   1.25rem    /* 20px */
--space-6:   1.5rem     /* 24px */
--space-7:   2rem       /* 32px */
--space-8:   3rem       /* 48px */
--space-9:   4rem       /* 64px */
```

---

## Border Radius

```css
--radius-sm:    0.5rem     /* 8px — inputs, buttons */
--radius-md:    0.75rem    /* 12px — cards, dropdowns */
--radius-lg:    1rem       /* 16px — modals */
--radius-full:  9999px     /* pills, avatars */
```

---

## Shadows (3 levels only)

```css
--shadow-sm:  0 1px 3px rgba(0,0,0,0.04), 0 1px 2px rgba(0,0,0,0.02)
--shadow-md:  0 4px 12px rgba(0,0,0,0.06), 0 2px 4px rgba(0,0,0,0.03)
--shadow-lg:  0 8px 24px rgba(0,0,0,0.08), 0 4px 8px rgba(0,0,0,0.04)
```

Max 2 shadow levels on any single page.

---

## Components

### Buttons
- Radius: 8px all sizes
- Primary: bg `--color-primary`, hover darker + translateY(-1px)
- Secondary: transparent bg, 1.5px border, hover bg gray
- Ghost: transparent, no border, hover bg gray
- Danger: bg `--color-error`
- Focus: 2px solid teal, offset 2px
- Sizes: sm (32px), md (40px), lg (48px)

### Form Inputs
- Label: above, 14px, 500 weight, 6px gap
- Input: 40px height, 14px text, 8px radius, 1px border
- Focus: teal border + 3px ring
- Error: red border, message below (12px, red, with icon)

### Cards
- 12px radius, 1px border, white bg
- Padding: 24px
- No shadow at rest (border only)
- Hover (clickable): shadow-md + translateY(-1px)

### Tables
- Row height: min 48px
- Header: gray bg, 12px, 600 weight, no uppercase
- Rows: 14px, hover bg #F9FAFB
- Header border: 1px (not 2px)
- Empty state: centered icon + message
- Loading: skeleton rows

### Modals
- 16px radius, shadow-lg, 1px border
- Header: white bg, heading font (Cairo), 18px title
- Body: 24px padding
- Backdrop: rgba(0,0,0,0.35) + blur(4px)
- Animation: 200ms ease-out, scale(0.97) — no bounce, no translateY

### Badges / Status Tags
- Pill shape (radius-full), 12px, 600 weight
- Tinted background + colored text (NOT solid-color)
- Statuses: success (green tint), warning (amber tint), danger (red tint), info (blue tint), neutral (gray tint)
- Padding: 2px 8px
- No border, no shadow

### Toast / Notifications
- 12px radius, shadow-md, 3px left accent border
- Auto-dismiss: 4s default
- Position: top-center

---

## Animation Rules

- Max duration: 250ms
- Easing: cubic-bezier(0.4, 0, 0.2, 1)
- Hover: translateY(-1px) on clickable elements only
- Page entrance: 200ms opacity fade only — no translateY
- Modal entrance: 200ms scale(0.97) — no bounce
- No gradients, no spring easing

---

## Icon Library

Bootstrap Icons (`bi-*`) — already installed across all components.

---

## Build Order

| # | Step | Branch | Status |
|---|------|--------|--------|
| 1 | variables.css — full token replacement | `2026-06-27_Mustafa_DesignTokens` | Done |
| 2 | Global styles (typography, body, Google Fonts) | `2026-06-28_Mustafa_GlobalStyles` | Done |
| 3 | Sidebar / Navbar | `2026-06-28_Mustafa_SidebarRedesign` | Done |
| 4 | Buttons (primary/secondary/ghost/danger) | `2026-06-28_Mustafa_ButtonsRedesign` | Done |
| 5 | Form inputs (input, select, textarea, checkbox, radio) | `2026-06-28_Mustafa_FormInputsRedesign` | Done |
| 6 | Cards (stats-card) | `2026-06-28_Mustafa_CardsRedesign` | Done |
| 7 | Tables (hover, empty state) | `2026-06-28_Mustafa_TablesRedesign` | Done |
| 8 | Modals | `2026-06-28_Mustafa_ModalsRedesign` | Done |
| 9 | Badges, status tags, toast | — | Pending |
| 10 | Pages — auth, dashboard, patients, sessions, exercises, reports | — | Pending |

---

## Page-Level Redesign (Step 10)

Each page gets its own branch. Order by most-used:

1. **Dashboard** — stats cards, charts, welcome header
2. **Patient List** — table, search, filters, empty state
3. **Patient Detail** — tabs, intake, assessments
4. **Patient Form** — create/edit form
5. **Session List** — table, filters
6. **Session Form / Detail** — form, notes editor
7. **Exercise List** — cards/table, assign flow
8. **Report List / Generate / Detail** — AI report flow
9. **Auth pages** — login, register, forgot/reset password, profile
