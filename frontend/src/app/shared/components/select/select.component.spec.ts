import { TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { SelectComponent, SelectOption } from './select.component';

describe('SelectComponent', () => {
    const mockOptions: SelectOption[] = [
        { value: 'option1', label: 'Option 1' },
        { value: 'option2', label: 'Option 2' },
        { value: 'option3', label: 'Option 3', disabled: true },
    ];

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [SelectComponent, FormsModule],
        }).compileComponents();
    });

    it('should create', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        expect(component).toBeTruthy();
    });

    it('should have default values', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        expect(component.label).toBe('');
        expect(component.placeholder).toBe('Select an option');
        expect(component.helpText).toBe('');
        expect(component.error).toBe('');
        expect(component.required).toBe(false);
        expect(component.disabled).toBe(false);
        expect(component.readonly).toBe(false);
        expect(component.options).toEqual([]);
        expect(component.multiple).toBe(false);
        expect(component.value).toBe('');
    });

    it('should render label when provided', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.label = 'Test Label';
        fixture.detectChanges();
        const label = fixture.nativeElement.querySelector('label');
        expect(label).toBeTruthy();
        expect(label.textContent).toContain('Test Label');
    });

    it('should not render label when not provided', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        fixture.detectChanges();
        const label = fixture.nativeElement.querySelector('label');
        expect(label).toBeFalsy();
    });

    it('should render options', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        fixture.detectChanges();
        const options = fixture.nativeElement.querySelectorAll('option');
        expect(options.length).toBe(4); // 3 options + 1 placeholder
    });

    it('should render placeholder option', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        fixture.detectChanges();
        const placeholderOption = fixture.nativeElement.querySelector('option[value=""]');
        expect(placeholderOption).toBeTruthy();
        expect(placeholderOption.textContent).toContain('Select an option');
    });

    it('should render help text', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.helpText = 'Help text';
        fixture.detectChanges();
        const helpText = fixture.nativeElement.querySelector('.form-text');
        expect(helpText).toBeTruthy();
        expect(helpText.textContent).toContain('Help text');
    });

    it('should render error message', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.error = 'Error message';
        fixture.detectChanges();
        const error = fixture.nativeElement.querySelector('.invalid-feedback');
        expect(error).toBeTruthy();
        expect(error.textContent).toContain('Error message');
    });

    it('should apply error class when error is present', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.error = 'Error';
        fixture.detectChanges();
        const select = fixture.nativeElement.querySelector('select');
        expect(select.classList.contains('is-invalid')).toBe(true);
    });

    it('should disable select when disabled is true', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.disabled = true;
        fixture.detectChanges();
        const select = fixture.nativeElement.querySelector('select');
        expect(select.disabled).toBe(true);
    });

    it('should set select as required when required is true', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.required = true;
        fixture.detectChanges();
        const select = fixture.nativeElement.querySelector('select');
        expect(select.required).toBe(true);
    });

    it('should update value on change event', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        fixture.detectChanges();
        const select = fixture.nativeElement.querySelector('select');
        select.value = 'option1';
        select.dispatchEvent(new Event('change'));
        expect(component.value).toBe('option1');
    });

    it('should call onChange when value changes', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        let changedValue: string | number | string[] | number[] = '';
        component.registerOnChange((value: string | number | string[] | number[]) => {
            changedValue = value;
        });
        fixture.detectChanges();
        const select = fixture.nativeElement.querySelector('select');
        select.value = 'option1';
        select.dispatchEvent(new Event('change'));
        expect(changedValue).toBe('option1');
    });

    it('should call onTouched when select is blurred', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        let touched = false;
        component.registerOnTouched(() => {
            touched = true;
        });
        fixture.detectChanges();
        const select = fixture.nativeElement.querySelector('select');
        select.dispatchEvent(new Event('blur'));
        expect(touched).toBe(true);
    });

    it('should emit blur event', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        let blurEmitted = false;
        component.blur.subscribe(() => {
            blurEmitted = true;
        });
        fixture.detectChanges();
        const select = fixture.nativeElement.querySelector('select');
        select.dispatchEvent(new Event('blur'));
        expect(blurEmitted).toBe(true);
    });

    it('should set value via writeValue', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.writeValue('option1');
        expect(component.value).toBe('option1');
    });

    it('should disable select via setDisabledState', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.setDisabledState(true);
        expect(component.disabled).toBe(true);
    });

    it('should generate unique id when not provided', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        const id1 = component.selectId;
        const id2 = component.selectId;
        expect(id1).toBeTruthy();
        expect(id1).toBe(id2);
    });

    it('should use provided id', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.id = 'custom-id';
        expect(component.selectId).toBe('custom-id');
    });

    it('should have correct aria attributes', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.required = true;
        component.error = 'Error';
        fixture.detectChanges();
        const select = fixture.nativeElement.querySelector('select');
        expect(select.getAttribute('aria-required')).toBe('true');
        expect(select.getAttribute('aria-invalid')).toBe('true');
    });

    it('should disable option when option.disabled is true', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        fixture.detectChanges();
        const options = fixture.nativeElement.querySelectorAll('option');
        const disabledOption = Array.from(options).find((option: Element) => option.textContent?.includes('Option 3'));
        expect(disabledOption).toBeTruthy();
        expect((disabledOption as HTMLOptionElement).disabled).toBe(true);
    });

    it('should render multiple select when multiple is true', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.multiple = true;
        component.options = mockOptions;
        fixture.detectChanges();
        const select = fixture.nativeElement.querySelector('select');
        expect(select.multiple).toBe(true);
    });

    it('should not render placeholder when multiple is true', () => {
        const fixture = TestBed.createComponent(SelectComponent);
        const component = fixture.componentInstance;
        component.multiple = true;
        component.options = mockOptions;
        fixture.detectChanges();
        const placeholderOption = fixture.nativeElement.querySelector('option[value=""]');
        expect(placeholderOption).toBeFalsy();
    });
});
