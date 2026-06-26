

🧰 Jalsa – Phase 4: Shared Component Library Expanded Implementation Handbook · 13 Tasks · 65+ Subtasks
=======================================================================================================

🧰 Phase 4 – Shared Component Library
-------------------------------------

**Purpose:** Build a reusable library of UI components, directives, and pipes that will be used across all features of the Jalsa platform. These shared elements ensure visual consistency, reduce code duplication, and accelerate feature development. Every component is built as a Standalone Angular component, follows the design system, and is fully tested.

📋 Tasks: 13 ⏱️ Total Effort: ~30 hours 👤 Owners: M3 (Frontend Lead), M4 (Frontend Developer) 🔗 Dependencies: Phase 3 (RTL & Design System) must be complete 🎯 Deliverable: Complete shared library with all components, directives, pipes, and documentation

## FE-SHARED-001Shared Library Setup P0 Medium 2h ▾

### Task Information

*   **Task ID:** FE-SHARED-001
*   **Task Name:** Shared Library Setup
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SETUP-004 (folder structure), FE-RTL-001 (design system variables)
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** Critical

### Objective

Create the folder structure for the shared library, set up barrel exports, and establish the pattern for building Standalone components. Provide a consistent way to import shared components across the application.

### Business Purpose

A well‑organised shared library makes it easy to find and reuse components, reducing development time and ensuring consistency.

### Technical Purpose

Define the shared library as a set of Standalone components, directives, and pipes that are exported via a single barrel file (`shared/index.ts`) so they can be imported easily.

### Prerequisites

*   Understanding of Angular Standalone components.
*   Knowledge of barrel exports and TypeScript path aliases (optional).

### Dependencies

*   **FE-SETUP-004:** The `shared/` folder exists.
*   **FE-RTL-001:** Design system variables are available for styling.

### Inputs

*   Design system from Phase 3.

### Outputs

*   `shared/components/` folder.
*   `shared/directives/` folder.
*   `shared/pipes/` folder.
*   `shared/index.ts` barrel file.
*   Optional path alias in `tsconfig.json` for easier imports.

### Detailed Workflow

#### Step 1: Create Subfolders

**What to do:** Inside `src/app/shared/`, create `components/`, `directives/`, `pipes/`.

```bash
cd src/app/shared
mkdir components directives pipes
```
#### Step 2: Create Barrel Export

**What to do:** Create `shared/index.ts` to export all shared artifacts.

```typescript
// shared/index.ts
export * from './components';
export * from './directives';
export * from './pipes';
```
But we'll create individual barrel files later. For now, just create the file and export empty stubs.

#### Step 3: Configure Path Alias (Optional)

Add a path alias in `tsconfig.json` for easier imports.

```json
// tsconfig.json
{
    "compilerOptions": {
        "paths": {
            "@shared/*": ["src/app/shared/*"]
        }
    }
}
```
Then restart the dev server.

#### Step 4: Create Placeholder `.gitkeep` Files

Add `.gitkeep` in each folder to track them.

### Files To Create

*   `shared/components/.gitkeep`
*   `shared/directives/.gitkeep`
*   `shared/pipes/.gitkeep`
*   `shared/index.ts`

### CLI Commands

```bash
cd src/app/shared
mkdir components directives pipes
touch index.ts
touch components/.gitkeep directives/.gitkeep pipes/.gitkeep
```
### Concepts Required

*   Barrel exports
*   Path aliases

### Tools/Libraries Required

*   None

### Testing Steps

1.  Import something from `@shared` in a component to verify the alias works.

### Expected Deliverables

*   Shared library structure ready.

### Common Mistakes

**Mistake 1:** Forgetting to update `tsconfig.json` paths correctly.  
**Fix:** Ensure the path is relative to `baseUrl`.

### Definition of Done

*   Folders and barrel file created.
*   Path alias working.

### Frontend Architecture Notes

*   All shared components will be Standalone.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `@shared` alias for imports.

## FE-SHARED-002Button Component P0 Medium 3h ▾

### Task Information

*   **Task ID:** FE-SHARED-002
*   **Task Name:** Button Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** Critical

### Objective

Create a reusable Button component that supports multiple variants (primary, secondary, success, danger, etc.), sizes, loading states, and disabled states. The component should be fully RTL‑aware and follow the design system.

### Business Purpose

Buttons are the most common interactive element; a consistent button component ensures brand consistency and accessibility.

### Technical Purpose

Build a Standalone component with inputs for variant, size, loading, disabled, and type. Use Bootstrap classes with custom SCSS overrides for theming.

### Prerequisites

*   Understanding of Angular components and inputs.
*   Knowledge of Bootstrap button classes.

