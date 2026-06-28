import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    HostBinding,
    Input,
    Output,
    forwardRef,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'app-checkbox',
    standalone: true,
    templateUrl: './checkbox.component.html',
    styleUrl: './checkbox.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => CheckboxComponent),
            multi: true,
        },
    ],
})
export class CheckboxComponent implements ControlValueAccessor {
    @Input() label = '';
    @Input() helpText = '';
    @Input() error = '';
    @Input() disabled = false;
    @Input() readonly = false;
    @Input() id = '';
    @Input() name = '';
    @Input() value: string | number = '';
    @Input() checked = false;
    @Input() indeterminate = false;

    @Output() blur = new EventEmitter<FocusEvent>();
    @Output() change = new EventEmitter<boolean>();

    private internalValue: boolean = false;
    private onChange: (value: boolean) => void = () => {};
    private onTouched: () => void = () => {};

    @HostBinding('class.form-check')
    get hostClass(): boolean {
        return true;
    }

    @HostBinding('class.has-error')
    get hasError(): boolean {
        return !!this.error;
    }

    writeValue(value: boolean): void {
        this.internalValue = value ?? false;
        this.checked = this.internalValue;
    }

    registerOnChange(fn: (value: boolean) => void): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: () => void): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    onCheckboxChange(event: Event): void {
        const checkbox = event.target as HTMLInputElement;
        this.internalValue = checkbox.checked;
        this.checked = this.internalValue;
        this.onChange(this.internalValue);
        this.change.emit(this.internalValue);
        this.onTouched();
    }

    onBlur(event: FocusEvent): void {
        this.onTouched();
        this.blur.emit(event);
    }

    private _generatedId = `checkbox-${Math.random().toString(36).substring(2, 9)}`;

    get checkboxId(): string {
        return this.id || (this.name ? `checkbox-${this.name}` : this._generatedId);
    }

    get ariaDescribedBy(): string {
        const ids: string[] = [];
        if (this.helpText) {
            ids.push(`${this.checkboxId}-help`);
        }
        if (this.error) {
            ids.push(`${this.checkboxId}-error`);
        }
        return ids.join(' ');
    }
}
