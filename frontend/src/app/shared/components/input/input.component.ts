import { ChangeDetectionStrategy, Component, EventEmitter, HostBinding, Input, Output, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'app-input',
    standalone: true,
    templateUrl: './input.component.html',
    styleUrls: ['./input.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
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
    @Input() type: 'text' | 'email' | 'password' | 'number' | 'tel' | 'url' | 'search' = 'text';
    @Input() placeholder = '';
    @Input() helpText = '';
    @Input() error = '';
    @Input() required = false;
    @Input() disabled = false;
    @Input() readonly = false;
    @Input() id = '';
    @Input() name = '';
    @Input() autocomplete = '';
    @Input() maxlength?: number;
    @Input() minlength?: number;
    @Input() pattern = '';
    @Input() min?: number;
    @Input() max?: number;
    @Input() step?: number;

    @Output() blur = new EventEmitter<FocusEvent>();

    value = '';
    private onChange: (value: string) => void = () => {};
    private onTouched: () => void = () => {};

    @HostBinding('class.form-group')
    get hostClass(): boolean {
        return true;
    }

    @HostBinding('class.has-error')
    get hasError(): boolean {
        return !!this.error;
    }

    writeValue(value: string): void {
        this.value = value ?? '';
    }

    registerOnChange(fn: (value: string) => void): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: () => void): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    onInput(event: Event): void {
        const input = event.target as HTMLInputElement;
        this.value = input.value;
        this.onChange(this.value);
        this.onTouched();
    }

    onBlur(event: FocusEvent): void {
        this.onTouched();
        this.blur.emit(event);
    }

    get inputId(): string {
        return this.id || `input-${this.name || Math.random().toString(36).substring(2, 9)}`;
    }

    get ariaDescribedBy(): string {
        const ids: string[] = [];
        if (this.helpText) {
            ids.push(`${this.inputId}-help`);
        }
        if (this.error) {
            ids.push(`${this.inputId}-error`);
        }
        return ids.join(' ');
    }
}