### Dependencies

*   **FE-SHARED-001:** Shared library setup.

### Inputs

*   Design system colours and spacing.

### Outputs

*   `shared/components/button/button.component.ts`
*   `shared/components/button/button.component.html`
*   `shared/components/button/button.component.scss`
*   `shared/components/button/index.ts`

### Detailed Workflow

#### Step 1: Generate Component

```bash
ng g c shared/components/button --standalone --skip-tests
```
#### Step 2: Define Inputs

Add inputs for variant, size, loading, disabled, type, and aria-label.

```typescript
// button.component.ts
import { Component, Input, HostBinding } from '@angular/core';

type ButtonVariant = 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info' | 'light' | 'dark' | 'link';
type ButtonSize = 'sm' | 'md' | 'lg';

@Component({
    selector: 'app-button',
    standalone: true,
    templateUrl: './button.component.html',
    styleUrls: ['./button.component.scss'],
})
export class ButtonComponent {
    @Input() variant: ButtonVariant = 'primary';
    @Input() size: ButtonSize = 'md';
    @Input() loading = false;
    @Input() disabled = false;
    @Input() type: 'button' | 'submit' | 'reset' = 'button';
    @Input() ariaLabel?: string;
    @Input() icon?: string; // optional icon class

    @HostBinding('class')
    get classes(): string {
        const base = 'btn';
        const variantClass = `btn-${this.variant}`;
        const sizeClass = this.size !== 'md' ? `btn-${this.size}` : '';
        const loadingClass = this.loading ? 'btn-loading' : '';
        return [base, variantClass, sizeClass, loadingClass].filter(Boolean).join(' ');
    }

    @HostBinding('disabled')
    get isDisabled(): boolean {
        return this.disabled || this.loading;
    }
}

```
#### Step 3: Create Template

```html
<!-- button.component.html -->
<ng-container>
    <span *if="loading" class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
    <span *if="icon && !loading" class="bi {{ icon }}"></span>
    <ng-content></ng-content>
</ng-container>
```
Note: We use Bootstrap's spinner.

#### Step 4: Add Styles

```css
// button.component.scss
:host {
    display: inline-block;
}
.btn-loading {
    opacity: 0.65;
    pointer-events: none;
}
.btn-loading .spinner-border {
    margin-right: 0.5rem;
}
// RTL adjustments
[dir="rtl"] .btn-loading .spinner-border {
    margin-left: 0.5rem;
    margin-right: 0;
}
```
#### Step 5: Export in Barrel

Create `shared/components/button/index.ts`.

```typescript
export \* from './button.component';
```
#### Step 6: Update Shared Barrel

Update `shared/components/index.ts` to export button.

export \* from './button';

### Files To Create

*   button.component.ts
*   button.component.html
*   button.component.scss
*   button/index.ts

### CLI Commands

```bash
ng g c shared/components/button --standalone --skip-tests
# Then manually edit files
```
### Code Flow

```Parent Component → <app-button variant="primary" \[loading\]="true">Save</app-button> → ButtonComponent → Rendered button with classes and content
```
### Concepts Required

*   Angular components and inputs
*   HostBinding
*   Content projection
*   Bootstrap classes

### Tools/Libraries Required

*   Bootstrap CSS

### Angular Concepts Required

*   Standalone components
*   @Input and @HostBinding
*   ng-content

### UI/UX Requirements

*   Button states: default, hover, focus, active, disabled, loading.
*   RTL support: spacing and icon placement.

### Testing Steps

1.  Unit test: verify classes are applied correctly based on inputs.
2.  Test loading state: button is disabled and shows spinner.

### Expected Deliverables

*   Button component ready.

### Common Mistakes

**Mistake 1:** Not disabling the button when loading.  
**Fix:** Use HostBinding for disabled.

### Edge Cases

*   When both loading and disabled are true, loading takes precedence.

### Definition of Done

*   Button component works and is exported.

### Frontend Architecture Notes

*   Button uses Bootstrap classes; custom styles only for loading and RTL.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `<app-button>` instead of native `<button>`.

## FE-SHARED-003Input Components (Text, Textarea, Select, etc.) P0 High 5h ▾

### Task Information

*   **Task ID:** FE-SHARED-003
*   **Task Name:** Input Components (Text, Textarea, Select, etc.)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001
*   **Complexity:** High
*   **Estimated Effort:** 5 hours
*   **Priority:** Critical

### Objective

Create reusable form input components (Input, Textarea, Select, Checkbox, Radio) that support Bootstrap styling, validation states, and integrate with Angular Reactive Forms via ControlValueAccessor.

