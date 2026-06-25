import { TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { CheckboxComponent } from './checkbox.component';

describe('CheckboxComponent', () => {
    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [CheckboxComponent, FormsModule],
        }).compileComponents();
    });

    it('should create', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        expect(component).toBeTruthy();
    });

    it('should have default values', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        expect(component.label).toBe('');
        expect(component.helpText).toBe('');
        expect(component.error).toBe('');
        expect(component.disabled).toBe(false);
        expect(component.readonly).toBe(false);
        expect(component.checked).toBe(false);
        expect(component.indeterminate).toBe(false);
        expect(component.value).toBe('');
    });

    it('should render label when provided', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.label = 'Test Label';
        fixture.detectChanges();
        const label = fixture.nativeElement.querySelector('label');
        expect(label).toBeTruthy();
        expect(label.textContent).toContain('Test Label');
    });

    it('should not render label when not provided', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        fixture.detectChanges();
        const label = fixture.nativeElement.querySelector('.form-check-label');
        expect(label).toBeFalsy();
    });

    it('should render help text', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.helpText = 'Help text';
        fixture.detectChanges();
        const helpText = fixture.nativeElement.querySelector('.form-text');
        expect(helpText).toBeTruthy();
        expect(helpText.textContent).toContain('Help text');
    });

    it('should render error message', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.error = 'Error message';
        fixture.detectChanges();
        const error = fixture.nativeElement.querySelector('.invalid-feedback');
        expect(error).toBeTruthy();
        expect(error.textContent).toContain('Error message');
    });

    it('should apply error class when error is present', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.error = 'Error';
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        expect(checkbox.classList.contains('is-invalid')).toBe(true);
    });

    it('should disable checkbox when disabled is true', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.disabled = true;
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        expect(checkbox.disabled).toBe(true);
    });

    it('should check checkbox when checked is true', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.checked = true;
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        expect(checkbox.checked).toBe(true);
    });

    it('should update value on change event', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        checkbox.checked = true;
        checkbox.dispatchEvent(new Event('change'));
        expect(component.checked).toBe(true);
    });

    it('should call onChange when value changes', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        let changedValue = false;
        component.registerOnChange((value: boolean) => {
            changedValue = value;
        });
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        checkbox.checked = true;
        checkbox.dispatchEvent(new Event('change'));
        expect(changedValue).toBe(true);
    });

    it('should call onTouched when checkbox is blurred', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        let touched = false;
        component.registerOnTouched(() => {
            touched = true;
        });
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        checkbox.dispatchEvent(new Event('blur'));
        expect(touched).toBe(true);
    });

    it('should emit blur event', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        let blurEmitted = false;
        component.blur.subscribe(() => {
            blurEmitted = true;
        });
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        checkbox.dispatchEvent(new Event('blur'));
        expect(blurEmitted).toBe(true);
    });

    it('should set value via writeValue', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.writeValue(true);
        expect(component.checked).toBe(true);
    });

    it('should disable checkbox via setDisabledState', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.setDisabledState(true);
        expect(component.disabled).toBe(true);
    });

    it('should generate unique id when not provided', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        const id1 = component.checkboxId;
        const id2 = component.checkboxId;
        expect(id1).toBeTruthy();
        expect(id1).toBe(id2);
    });

    it('should use provided id', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.id = 'custom-id';
        expect(component.checkboxId).toBe('custom-id');
    });

    it('should have correct aria attributes', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.error = 'Error';
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        expect(checkbox.getAttribute('aria-invalid')).toBe('true');
    });

    it('should emit change event', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        let changeEmitted = false;
        component.change.subscribe(() => {
            changeEmitted = true;
        });
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        checkbox.checked = true;
        checkbox.dispatchEvent(new Event('change'));
        expect(changeEmitted).toBe(true);
    });

    it('should set indeterminate state', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.indeterminate = true;
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        expect(checkbox.indeterminate).toBe(true);
    });

    it('should render checkbox with value attribute', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.value = 'checkbox-value';
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        expect(checkbox.value).toBe('checkbox-value');
    });

    it('should render checkbox with name attribute', () => {
        const fixture = TestBed.createComponent(CheckboxComponent);
        const component = fixture.componentInstance;
        component.name = 'checkbox-name';
        fixture.detectChanges();
        const checkbox = fixture.nativeElement.querySelector('input[type="checkbox"]');
        expect(checkbox.name).toBe('checkbox-name');
    });
});
