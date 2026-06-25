import { TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { TextareaComponent } from './textarea.component';

describe('TextareaComponent', () => {
    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [TextareaComponent, FormsModule],
        }).compileComponents();
    });

    it('should create', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        expect(component).toBeTruthy();
    });

    it('should have default values', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        expect(component.label).toBe('');
        expect(component.placeholder).toBe('');
        expect(component.helpText).toBe('');
        expect(component.error).toBe('');
        expect(component.required).toBe(false);
        expect(component.disabled).toBe(false);
        expect(component.readonly).toBe(false);
        expect(component.rows).toBe(4);
        expect(component.value).toBe('');
    });

    it('should render label when provided', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.label = 'Test Label';
        fixture.detectChanges();
        const label = fixture.nativeElement.querySelector('label');
        expect(label).toBeTruthy();
        expect(label.textContent).toContain('Test Label');
    });

    it('should not render label when not provided', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        fixture.detectChanges();
        const label = fixture.nativeElement.querySelector('label');
        expect(label).toBeFalsy();
    });

    it('should render textarea with correct rows', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.rows = 6;
        fixture.detectChanges();
        const textarea = fixture.nativeElement.querySelector('textarea');
        expect(textarea.rows).toBe(6);
    });

    it('should render placeholder', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.placeholder = 'Enter text';
        fixture.detectChanges();
        const textarea = fixture.nativeElement.querySelector('textarea');
        expect(textarea.placeholder).toBe('Enter text');
    });

    it('should render help text', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.helpText = 'Help text';
        fixture.detectChanges();
        const helpText = fixture.nativeElement.querySelector('.form-text');
        expect(helpText).toBeTruthy();
        expect(helpText.textContent).toContain('Help text');
    });

    it('should render error message', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.error = 'Error message';
        fixture.detectChanges();
        const error = fixture.nativeElement.querySelector('.invalid-feedback');
        expect(error).toBeTruthy();
        expect(error.textContent).toContain('Error message');
    });

    it('should apply error class when error is present', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.error = 'Error';
        fixture.detectChanges();
        const textarea = fixture.nativeElement.querySelector('textarea');
        expect(textarea.classList.contains('is-invalid')).toBe(true);
    });

    it('should disable textarea when disabled is true', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.disabled = true;
        fixture.detectChanges();
        const textarea = fixture.nativeElement.querySelector('textarea');
        expect(textarea.disabled).toBe(true);
    });

    it('should set textarea as required when required is true', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.required = true;
        fixture.detectChanges();
        const textarea = fixture.nativeElement.querySelector('textarea');
        expect(textarea.required).toBe(true);
    });

    it('should update value on input event', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        fixture.detectChanges();
        const textarea = fixture.nativeElement.querySelector('textarea');
        textarea.value = 'test value';
        textarea.dispatchEvent(new Event('input'));
        expect(component.value).toBe('test value');
    });

    it('should call onChange when value changes', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        let changedValue = '';
        component.registerOnChange((value: string) => {
            changedValue = value;
        });
        fixture.detectChanges();
        const textarea = fixture.nativeElement.querySelector('textarea');
        textarea.value = 'test value';
        textarea.dispatchEvent(new Event('input'));
        expect(changedValue).toBe('test value');
    });

    it('should call onTouched when textarea is blurred', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        let touched = false;
        component.registerOnTouched(() => {
            touched = true;
        });
        fixture.detectChanges();
        const textarea = fixture.nativeElement.querySelector('textarea');
        textarea.dispatchEvent(new Event('blur'));
        expect(touched).toBe(true);
    });

    it('should emit blur event', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        let blurEmitted = false;
        component.blur.subscribe(() => {
            blurEmitted = true;
        });
        fixture.detectChanges();
        const textarea = fixture.nativeElement.querySelector('textarea');
        textarea.dispatchEvent(new Event('blur'));
        expect(blurEmitted).toBe(true);
    });

    it('should set value via writeValue', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.writeValue('test value');
        expect(component.value).toBe('test value');
    });

    it('should disable textarea via setDisabledState', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.setDisabledState(true);
        expect(component.disabled).toBe(true);
    });

    it('should generate unique id when not provided', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        const id1 = component.textareaId;
        const id2 = component.textareaId;
        expect(id1).toBeTruthy();
        expect(id1).toBe(id2);
    });

    it('should use provided id', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.id = 'custom-id';
        expect(component.textareaId).toBe('custom-id');
    });

    it('should render textarea with correct attributes', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.name = 'test-textarea';
        component.maxlength = 500;
        component.minlength = 10;
        fixture.detectChanges();
        const textarea = fixture.nativeElement.querySelector('textarea');
        expect(textarea.name).toBe('test-textarea');
        expect(textarea.getAttribute('maxlength')).toBe('500');
        expect(textarea.getAttribute('minlength')).toBe('10');
    });

    it('should have correct aria attributes', () => {
        const fixture = TestBed.createComponent(TextareaComponent);
        const component = fixture.componentInstance;
        component.required = true;
        component.error = 'Error';
        fixture.detectChanges();
        const textarea = fixture.nativeElement.querySelector('textarea');
        expect(textarea.getAttribute('aria-required')).toBe('true');
        expect(textarea.getAttribute('aria-invalid')).toBe('true');
    });
});