### Business Purpose

Forms are central to the application (patient intake, session notes, etc.). Consistent, accessible inputs improve user experience and reduce development time.

### Technical Purpose

Implement ControlValueAccessor for each input type so they work with `formControl` and `ngModel`. Include error messages, labels, and hints.

### Prerequisites

*   Understanding of Angular Reactive Forms and ControlValueAccessor.
*   Knowledge of Bootstrap form classes.

### Dependencies

*   **FE-SHARED-001:** Shared library setup.

### Inputs

*   Design system spacing and typography.

### Outputs

*   `shared/components/input/input.component.ts`
*   `shared/components/input/input.component.html`
*   `shared/components/textarea/textarea.component.ts`
*   `shared/components/select/select.component.ts`
*   `shared/components/checkbox/checkbox.component.ts`
*   Corresponding templates and styles.

### Detailed Workflow

#### Step 1: Generate Base Input Component

We'll create a generic input component that can be used for text, email, password, etc.

```bash
ng g c shared/components/input --standalone --skip-tests
```
Implement ControlValueAccessor:

```typescript
// input.component.ts
import { Component, Input, forwardRef, HostBinding } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'app-input',
    standalone: true,
    templateUrl: './input.component.html',
    styleUrls: ['./input.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => InputComponent),
            multi: true,
        },
    ],
})
export class InputComponent implements ControlValueAccessor {
    @Input() label = '';
    @Input() type = 'text';
    @Input() placeholder = '';
    @Input() helpText = '';
    @Input() error = '';
    @Input() required = false;
    @Input() disabled = false;

    value: string = '';
    onChange: (value: any) => void = () => {};
    onTouched: () => void = () => {};

    writeValue(value: any): void {
        this.value = value || '';
    }

    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    onInput(event: Event) {
        const input = event.target as HTMLInputElement;
        this.value = input.value;
        this.onChange(this.value);
        this.onTouched();
    }

    @HostBinding('class.has-error')
    get hasError(): boolean {
        return !!this.error;
    }
}
```
Template:

```html
<div class="form-group">
    <label *if="label" [class.required]="required">{{ label }}</label>
    <input
        [type]="type"
        class="form-control"
        [class.is-invalid]="error"
        [placeholder]="placeholder"
        [disabled]="disabled"
        [value]="value"
        (input)="onInput($event)"
    />
    <small *if="helpText" class="form-text text-muted">{{ helpText }}</small>
    <div *if="error" class="invalid-feedback">{{ error }}</div>
</div>
```
Similarly, create TextareaComponent, SelectComponent, CheckboxComponent, RadioComponent following the same pattern.

#### Step 2: Create Other Input Types

For each, generate component and implement ControlValueAccessor appropriately. For checkbox and radio, the value is boolean or selected option.

#### Step 3: Export in Barrel

Create `shared/components/input/index.ts`, etc., and update `shared/components/index.ts`.

### Files To Create

*   input/input.component.ts, .html, .scss
*   textarea/textarea.component.ts, .html, .scss
*   select/select.component.ts, .html, .scss
*   checkbox/checkbox.component.ts, .html, .scss
*   radio/radio.component.ts, .html, .scss
*   Each with index.ts

### CLI Commands

```bash
ng g c shared/components/input --standalone --skip-tests
ng g c shared/components/textarea --standalone --skip-tests
ng g c shared/components/select --standalone --skip-tests
ng g c shared/components/checkbox --standalone --skip-tests
ng g c shared/components/radio --standalone --skip-tests
```
### Concepts Required

*   ControlValueAccessor
*   Reactive Forms integration
*   Bootstrap form classes

### Tools/Libraries Required

*   Angular Forms

### Angular Concepts Required

*   NG\_VALUE\_ACCESSOR
*   forwardRef

### Testing Steps

1.  Unit test each component: writeValue, registerOnChange, etc.
2.  Test integration with Reactive Forms.

### Expected Deliverables

*   All input components ready.

### Common Mistakes

**Mistake 1:** Not calling `onTouched()` and `onChange()` correctly.  
**Fix:** Call them in appropriate event handlers.

### Edge Cases

*   When disabled, the input should not update.

### Definition of Done

*   All input components work with forms.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use these components in all forms for consistency.

## FE-SHARED-004Modal Component P0 High 4h ▾

### Task Information

*   **Task ID:** FE-SHARED-004
*   **Task Name:** Modal Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001, Bootstrap JS (for toggling)
*   **Complexity:** High
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Create a reusable modal component that can be opened and closed programmatically, with customizable header, body, and footer. Support for various sizes, backdrop options, and accessibility.

### Business Purpose

