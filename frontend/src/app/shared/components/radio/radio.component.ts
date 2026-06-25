import { ChangeDetectionStrategy, Component, EventEmitter, HostBinding, Input, Output, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export interface RadioOption {
    value: string | number;
    label: string;
    disabled?: boolean;
}

@Component({
    selector: 'app-radio',
    standalone: true,
    templateUrl: './radio.component.html',
    styleUrl: './radio.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => RadioComponent),
            multi: true,
        },
    ],
})
export class RadioComponent implements ControlValueAccessor {
    @Input() label = '';
    @Input() helpText = '';
    @Input() error = '';
    @Input() required = false;
    @Input() disabled = false;
    @Input() readonly = false;
    @Input() id = '';
    @Input() name = '';
    @Input() options: RadioOption[] = [];
    @Input() layout: 'stacked' | 'inline' = 'stacked';

    @Output() blur = new EventEmitter<FocusEvent>();
    @Output() change = new EventEmitter<string | number>();

    value: string | number = '';
    private onChange: (value: string | number) => void = () => {};
    private onTouched: () => void = () => {};

    @HostBinding('class.form-group')
    get hostClass(): boolean {
        return true;
    }

    @HostBinding('class.has-error')
    get hasError(): boolean {
        return !!this.error;
    }

    @HostBinding('class.form-check-inline')
    get isInline(): boolean {
        return this.layout === 'inline';
    }

    writeValue(value: string | number): void {
        this.value = value ?? '';
    }

    registerOnChange(fn: (value: string | number) => void): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: () => void): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    onRadioChange(event: Event): void {
        const radio = event.target as HTMLInputElement;
        this.value = radio.value;
        this.onChange(this.value);
        this.change.emit(this.value);
        this.onTouched();
    }

    onBlur(event: FocusEvent): void {
        this.onTouched();
        this.blur.emit(event);
    }

    getRadioId(index: number): string {
        return this.id || `radio-${this.name || Math.random().toString(36).substring(2, 9)}-${index}`;
    }

    get ariaDescribedBy(): string {
        const ids: string[] = [];
        if (this.helpText) {
            ids.push(`${this.id || this.name}-help`);
        }
        if (this.error) {
            ids.push(`${this.id || this.name}-error`);
        }
        return ids.join(' ');
    }

    trackByValue(index: number, option: RadioOption): string | number {
        return option.value;
    }
}
