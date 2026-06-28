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
- Header: gray bg, 13px, 600 weight
- Rows: 14px, hover bg #F9FAFB
- Empty state: centered icon + message
- Loading: skeleton rows

### Modals
- 16px radius, shadow-lg
- Header: 20px padding, 18px title
- Body: 24px padding
- Backdrop: rgba(0,0,0,0.4) + blur(4px)

### Badges
- Pill shape, 12px, 600 weight
- Tinted background + colored text (not full-color)

---

## Animation Rules

- Max duration: 250ms
- Easing: cubic-bezier(0.4, 0, 0.2, 1)
- Hover: translateY(-1px) on clickable elements only
- Page entrance: 200ms opacity fade only
- No gradients, no spring easing

---

## Icon Library

Bootstrap Icons (`bi-*`) — already installed across all components.

---

## Build Order

1. variables.css — full token replacement
2. Global styles (body, typography, Google Fonts, scrollbar)
3. Sidebar / Navbar
4. Buttons
5. Form inputs (text, select, textarea, checkbox)
6. Cards
7. Tables (hover, empty state, skeleton)
8. Modals
9. Badges and status tags
10. Pages — one at a time