Modals are used for confirmations, forms, and additional information. A standard modal ensures consistency.

### Technical Purpose

Use Bootstrap's modal JavaScript (or a pure Angular implementation) with Angular's component interaction (Input/Output) to control visibility and events.

### Prerequisites

*   Understanding of Bootstrap modals.
*   Angular component communication (@Input, @Output).

### Dependencies

*   **FE-SHARED-001:** Setup.

### Inputs

*   Bootstrap modal markup.

### Outputs

*   `shared/components/modal/modal.component.ts`
*   HTML and SCSS.

### Detailed Workflow

#### Step 1: Generate Component

```bash
ng g c shared/components/modal --standalone --skip-tests
```
#### Step 2: Implement Modal with Inputs/Outputs

We'll use Bootstrap's modal classes and handle visibility with a boolean Input.

```typescript
// modal.component.ts
import { Component, Input, Output, EventEmitter, HostListener } from '@angular/core';

@Component({
    selector: 'app-modal',
    standalone: true,
    templateUrl: './modal.component.html',
    styleUrls: ['./modal.component.scss'],
})
export class ModalComponent {
    @Input() title = '';
    @Input() size: 'sm' | 'lg' | 'xl' = 'lg';
    @Input() closeOnBackdropClick = true;
    @Input() showModal = false;
    @Output() showModalChange = new EventEmitter<boolean>();
    @Output() closed = new EventEmitter<void>();

    close() {
        this.showModal = false;
        this.showModalChange.emit(false);
        this.closed.emit();
    }

    onBackdropClick() {
        if (this.closeOnBackdropClick) {
            this.close();
        }
    }

    @HostListener('document:keydown.escape')
    onEscape() {
        this.close();
    }
}
```
Template:

```html
<div class="modal" [class.show]="showModal" [class.d-block]="showModal" style="display: none;" (click)="onBackdropClick()">
    <div class="modal-dialog" [class.modal-sm]="size==='sm'" [class.modal-lg]="size==='lg'" [class.modal-xl]="size==='xl'" (click)="$event.stopPropagation()">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">{{ title }}</h5>
                <button type="button" class="btn-close" (click)="close()"></button>
            </div>
            <div class="modal-body">
                <ng-content select=".modal-body-content"></ng-content>
            </div>
            <div class="modal-footer">
                <ng-content select=".modal-footer-content"></ng-content>
            </div>
        </div>
    </div>
</div>
<div class="modal-backdrop fade show" *if="showModal"></div>
```
We use `ng-content` with selectors for body and footer to allow custom content.

#### Step 3: Add Styles

Customize modal backdrop and animation if needed.

#### Step 4: Export

Create `modal/index.ts` and update shared barrel.

### Files To Create

*   modal.component.ts, .html, .scss
*   modal/index.ts

### CLI Commands

```bash
ng g c shared/components/modal --standalone --skip-tests
```
### Concepts Required

*   Bootstrap modal structure
*   ng-content with selectors
*   HostListener for Escape key

### Testing Steps

1.  Test opening/closing via `showModal` input.
2.  Test Escape key closes.
3.  Test backdrop click closes if enabled.

### Expected Deliverables

*   Modal component ready.

### Common Mistakes

**Mistake 1:** Not stopping event propagation on modal content, causing backdrop click to close even when clicking inside.  
**Fix:** Use `$event.stopPropagation()` on modal-dialog.

### Definition of Done

*   Modal works with all features.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `<app-modal>` with `[(showModal)]`.

## FE-SHARED-005Data Table Component P0 High 5h ▾

### Task Information

*   **Task ID:** FE-SHARED-005
*   **Task Name:** Data Table Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001
*   **Complexity:** High
*   **Estimated Effort:** 5 hours
*   **Priority:** Critical

### Objective

Create a reusable data table component that displays a list of objects with configurable columns, supports sorting, pagination (via separate component), and row actions.

### Business Purpose

Tables are used extensively for patient lists, session lists, exercise lists, etc. A consistent table component accelerates development and ensures uniformity.

### Technical Purpose

Use Angular's structural directives and content projection to define columns. Accept an array of data and column definitions. Emit events for sorting and row actions.

### Prerequisites

*   Understanding of Angular templates and ngFor.
*   Knowledge of content projection with `ng-template`.

### Dependencies

*   **FE-SHARED-001:** Setup.

### Inputs

*   Data array, column definitions, sortable columns.

### Outputs

*   `shared/components/table/table.component.ts`
*   HTML, SCSS.

### Detailed Workflow

#### Step 1: Generate Component

```bash
ng g c shared/components/table --standalone --skip-tests
```
#### Step 2: Define Column and Table Logic

