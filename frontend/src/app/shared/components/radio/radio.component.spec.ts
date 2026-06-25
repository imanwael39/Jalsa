import { TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { RadioComponent, RadioOption } from './radio.component';

describe('RadioComponent', () => {
    const mockOptions: RadioOption[] = [
        { value: 'option1', label: 'Option 1' },
        { value: 'option2', label: 'Option 2' },
        { value: 'option3', label: 'Option 3', disabled: true },
    ];

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [RadioComponent, FormsModule],
        }).compileComponents();
    });

    it('should create', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        expect(component).toBeTruthy();
    });

    it('should have default values', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        expect(component.label).toBe('');
        expect(component.helpText).toBe('');
        expect(component.error).toBe('');
        expect(component.required).toBe(false);
        expect(component.disabled).toBe(false);
        expect(component.readonly).toBe(false);
        expect(component.options).toEqual([]);
        expect(component.layout).toBe('stacked');
        expect(component.value).toBe('');
    });

    it('should render label when provided', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.label = 'Test Label';
        fixture.detectChanges();
        const label = fixture.nativeElement.querySelector('label');
        expect(label).toBeTruthy();
        expect(label.textContent).toContain('Test Label');
    });

    it('should not render label when not provided', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        fixture.detectChanges();
        const labels = fixture.nativeElement.querySelectorAll('label');
        expect(labels.length).toBe(0);
    });

    it('should render options', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        fixture.detectChanges();
        const radios = fixture.nativeElement.querySelectorAll('input[type="radio"]');
        expect(radios.length).toBe(3);
    });

    it('should render help text', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.helpText = 'Help text';
        fixture.detectChanges();
        const helpText = fixture.nativeElement.querySelector('.form-text');
        expect(helpText).toBeTruthy();
        expect(helpText.textContent).toContain('Help text');
    });

    it('should render error message', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.error = 'Error message';
        fixture.detectChanges();
        const error = fixture.nativeElement.querySelector('.invalid-feedback');
        expect(error).toBeTruthy();
        expect(error.textContent).toContain('Error message');
    });

    it('should apply error class when error is present', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        component.error = 'Error';
        fixture.detectChanges();
        const radios = fixture.nativeElement.querySelectorAll('input[type="radio"]');
        radios.forEach((radio: Element) => {
            expect(radio.classList.contains('is-invalid')).toBe(true);
        });
    });

    it('should disable radios when disabled is true', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        component.disabled = true;
        fixture.detectChanges();
        const radios = fixture.nativeElement.querySelectorAll('input[type="radio"]');
        radios.forEach((radio: Element) => {
            expect((radio as HTMLInputElement).disabled).toBe(true);
        });
    });

    it('should disable option when option.disabled is true', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        fixture.detectChanges();
        const radios = fixture.nativeElement.querySelectorAll('input[type="radio"]');
        const disabledRadio = radios[2] as HTMLInputElement;
        expect(disabledRadio.disabled).toBe(true);
    });

    it('should update value on change event', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        fixture.detectChanges();
        const radios = fixture.nativeElement.querySelectorAll('input[type="radio"]');
        const radio = radios[0] as HTMLInputElement;
        radio.checked = true;
        radio.dispatchEvent(new Event('change'));
        expect(component.value).toBe('option1');
    });

    it('should call onChange when value changes', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        let changedValue: string | number = '';
        component.registerOnChange((value: string | number) => {
            changedValue = value;
        });
        fixture.detectChanges();
        const radios = fixture.nativeElement.querySelectorAll('input[type="radio"]');
        const radio = radios[0] as HTMLInputElement;
        radio.checked = true;
        radio.dispatchEvent(new Event('change'));
        expect(changedValue).toBe('option1');
    });

    it('should call onTouched when radio is blurred', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        let touched = false;
        component.registerOnTouched(() => {
            touched = true;
        });
        fixture.detectChanges();
        const radios = fixture.nativeElement.querySelectorAll('input[type="radio"]');
        const radio = radios[0] as HTMLInputElement;
        radio.dispatchEvent(new Event('blur'));
        expect(touched).toBe(true);
    });

    it('should emit blur event', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        let blurEmitted = false;
        component.blur.subscribe(() => {
            blurEmitted = true;
        });
        fixture.detectChanges();
        const radios = fixture.nativeElement.querySelectorAll('input[type="radio"]');
        const radio = radios[0] as HTMLInputElement;
        radio.dispatchEvent(new Event('blur'));
        expect(blurEmitted).toBe(true);
    });

    it('should set value via writeValue', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.writeValue('option1');
        expect(component.value).toBe('option1');
    });

    it('should disable radios via setDisabledState', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.setDisabledState(true);
        expect(component.disabled).toBe(true);
    });

    it('should generate unique ids for radios', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        fixture.detectChanges();
        const id1 = component.getRadioId(0);
        const id2 = component.getRadioId(1);
        expect(id1).toBeTruthy();
        expect(id2).toBeTruthy();
        expect(id1).not.toBe(id2);
    });

    it('should use provided id for radios', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.id = 'custom-id';
        component.options = mockOptions;
        fixture.detectChanges();
        const id1 = component.getRadioId(0);
        expect(id1).toContain('custom-id');
    });

    it('should have correct aria attributes', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        component.error = 'Error';
        fixture.detectChanges();
        const radios = fixture.nativeElement.querySelectorAll('input[type="radio"]');
        radios.forEach((radio: Element) => {
            expect(radio.getAttribute('aria-invalid')).toBe('true');
        });
    });

    it('should emit change event', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.options = mockOptions;
        let changeEmitted: string | number = '';
        component.change.subscribe((value: string | number) => {
            changeEmitted = value;
        });
        fixture.detectChanges();
        const radios = fixture.nativeElement.querySelectorAll('input[type="radio"]');
        const radio = radios[0] as HTMLInputElement;
        radio.checked = true;
        radio.dispatchEvent(new Event('change'));
        expect(changeEmitted).toBe('option1');
    });

    it('should render radios with correct names', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.name = 'radio-group';
        component.options = mockOptions;
        fixture.detectChanges();
        const radios = fixture.nativeElement.querySelectorAll('input[type="radio"]');
        radios.forEach((radio: Element) => {
            expect((radio as HTMLInputElement).name).toBe('radio-group');
        });
    });

    it('should apply inline class when layout is inline', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.layout = 'inline';
        component.options = mockOptions;
        fixture.detectChanges();
        const radioDivs = fixture.nativeElement.querySelectorAll('.form-check-inline');
        expect(radioDivs.length).toBe(3);
    });

    it('should not apply inline class when layout is stacked', () => {
        const fixture = TestBed.createComponent(RadioComponent);
        const component = fixture.componentInstance;
        component.layout = 'stacked';
        component.options = mockOptions;
        fixture.detectChanges();
        const radioDivs = fixture.nativeElement.querySelectorAll('.form-check-inline');
        expect(radioDivs.length).toBe(0);
    });
});
