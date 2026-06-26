
🖌️ Jalsa – Phase 3: RTL & Design System Expanded Implementation Handbook · 5 Tasks · 25+ Subtasks
==================================================================================================

🖌️ Phase 3 – RTL & Design System
---------------------------------

**Purpose:** Establish the visual foundation of the Jalsa platform. This phase configures Bootstrap with full RTL support, defines the design system (colours, typography, spacing, shadows), and creates utility classes. The result is a consistent, brand‑aligned, and accessible UI that adapts to both Arabic (RTL) and English (LTR) contexts.

📋 Tasks: 5 ⏱️ Total Effort: ~16 hours 👤 Owners: M3 (Frontend Lead), M4 (Frontend Developer) 🔗 Dependencies: Phase 1 (Foundation Setup) must be complete 🎯 Deliverable: Complete RTL‑ready design system with css variables, typography, utilities, and validated RTL layout

## FE-RTL-001Setup Bootstrap RTL and Base Styles P0 High 4h ▾

### Task Information

*   **Task ID:** FE-RTL-001
*   **Task Name:** Setup Bootstrap RTL and Base Styles
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SETUP-001, FE-SETUP-003 (Bootstrap installed)
*   **Complexity:** High
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Configure Bootstrap with full RTL (Right‑to‑Left) support, import custom css variables, and set up the base global styles for the application. Ensure that the UI correctly mirrors for Arabic text and that all Bootstrap components work seamlessly in RTL mode.

### Business Purpose

Jalsa serves Arabic‑speaking therapists and patients. A fully functional RTL layout is essential for usability and cultural appropriateness.

### Technical Purpose