Use `ng-template` for cell rendering to allow custom content.

```typescript
// table.component.ts
import { Component, Input, Output, EventEmitter, ContentChildren, QueryList, TemplateRef } from '@angular/core';

export interface TableColumn {
    key: string;
    label: string;
    sortable?: boolean;
}

@Component({
    selector: 'app-table',
    standalone: true,
    templateUrl: './table.component.html',
    styleUrls: ['./table.component.scss'],
})
export class TableComponent {
    @Input() data: any[] = [];
    @Input() columns: TableColumn[] = [];
    @Input() loading = false;
    @Input() emptyMessage = 'No data available';
    @Output() sort = new EventEmitter<{ key: string; direction: 'asc' | 'desc' }>();

    sortColumn(key: string) {
        if (!this.columns.find(c => c.key === key && c.sortable)) return;
        const currentDirection = this.currentSortKey === key ? (this.currentSortDirection === 'asc' ? 'desc' : 'asc') : 'asc';
        this.currentSortKey = key;
        this.currentSortDirection = currentDirection;
        this.sort.emit({ key, direction: currentDirection });
    }

    private currentSortKey: string = '';
    private currentSortDirection: 'asc' | 'desc' = 'asc';
}
```
Template: iterate over columns and data, render rows using structural directives for custom cell templates.

#### Step 3: Use Content Projection for Custom Cells

We can create a `ColumnCellDirective` to identify custom cell templates.

#### Step 4: Export

### Files To Create

*   table.component.ts, .html, .scss
*   column-cell.directive.ts (optional)

### CLI Commands

```bash
ng g c shared/components/table --standalone --skip-tests
```
### Concepts Required

*   Content projection with ng-content or ng-template
*   Output events

### Testing Steps

1.  Test data rendering.
2.  Test sorting emits correct event.

### Expected Deliverables

*   Table component ready.

### Common Mistakes

**Mistake 1:** Not handling missing data gracefully.  
**Fix:** Show empty state message.

### Definition of Done

*   Table works with custom columns and sorting.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use table component for all data lists.

## FE-SHARED-006Pagination Component P0 Medium 3h ▾

### Task Information

*   **Task ID:** FE-SHARED-006
*   **Task Name:** Pagination Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** Critical

### Objective

Create a standalone pagination component that displays page numbers, previous/next buttons, and supports total items, page size, and page change events.

### Business Purpose

Used with tables and lists to navigate large datasets.

### Technical Purpose

Calculate total pages based on total items and page size. Emit page change event with new page number.

### Prerequisites

*   Basic math for pagination.

### Dependencies

*   **FE-SHARED-001:** Setup.

### Inputs

*   Total items, page size, current page.

### Outputs

*   `shared/components/pagination/pagination.component.ts`

### Detailed Workflow

#### Step 1: Generate Component

```bash
ng g c shared/components/pagination --standalone --skip-tests
```
#### Step 2: Implement Logic

```typescript
// pagination.component.ts
import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-pagination',
    standalone: true,
    templateUrl: './pagination.component.html',
    styleUrls: ['./pagination.component.scss'],
})
export class PaginationComponent {
    @Input() totalItems = 0;
    @Input() pageSize = 10;
    @Input() currentPage = 1;
    @Input() maxVisible = 5;
    @Output() pageChange = new EventEmitter<number>();

    get totalPages(): number {
        return Math.ceil(this.totalItems / this.pageSize);
    }

    get pages(): number[] {
        const total = this.totalPages;
        const current = this.currentPage;
        const max = this.maxVisible;
        let start = Math.max(1, current - Math.floor(max / 2));
        let end = Math.min(total, start + max - 1);
        if (end - start + 1 < max) {
            start = Math.max(1, end - max + 1);
        }
        return Array.from({ length: end - start + 1 }, (_, i) => start + i);
    }

    goToPage(page: number) {
        if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
            this.pageChange.emit(page);
        }
    }
}
```
Template: bootstrap pagination.

### Files To Create

*   pagination.component.ts, .html, .scss

### CLI Commands

```bash
ng g c shared/components/pagination --standalone --skip-tests
```
### Concepts Required

*   Computed properties
*   EventEmitter

### Testing Steps

1.  Test total pages calculation.
2.  Test page array generation.
3.  Test page change event.

### Expected Deliverables

*   Pagination component ready.

### Common Mistakes

**Mistake 1:** Not handling edge cases (0 items, 1 page).  
**Fix:** Check totalPages > 1 before showing pagination.

### Definition of Done

*   Pagination works and emits events.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use with table component.

