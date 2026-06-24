# Component Styling Guidelines

## Principles

1. **Use design tokens** — All colours, spacing, and typography must use CSS custom properties from `variables.css`. Never hardcode values.
2. **Prefer utility classes** — Use `d-flex`, `p-3`, `ms-2` etc. from `utilities.css` before writing custom CSS.
3. **Scoped styles** — Each component has its own `.css` file. Styles are scoped by Angular's ViewEncapsulation.
4. **RTL-first** — Use logical properties (`margin-inline-start`, `padding-inline-end`) for all spacing. Avoid `left`/`right` directly.
5. **Responsive** — Design for mobile-first. Use Tailwind breakpoints (`sm:`, `md:`, `lg:`, `xl:`).

## Component Structure

Each component folder contains:
```
component-name/
├── component-name.component.ts
├── component-name.component.html
├── component-name.component.css
└── index.ts            (optional barrel export)
```

## RTL Guidelines

- Use `margin-inline-start` / `margin-inline-end` instead of `margin-left` / `margin-right`.
- Use `padding-inline-start` / `padding-inline-end` instead of `padding-left` / `padding-right`.
- Use `border-inline-start` / `border-inline-end` instead of `border-left` / `border-right`.
- For text alignment, use `text-start` / `text-end` classes.
- Test every component in both `dir="ltr"` and `dir="rtl"`.

## Component Patterns

### Buttons
- Variants: `primary`, `secondary`, `outline`, `ghost`, `danger`
- Sizes: `sm`, `md`, `lg`
- States: default, hover, active, disabled, loading
- Include `aria-label` and support keyboard navigation

### Form Inputs
- Variants: default, filled, outlined
- States: default, focused, error, disabled, readonly
- Always include `<label>` with `for` attribute
- Show validation messages inline below the input

### Cards
- Surface colour with default shadow
- Optional header, body, footer sections
- Responsive padding (smaller on mobile)

### Modals
- Centred overlay with backdrop
- Close on Escape key and backdrop click
- Header with title, body with content, footer with actions

### Tables
- Striped rows for readability
- Responsive horizontal scroll on mobile
- Sortable column headers
- Loading skeleton state

## Accessibility

- All interactive elements must be keyboard accessible.
- Use semantic HTML (`<button>`, `<nav>`, `<main>`, `<header>`).
- Include `aria-label` on icon-only buttons.
- Maintain minimum touch target of 44×44px on mobile.
- Error messages must be associated with inputs via `aria-describedby`.