Use `bootstrap-rtl` package (or Bootstrap's built‑in RTL support) to enable RTL. Override Bootstrap variables with brand colors and fonts. Create the main `styles.css` file that imports everything correctly.

### Prerequisites

*   Bootstrap and Bootstrap RTL installed (from FE-SETUP-003).
*   css preprocessor configured in Angular.
*   Basic knowledge of css and RTL concepts.

### Dependencies

*   **FE-SETUP-001:** Angular project exists.
*   **FE-SETUP-003:** Bootstrap packages installed.

### Inputs

*   Brand colours and font preferences from Phase 0.

### Outputs

*   `src/styles/_variables.css` (Bootstrap overrides and custom variables).
*   `src/styles.css` updated to import Bootstrap, Bootstrap RTL, and custom styles.
*   RTL layout working in the application.

### Detailed Workflow

#### Step 1: Create `_variables.css`

**What to do:** Create a css file to override Bootstrap variables and define custom design tokens.

**Why it is required:** Centralising variables ensures consistency and makes theming easier.


```css
// src/styles/_variables.css

// ================================================================
// 1. Bootstrap Overrides
// ================================================================

// Enable RTL (Bootstrap 5.3+ supports native RTL via $enable-rtl)
$enable-rtl: true;

// Fonts
$font-family-base: 'Cairo', 'Inter', -apple-system, sans-serif;
$font-size-base: 1rem; // 16px
$line-height-base: 1.6;

// Colours
$primary: #0F172A;
$primary-light: #1E293B;
$primary-dark: #020617;

$secondary: #3B82F6;
$secondary-light: #60A5FA;
$secondary-dark: #2563EB;

$success: #22C55E;
$warning: #F59E0B;
$danger: #EF4444;
$info: #3B82F6;

$body-bg: #F8FAFC;
$body-color: #0F172A;

$border-color: #E2E8F0;

// Typography
$headings-font-weight: 700;
$headings-line-height: 1.2;

// Spacing
$spacer: 1rem;
$spacers: (
    0: 0,
    1: $spacer * 0.25,
    2: $spacer * 0.5,
    3: $spacer,
    4: $spacer * 1.5,
    5: $spacer * 3,
    6: $spacer * 4,
    7: $spacer * 5,
    8: $spacer * 6,
);

// Border radius
$border-radius: 0.375rem;
$border-radius-lg: 0.5rem;
$border-radius-sm: 0.25rem;
$border-radius-pill: 50rem;

// Shadows
$box-shadow-sm: 0 1px 2px rgba(0, 0, 0, 0.05);
$box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.06);
$box-shadow-lg: 0 10px 15px rgba(0, 0, 0, 0.1), 0 4px 6px rgba(0, 0, 0, 0.05);

// Z‑index
$zindex-dropdown: 1000;
$zindex-sticky: 1020;
$zindex-modal: 1050;
$zindex-toast: 1060;
$zindex-loader: 2000;

// ================================================================
// 2. Custom Design Tokens (beyond Bootstrap)
// ================================================================

// Additional colours
$bg-subtle: #F1F5F9;
$text-muted: #94A3B8;
$text-secondary: #475569;

// Custom breakpoints (optional)
$custom-breakpoints: (
    xs: 0,
    sm: 576px,
    md: 768px,
    lg: 992px,
    xl: 1200px,
    xxl: 1400px,
);

#### Step 2: Create `styles.css`

**What to do:** Import Bootstrap, Bootstrap RTL, and the variables file in the correct order.

**Why it is required:** This is the entry point for all global styles.


```css
// src/styles.css

// 1. Custom variables (must be first)
@import 'styles/variables';

// 2. Bootstrap core
@import 'bootstrap/css/bootstrap';

// 3. Bootstrap RTL (overrides)
@import 'bootstrap-rtl/css/bootstrap-rtl';

// 4. Custom typography (will be added in next task)
@import 'styles/typography';

// 5. Custom utilities (will be added later)
@import 'styles/utilities';

// 6. Global app styles
// (can add here or keep separate)
```
#### Step 3: Test RTL

**What to do:** Add a simple Arabic text and a Bootstrap component to verify RTL rendering.

**Why it is required:** Confirm that RTL is working correctly.

In `app.component.html`:

```html
<div dir="rtl">
    <h1>مرحباً بكم في جلسة</h1>
    <button class="btn btn-primary">زر تجريبي</button>
</div>
```
Run `ng serve` and check that the text aligns to the right and the button is mirrored correctly.

### Files To Create

*   `src/styles/_variables.css`
*   Modify `src/styles.css`

### CLI Commands


```bash
# Create styles folder and variables file
mkdir -p src/styles
touch src/styles/_variables.css
```
### Concepts Required

*   css variables and imports
*   Bootstrap 5 theming
*   RTL (Right‑to‑Left) layout basics

### Tools/Libraries Required

*   Bootstrap 5
*   bootstrap-rtl (or built‑in RTL support)

### Angular Concepts Required

*   Global styles in `angular.json` (already configured)

### UI/UX Requirements

*   All UI components must mirror correctly in RTL.
*   Arabic text should be legible and properly spaced.
*   Colours must meet accessibility contrast ratios.

### Testing Steps

1.  Run `ng serve` and verify that the RTL direction is applied.
2.  Test a few Bootstrap components (button, navbar, card) in RTL.
3.  Check that the layout mirrors properly.

### Expected Deliverables

*   RTL support configured.
*   Base variables defined.

### Common Mistakes

**Mistake 1:** Importing Bootstrap RTL before Bootstrap (order matters).  
**Fix:** Import Bootstrap first, then Bootstrap RTL.

**Mistake 2:** Not setting `$enable-rtl: true`.  
**Fix:** Include the variable before importing Bootstrap.

### Edge Cases

*   If using mixed LTR/RTL content, ensure the `dir` attribute is set correctly.

### Definition of Done

*   RTL works correctly in the application.
*   Variables file exists and is imported.

### Frontend Architecture Notes

*   We use Bootstrap's native RTL support; no external hacks.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Always test UI in RTL mode.

## FE-RTL-002Define Typography System P0 Medium 3h ▾

### Task Information

*   **Task ID:** FE-RTL-002
*   **Task Name:** Define Typography System
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-RTL-001
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** Critical

### Objective

Define a complete typography system including font families, heading sizes, body text, and utility classes for text styling. Ensure the system works for both Arabic (RTL) and English (LTR) text.

### Business Purpose

Consistent and accessible typography improves readability and brand perception.

### Technical Purpose

Create css classes for headings, body text, and text utilities that can be used throughout the application.

### Prerequisites

*   Understanding of typography principles (font sizes, line heights, weights).
*   Knowledge of css.

### Dependencies

*   **FE-RTL-001:** Variables file exists.

### Inputs

*   Font families and sizes from design mockups.

### Outputs

*   `src/styles/_typography.css` with typography styles.
*   Import added to `styles.css`.

### Detailed Workflow

#### Step 1: Create `_typography.css`

**What to do:** Define heading styles, body styles, and text utilities.

```css
// src/styles/_typography.css

// ================================================================
// 1. Base Body
// ================================================================
body {
    font-family: $font-family-base;
    font-size: $font-size-base;
    line-height: $line-height-base;
    color: $body-color;
    background-color: $body-bg;
}

// ================================================================
// 2. Headings
// ================================================================
h1, .h1 { font-size: 2.5rem; font-weight: 700; line-height: 1.2; margin-bottom: 0.5rem; }
h2, .h2 { font-size: 2rem; font-weight: 700; line-height: 1.2; margin-bottom: 0.5rem; }
h3, .h3 { font-size: 1.75rem; font-weight: 600; line-height: 1.3; margin-bottom: 0.5rem; }
h4, .h4 { font-size: 1.5rem; font-weight: 600; line-height: 1.3; margin-bottom: 0.5rem; }
h5, .h5 { font-size: 1.25rem; font-weight: 600; line-height: 1.4; margin-bottom: 0.5rem; }
h6, .h6 { font-size: 1rem; font-weight: 600; line-height: 1.4; margin-bottom: 0.5rem; }

// ================================================================
// 3. Text Utilities
// ================================================================
.text-muted { color: $text-muted; }
.text-secondary { color: $text-secondary; }
.text-primary { color: $primary; }
.text-success { color: $success; }
.text-danger { color: $danger; }
.text-warning { color: $warning; }

.text-center { text-align: center; }
.text-start { text-align: left; }
.text-end { text-align: right; }

// RTL‑aware text alignment (using Bootstrap's built‑in classes)
// Bootstrap already provides .text-start and .text-end that respect RTL

// ================================================================
// 4. Font weights
// ================================================================
.fw-light { font-weight: 300; }
.fw-normal { font-weight: 400; }
.fw-medium { font-weight: 500; }
.fw-semibold { font-weight: 600; }
.fw-bold { font-weight: 700; }

// ================================================================
// 5. Text size utilities
// ================================================================
.fs-1 { font-size: 2.5rem; }
.fs-2 { font-size: 2rem; }
.fs-3 { font-size: 1.75rem; }
.fs-4 { font-size: 1.5rem; }
.fs-5 { font-size: 1.25rem; }
.fs-6 { font-size: 1rem; }
.fs-7 { font-size: 0.875rem; }
.fs-8 { font-size: 0.75rem; }

// ================================================================
// 6. Text decoration
// ================================================================
.text-uppercase { text-transform: uppercase; }
.text-lowercase { text-transform: lowercase; }
.text-capitalize { text-transform: capitalize; }
.text-decoration-none { text-decoration: none; }
.text-decoration-underline { text-decoration: underline; }

// ================================================================
// 7. Truncation
// ================================================================
.text-truncate {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
}
```
#### Step 2: Update `styles.css`

Ensure `_typography.css` is imported after Bootstrap.

### Files To Create

*   `src/styles/_typography.css`

### CLI Commands

Manual creation.

### Concepts Required

*   Typography scales
*   css nesting

### UI/UX Requirements

*   Font sizes should be responsive (use relative units).
*   Line heights should be comfortable (1.5–1.7).

### Testing Steps

1.  Add sample headings and text to the app template.
2.  Verify that the styles are applied correctly.
3.  Test in both RTL and LTR (by toggling `dir`).

### Expected Deliverables

*   Typography system ready.

### Common Mistakes

**Mistake 1:** Using absolute font sizes (px) instead of rem.  
**Fix:** Use rem units for accessibility.

### Definition of Done

*   Typography styles are defined and applied.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `.h1` classes for semantic headings if needed.

## FE-RTL-003Create Utility Classes P0 Medium 3h ▾

### Task Information

*   **Task ID:** FE-RTL-003
*   **Task Name:** Create Utility Classes
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-RTL-001
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** High

### Objective

Create a comprehensive set of utility classes for spacing, flexbox, display, and other common layout needs. These utilities will speed up development and reduce the need for custom CSS.

### Business Purpose

Utilities reduce development time and ensure consistent spacing and alignment across the app.

### Technical Purpose

Provide css classes like `.m-3`, `.p-4`, `.d-flex`, `.align-items-center`, etc., that are RTL‑aware.

### Prerequisites

*   Understanding of CSS box model and flexbox.
*   Knowledge of css loops (to generate spacing utilities).

### Dependencies

*   **FE-RTL-001:** Variables defined.

### Inputs

*   Spacing scale from variables.

### Outputs

*   `src/styles/_utilities.css`

### Detailed Workflow

#### Step 1: Create `_utilities.css`

**What to do:** Define utility classes using css loops and helpers.

```css
// src/styles/_utilities.css

// ================================================================
// 1. Spacing (margin and padding)
// ================================================================

// Loop through spacers to generate margin and padding classes
@each $key, $value in $spacers {
    // Margin
    .m-#{$key} { margin: $value; }
    .mt-#{$key} { margin-top: $value; }
    .mb-#{$key} { margin-bottom: $value; }
    .ms-#{$key} { margin-left: $value; }  // Will be flipped in RTL
    .me-#{$key} { margin-right: $value; } // Will be flipped in RTL
    .mx-#{$key} { margin-left: $value; margin-right: $value; }
    .my-#{$key} { margin-top: $value; margin-bottom: $value; }

    // Padding
    .p-#{$key} { padding: $value; }
    .pt-#{$key} { padding-top: $value; }
    .pb-#{$key} { padding-bottom: $value; }
    .ps-#{$key} { padding-left: $value; }
    .pe-#{$key} { padding-right: $value; }
    .px-#{$key} { padding-left: $value; padding-right: $value; }
    .py-#{$key} { padding-top: $value; padding-bottom: $value; }
}

// ================================================================
// 2. Flexbox
// ================================================================
.d-flex { display: flex; }
.d-inline-flex { display: inline-flex; }
.flex-row { flex-direction: row; }
.flex-column { flex-direction: column; }
.flex-wrap { flex-wrap: wrap; }
.flex-nowrap { flex-wrap: nowrap; }

.justify-content-start { justify-content: flex-start; }
.justify-content-end { justify-content: flex-end; }
.justify-content-center { justify-content: center; }
.justify-content-between { justify-content: space-between; }
.justify-content-around { justify-content: space-around; }
.justify-content-evenly { justify-content: space-evenly; }

.align-items-start { align-items: flex-start; }
.align-items-end { align-items: flex-end; }
.align-items-center { align-items: center; }
.align-items-baseline { align-items: baseline; }
.align-items-stretch { align-items: stretch; }

.align-self-start { align-self: flex-start; }
.align-self-end { align-self: flex-end; }
.align-self-center { align-self: center; }

.flex-grow-0 { flex-grow: 0; }
.flex-grow-1 { flex-grow: 1; }
.flex-shrink-0 { flex-shrink: 0; }
.flex-shrink-1 { flex-shrink: 1; }

.gap-1 { gap: $spacer * 0.25; }
.gap-2 { gap: $spacer * 0.5; }
.gap-3 { gap: $spacer; }
.gap-4 { gap: $spacer * 1.5; }
.gap-5 { gap: $spacer * 3; }

// ================================================================
// 3. Display
// ================================================================
.d-block { display: block; }
.d-inline { display: inline; }
.d-inline-block { display: inline-block; }
.d-none { display: none; }

// ================================================================
// 4. Position
// ================================================================
.position-relative { position: relative; }
.position-absolute { position: absolute; }
.position-fixed { position: fixed; }
.position-sticky { position: sticky; }
.position-static { position: static; }

.top-0 { top: 0; }
.bottom-0 { bottom: 0; }
.start-0 { left: 0; }
.end-0 { right: 0; }

// ================================================================
// 5. Overflow
// ================================================================
.overflow-auto { overflow: auto; }
.overflow-hidden { overflow: hidden; }
.overflow-visible { overflow: visible; }
.overflow-scroll { overflow: scroll; }

// ================================================================
// 6. Width and Height
// ================================================================
.w-100 { width: 100%; }
.w-75 { width: 75%; }
.w-50 { width: 50%; }
.w-25 { width: 25%; }
.h-100 { height: 100%; }

// ================================================================
// 7. Border
// ================================================================
.border { border: 1px solid $border-color; }
.border-top { border-top: 1px solid $border-color; }
.border-bottom { border-bottom: 1px solid $border-color; }
.border-start { border-left: 1px solid $border-color; }
.border-end { border-right: 1px solid $border-color; }
.border-0 { border: 0; }
.border-radius { border-radius: $border-radius; }
.border-radius-lg { border-radius: $border-radius-lg; }
.border-radius-sm { border-radius: $border-radius-sm; }
.border-radius-pill { border-radius: $border-radius-pill; }
.border-radius-circle { border-radius: 50%; }

// ================================================================
// 8. Shadow
// ================================================================
.shadow-sm { box-shadow: $box-shadow-sm; }
.shadow { box-shadow: $box-shadow; }
.shadow-lg { box-shadow: $box-shadow-lg; }
.shadow-none { box-shadow: none; }

// ================================================================
// 9. Background
// ================================================================
.bg-white { background-color: #fff; }
.bg-transparent { background-color: transparent; }
.bg-primary { background-color: $primary; }
.bg-secondary { background-color: $secondary; }
.bg-success { background-color: $success; }
.bg-danger { background-color: $danger; }
.bg-warning { background-color: $warning; }
.bg-info { background-color: $info; }
.bg-light { background-color: $body-bg; }
.bg-dark { background-color: $primary-dark; }

// ================================================================
// 10. Cursor
// ================================================================
.cursor-pointer { cursor: pointer; }
.cursor-default { cursor: default; }    
```
#### Step 2: Update `styles.css`

Import `_utilities.css`.

### Files To Create

*   `src/styles/_utilities.css`

### CLI Commands

Manual creation.

### Concepts Required

*   css loops and maps
*   CSS Box Model
*   Flexbox

### UI/UX Requirements

*   Spacing utilities should be consistent with the design system.
*   RTL‑aware margin/padding classes must be correct (e.g., `ms-` becomes `me-` in RTL).

### Testing Steps

1.  Apply spacing classes to elements and verify they work.
2.  Test flex utilities with different layouts.
3.  Test RTL margin/padding by switching direction.

### Expected Deliverables

*   Utility classes ready.

### Common Mistakes

**Mistake 1:** Using `left` and `right` directly instead of `start` and `end` for RTL support.  
**Fix:** Use logical properties (e.g., `margin-inline-start`) or RTL‑aware classes.

### Definition of Done

*   All utility classes are defined and working.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use utility classes instead of custom CSS where possible.

## FE-RTL-004RTL Validation and Accessibility P1 Medium 3h ▾

### Task Information

*   **Task ID:** FE-RTL-004
*   **Task Name:** RTL Validation and Accessibility
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-RTL-001, FE-RTL-002, FE-RTL-003
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** High

### Objective

Validate the RTL implementation across all Bootstrap components and custom styles. Ensure accessibility (WCAG 2.1 AA) in terms of colour contrast, font sizes, and ARIA attributes.

### Business Purpose

RTL and accessibility are critical for the Arabic‑speaking audience and for compliance with regulations.

### Technical Purpose

Test and fix any RTL issues (e.g., misaligned icons, reversed margins). Use Lighthouse and axe DevTools to verify accessibility.

### Prerequisites

*   Understanding of WCAG 2.1 AA guidelines.
*   Knowledge of browser dev tools and accessibility testing tools.

### Dependencies

*   All previous RTL tasks.

### Inputs

*   Application shell with some components (e.g., navbar, cards, forms).

### Outputs

*   RTL validation report.
*   Accessibility audit report.
*   Fixes for any issues found.

### Detailed Workflow

#### Step 1: RTL Visual Validation

**What to do:** Manually test all UI components in RTL mode.

*   Check that text is right‑aligned.
*   Check that margins and paddings are flipped correctly.
*   Check that icons and images are mirrored if needed (e.g., arrows).
*   Check that dropdown menus open in the correct direction.

#### Step 2: Accessibility Audit

**What to do:** Use axe DevTools or Lighthouse to check for accessibility issues.

*   Run Lighthouse Accessibility audit.
*   Check colour contrast ratios.
*   Ensure font sizes are legible.
*   Check for missing ARIA attributes.

#### Step 3: Fix Issues

Address any issues found (e.g., adding `dir="rtl"` to the HTML, fixing contrast, adjusting margins).

### Files To Modify

Potentially `index.html` to set `dir="rtl"` or `lang="ar"`.

// src/index.html <html lang="ar" dir="rtl">

Also, ensure all components use logical CSS properties (e.g., `margin-inline-start`).

### Testing Steps

1.  Open the app in Chrome, set language to Arabic, and verify RTL.
2.  Run Lighthouse (Accessibility) and aim for score ≥ 90.
3.  Run axe DevTools and fix any critical issues.

### Expected Deliverables

*   RTL‑ready application.
*   Accessibility audit passed.

### Common Mistakes

**Mistake 1:** Forgetting to set `dir="rtl"` on the root element.  
**Fix:** Set in `index.html` or dynamically.

**Mistake 2:** Using absolute positioning with left/right values that break in RTL.  
**Fix:** Use `start`/`end` or logical properties.

### Edge Cases

*   Mixed LTR and RTL content (e.g., numbers, English words) should still render correctly.

### Definition of Done

*   All RTL issues fixed.
*   Accessibility audit passes.

### Frontend Architecture Notes

*   We rely on Bootstrap's RTL support and logical CSS properties.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Always test in RTL when adding new components.

## FE-RTL-005Commit and Document Design System P0 Medium 3h ▾

### Task Information

*   **Task ID:** FE-RTL-005
*   **Task Name:** Commit and Document Design System
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** All previous FE-RTL tasks
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** Critical

### Objective

Finalize the design system by documenting all design tokens, utilities, and RTL decisions. Commit all changes to the repository.

### Business Purpose

Documentation ensures that the design system is understood and used consistently by the team.

### Technical Purpose

Create a living style guide (or documentation) that lists all variables, classes, and usage examples.

### Prerequisites

*   All design system files created.

### Dependencies

*   All previous tasks.

### Inputs

*   css files from tasks 1–4.

### Outputs

*   Design system documentation (e.g., `/docs/design-system/`).
*   Commit with all changes.

### Detailed Workflow

#### Step 1: Create Design System Documentation

Write a Markdown document covering:

*   Colour palette with hex codes and usage
*   Typography scale
*   Spacing scale
*   Utility classes
*   RTL implementation notes

```markdown
<!-- /docs/design-system/README.md -->
# Jalsa Design System

## Colours
- Primary: #0F172A
- Secondary: #3B82F6
...

## Typography
- Headings: ...
...

## Utilities
- Spacing: .m-*, .p-*, etc.
- Flex: .d-flex, .justify-content-*, etc.
...

## RTL
- All components support RTL via Bootstrap RTL.
- Use logical properties where possible.
```
#### Step 2: Commit All Changes

```bash
git add .
git commit -m "feat: complete RTL and design system

- Bootstrap RTL configured
- Typography system defined
- Utility classes created
- RTL validation and accessibility fixes applied
- Design system documented"


```
### Files To Create

*   `/docs/design-system/README.md`

### CLI Commands

```bash
mkdir -p docs/design-system
touch docs/design-system/README.md
```
### Testing Steps

1.  Review the documentation for completeness.
2.  Ensure all files are committed.

### Expected Deliverables

*   Design system documentation.
*   Committed code.

### Common Mistakes

**Mistake 1:** Not documenting RTL‑specific utilities.  
**Fix:** Include RTL notes in the documentation.

### Definition of Done

*   All design system files are committed.
*   Documentation is complete.

### Frontend Architecture Notes

*   The design system is now the foundation for all UI development.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Refer to the design system documentation when building UI.

* * *

✅ Phase Completion Criteria
---------------------------

*   All 5 RTL & Design System tasks are complete (FE-RTL-001 to FE-RTL-005).
*   Bootstrap RTL is configured and working.
*   css variables are defined and used.
*   Typography system is complete and applied.
*   Utility classes are created and available.
*   RTL validation is done (all components look correct in RTL).
*   Accessibility audit passes (WCAG 2.1 AA).
*   Design system is documented.
*   All changes are committed to the repository.

📋 Code Review Checklist
------------------------

*   css variables are properly named and organised.
*   Typography styles are consistent with the design mockups.
*   Utility classes are comprehensive and follow naming conventions.
*   RTL support is correctly implemented (margin/padding flipped).
*   Accessibility checks are passed.

🚀 Pull Request Checklist
-------------------------

*   Branch is up‑to‑date with main.
*   All checks pass (lint, build, tests).
*   Code review completed.
*   Documentation updated.

🧪 Deployment Readiness Checklist
---------------------------------

*   Not applicable for this phase (design system only).

* * *

Jalsa – Phase 3: RTL & Design System – Expanded Implementation Handbook • v1.0 • For development team