## FE-SHARED-007Spinner/Loading Component P1 Medium 2h ▾

### Task Information

*   **Task ID:** FE-SHARED-007
*   **Task Name:** Spinner/Loading Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** High

### Objective

Create a spinner/loading indicator component that can be used as an overlay or inline, with different sizes and colours.

### Business Purpose

Provide visual feedback for async operations.

### Technical Purpose

Use Bootstrap spinner classes with custom theming.

### Prerequisites

*   Bootstrap spinner.

### Dependencies

*   **FE-SHARED-001:** Setup.

### Inputs

*   Size, colour, overlay flag.

### Outputs

*   `shared/components/spinner/spinner.component.ts`

### Detailed Workflow

#### Step 1: Generate Component

```bash
ng g c shared/components/spinner --standalone --skip-tests
```
#### Step 2: Implement

```typescript
// spinner.component.ts
import { Component, Input, HostBinding } from '@angular/core';

@Component({
    selector: 'app-spinner',
    standalone: true,
    templateUrl: './spinner.component.html',
    styleUrls: ['./spinner.component.scss'],
})
export class SpinnerComponent {
    @Input() size: 'sm' | 'md' | 'lg' = 'md';
    @Input() colour = 'primary';
    @Input() overlay = false;

    @HostBinding('class.overlay')
    get isOverlay(): boolean {
        return this.overlay;
    }
}
```
Template:

```html
<div class="spinner-border text-{{ colour }}" role="status" [class.spinner-border-sm]="size==='sm'" [class.spinner-border-lg]="size==='lg'">
    <span class="visually-hidden">Loading...</span>
</div>
```
### Files To Create

*   spinner.component.ts, .html, .scss

### CLI Commands

```bash
ng g c shared/components/spinner --standalone --skip-tests
```
### Testing Steps

1.  Test different sizes and colours.
2.  Test overlay mode (background).

### Expected Deliverables

*   Spinner component ready.

### Definition of Done

*   Spinner works.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use for loading states.

## FE-SHARED-008Toast/Notification Component P1 Medium 2h ▾

### Task Information

*   **Task ID:** FE-SHARED-008
*   **Task Name:** Toast/Notification Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001, Bootstrap
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** High

### Objective

Create a toast notification component that displays messages with different types (success, error, warning, info) and auto‑dismisses after a timeout.

### Business Purpose

Provide user feedback for actions like saving, errors, etc.

### Technical Purpose

Use Bootstrap's toast component with Angular animations for smooth appearance.

### Prerequisites

*   Bootstrap toast.

### Dependencies

*   **FE-SHARED-001:** Setup.

### Inputs

*   Message, type, duration.

### Outputs

*   `shared/components/toast/toast.component.ts`

### Detailed Workflow

#### Step 1: Generate Component

```bash 
ng g c shared/components/toast --standalone --skip-tests
```
#### Step 2: Implement

Use Bootstrap toast classes and Angular's `setTimeout` for auto‑dismiss.

### Files To Create

*   toast.component.ts, .html, .scss

### CLI Commands

```bash
ng g c shared/components/toast --standalone --skip-tests
```
### Testing Steps

1.  Test display with different types.
2.  Test auto‑dismiss.

### Expected Deliverables

*   Toast component ready.

### Definition of Done

*   Toast works.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use NotificationService to trigger toasts.

## FE-SHARED-009Empty State Component P1 Medium 2h ▾

### Task Information

*   **Task ID:** FE-SHARED-009
*   **Task Name:** Empty State Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** Medium

### Objective

Create a component to display when there is no data, with optional icon, message, and action button.

### Business Purpose

Improve user experience by providing clear feedback when lists are empty.

### Technical Purpose

Reusable component with inputs for title, message, image, and action.

### Prerequisites

*   Basic Angular component.

### Dependencies

*   **FE-SHARED-001:** Setup.

### Inputs

*   Title, message, image/icon, action label, action event.

### Outputs

*   `shared/components/empty-state/empty-state.component.ts`

### Detailed Workflow

#### Step 1: Generate Component

```bash
ng g c shared/components/empty-state --standalone --skip-tests
```
#### Step 2: Implement

Simple template with icon, title, description, and a button if action provided.

### Files To Create

*   empty-state.component.ts, .html, .scss

### CLI Commands

```bash
ng g c shared/components/empty-state --standalone --skip-tests
```
### Testing Steps

1.  Test rendering with different inputs.

### Expected Deliverables

*   Empty state component ready.

### Definition of Done

*   Empty state works.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use when data list is empty.

## FE-SHARED-010Role Directive P1 Medium 2h ▾

### Task Information

*   **Task ID:** FE-SHARED-010
*   **Task Name:** Role Directive
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001, AuthService (from Phase 5 – will be available later)
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** High

### Objective

Create a structural directive that conditionally renders content based on the user's role.

### Business Purpose

Control visibility of UI elements (buttons, menus, sections) based on user permissions.

### Technical Purpose

Use `*appRole` directive with a list of allowed roles. If the user has any of the roles, the content is rendered; otherwise, it is removed from the DOM.

### Prerequisites

*   Understanding of Angular structural directives.
*   AuthService with `hasRole()` method.

### Dependencies

*   **FE-SHARED-001:** Setup.
*   **AuthService:** Will be created in Phase 5, but we can prepare the directive now.

### Inputs

*   Roles array.

### Outputs

*   `shared/directives/role.directive.ts`

### Detailed Workflow

#### Step 1: Generate Directive

```bash
ng g d shared/directives/role --standalone --skip-tests
```
#### Step 2: Implement

```typescript
// role.directive.ts
import { Directive, Input, TemplateRef, ViewContainerRef, inject } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';

@Directive({
    selector: '[appRole]',
    standalone: true,
})
export class RoleDirective {
    private templateRef = inject(TemplateRef);
    private viewContainer = inject(ViewContainerRef);
    private authService = inject(AuthService);

    @Input() set appRole(roles: string[]) {
        if (this.authService.hasAnyRole(roles)) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        } else {
            this.viewContainer.clear();
        }
    }
}
```
Note: `hasAnyRole` should be added to AuthService.

### Files To Create

*   role.directive.ts

### CLI Commands

```bash
ng g d shared/directives/role --standalone --skip-tests
```
### Testing Steps

1.  Test with different roles.
2.  Verify element is removed or shown.

### Expected Deliverables

*   Role directive ready.

### Common Mistakes

**Mistake 1:** Not using `hasAnyRole` correctly.  
**Fix:** Ensure AuthService method exists.

### Definition of Done

*   Role directive works.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `*appRole="['Therapist']"` to conditionally show content.

## FE-SHARED-011Click Outside Directive P1 Medium 2h ▾

### Task Information

*   **Task ID:** FE-SHARED-011
*   **Task Name:** Click Outside Directive
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** Medium

### Objective

Create a directive that emits an event when a click occurs outside the element it is attached to. Useful for closing dropdowns, modals, and menus.

### Business Purpose

Improve UX by closing dropdowns when clicking away.

### Technical Purpose

Listen for `document:click` and check if the target is inside the element.

### Prerequisites

*   Understanding of DOM events and ElementRef.

### Dependencies

*   **FE-SHARED-001:** Setup.

### Inputs

*   Optional exclude elements.

### Outputs

*   `shared/directives/click-outside.directive.ts`

### Detailed Workflow

#### Step 1: Generate Directive

```bash
ng g d shared/directives/click-outside --standalone --skip-tests
```
#### Step 2: Implement

```typescript
// click-outside.directive.ts
import { Directive, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';

@Directive({
    selector: '[appClickOutside]',
    standalone: true,
})
export class ClickOutsideDirective {
    @Output() appClickOutside = new EventEmitter<void>();

    constructor(private elementRef: ElementRef) {}

    @HostListener('document:click', ['$event'])
    onClick(event: MouseEvent) {
        const target = event.target as HTMLElement;
        if (!this.elementRef.nativeElement.contains(target)) {
            this.appClickOutside.emit();
        }
    }
}
```
### Files To Create

*   click-outside.directive.ts

### CLI Commands

```bash
ng g d shared/directives/click-outside --standalone --skip-tests
```
### Testing Steps

1.  Test directive on a dropdown menu.
2.  Click inside and outside to verify events.

### Expected Deliverables

*   Click outside directive ready.

### Definition of Done

*   Directive works.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `(appClickOutside)="close()"` on dropdowns.

## FE-SHARED-012Truncate Pipe P1 Low 1h ▾

### Task Information

*   **Task ID:** FE-SHARED-012
*   **Task Name:** Truncate Pipe
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001
*   **Complexity:** Low
*   **Estimated Effort:** 1 hour
*   **Priority:** Low

### Objective

Create a pipe that truncates a string to a specified length and appends an ellipsis.

### Business Purpose

Useful for displaying long text in tables and cards.

### Technical Purpose

Pure pipe that returns substring and adds '...' if truncated.

### Prerequisites

*   Understanding of Angular pipes.

### Dependencies

*   **FE-SHARED-001:** Setup.

### Inputs

*   String, length.

### Outputs

*   `shared/pipes/truncate.pipe.ts`

### Detailed Workflow

#### Step 1: Generate Pipe

```bash
ng g p shared/pipes/truncate --standalone --skip-tests
```
#### Step 2: Implement

```typescript
// truncate.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'truncate',
    standalone: true,
})
export class TruncatePipe implements PipeTransform {
    transform(value: string, limit: number = 100, trail: string = '…'): string {
        if (!value) return '';
        if (value.length <= limit) return value;
        return value.substring(0, limit) + trail;
    }
}
```
### Files To Create

*   truncate.pipe.ts

### CLI Commands

```bash
ng g p shared/pipes/truncate --standalone --skip-tests
```
### Testing Steps

1.  Test pipe with different lengths.

### Expected Deliverables

*   Truncate pipe ready.

### Definition of Done

*   Pipe works.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `{{ text | truncate:50 }}`.

## FE-SHARED-013Date Ago Pipe P1 Low 1h ▾

### Task Information

*   **Task ID:** FE-SHARED-013
*   **Task Name:** Date Ago Pipe
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001
*   **Complexity:** Low
*   **Estimated Effort:** 1 hour
*   **Priority:** Low

### Objective

Create a pipe that converts a date into a relative time string (e.g., "2 hours ago").

### Business Purpose

Display human‑readable timestamps for sessions, messages, etc.

### Technical Purpose

Use JavaScript `Date` to calculate difference and return a string.

### Prerequisites

*   Date manipulation.

### Dependencies

*   **FE-SHARED-001:** Setup.

### Inputs

*   Date string or Date object.

### Outputs

*   `shared/pipes/date-ago.pipe.ts`

### Detailed Workflow

#### Step 1: Generate Pipe

```bash
ng g p shared/pipes/date-ago --standalone --skip-tests
```
#### Step 2: Implement

```typescript
// date-ago.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'dateAgo',
    standalone: true,
})
export class DateAgoPipe implements PipeTransform {
    transform(value: string | Date): string {
        if (!value) return '';
        const date = typeof value === 'string' ? new Date(value) : value;
        const now = new Date();
        const diff = Math.floor((now.getTime() - date.getTime()) / 1000);

        if (diff < 60) return 'just now';
        if (diff < 3600) return Math.floor(diff / 60) + 'm ago';
        if (diff < 86400) return Math.floor(diff / 3600) + 'h ago';
        if (diff < 604800) return Math.floor(diff / 86400) + 'd ago';
        if (diff < 2592000) return Math.floor(diff / 604800) + 'w ago';
        if (diff < 31536000) return Math.floor(diff / 2592000) + 'mo ago';
        return Math.floor(diff / 31536000) + 'y ago';
    }
}
```
### Files To Create

*   date-ago.pipe.ts

### CLI Commands

```bash
ng g p shared/pipes/date-ago --standalone --skip-tests
```
### Testing Steps

1.  Test with different dates.

### Expected Deliverables

*   Date ago pipe ready.

### Definition of Done

*   Pipe works.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `{{ timestamp | dateAgo }}`.

* * *

✅ Phase Completion Criteria
---------------------------

*   All 13 shared tasks are complete (FE-SHARED-001 to FE-SHARED-013).
*   All components are Standalone and exported via barrel files.
*   Button component supports all variants, sizes, loading, and disabled.
*   Input components (text, textarea, select, checkbox, radio) implement ControlValueAccessor and work with Reactive Forms.
*   Modal component is fully functional with inputs/outputs, Escape key, and backdrop handling.
*   Data Table component displays data with sorting and custom cell templates.
*   Pagination component calculates pages and emits events.
*   Spinner, Toast, Empty State components are ready.
*   Role and Click Outside directives work.
*   Truncate and Date Ago pipes are implemented.
*   All components follow the design system and are RTL‑aware.
*   Unit tests pass for each component.
*   All changes are committed to the repository.

📋 Code Review Checklist
------------------------

*   Components are properly named and follow naming conventions.
*   All components use `ChangeDetectionStrategy.OnPush` (add if not).
*   ControlValueAccessor implementations are correct.
*   Directives handle edge cases (e.g., multiple roles).
*   Pipes are pure and efficient.
*   RTL support is verified (e.g., margin for spinner in button).
*   Accessibility: labels, aria attributes, keyboard navigation.

🚀 Pull Request Checklist
-------------------------

*   Branch is up‑to‑date with main.
*   All tests pass.
*   Code review completed.
*   Documentation updated (if applicable).

🧪 Deployment Readiness Checklist
---------------------------------

*   Not applicable for this phase (shared library).

* * *

Jalsa – Phase 4: Shared Component Library – Expanded Implementation Handbook • v1.0 • For development team

