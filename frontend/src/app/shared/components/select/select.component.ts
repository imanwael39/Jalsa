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
import { JsonPipe } from '@angular/common';

export interface SelectOption {
    value: string | number;
    label: string;
    disabled?: boolean;
}

@Component({
    selector: 'app-select',
    standalone: true,
    templateUrl: './select.component.html',
    styleUrl: './select.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [JsonPipe],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SelectComponent),
            multi: true,
        },
    ],
})
export class SelectComponent implements ControlValueAccessor {
    @Input() label = '';
    @Input() placeholder = 'اختر...';
    @Input() helpText = '';
    @Input() error = '';
    @Input() required = false;
    @Input() disabled = false;
    @Input() readonly = false;
    @Input() id = '';
    @Input() name = '';
    @Input() options: SelectOption[] = [];
    @Input() multiple = false;
    @Input() size?: number;

    @Output() blur = new EventEmitter<FocusEvent>();
    @Output() change = new EventEmitter<string | number | string[] | number[]>();

    value: string | number | string[] | number[] = '';
    private onChange: (value: string | number | string[] | number[]) => void = () => {};
    private onTouched: () => void = () => {};

    @HostBinding('class.form-group')
    get hostClass(): boolean {
        return true;
    }

    @HostBinding('class.has-error')
    get hasError(): boolean {
        return !!this.error;
    }

    writeValue(value: string | number | string[] | number[]): void {
        this.value = value ?? (this.multiple ? [] : '');
    }

    registerOnChange(fn: (value: string | number | string[] | number[]) => void): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: () => void): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    onSelectChange(event: Event): void {
        const select = event.target as HTMLSelectElement;

        if (this.multiple) {
            const selectedOptions = Array.from(select.selectedOptions).map(option => option.value);
            this.value = selectedOptions;
            this.onChange(selectedOptions);
            this.change.emit(selectedOptions);
        } else {
            this.value = select.value;
            this.onChange(select.value);
            this.change.emit(select.value);
        }

        this.onTouched();
    }

    onBlur(event: FocusEvent): void {
        this.onTouched();
        this.blur.emit(event);
    }

    private _generatedId = `select-${Math.random().toString(36).substring(2, 9)}`;

    get selectId(): string {
        return this.id || (this.name ? `select-${this.name}` : this._generatedId);
    }

    get ariaDescribedBy(): string {
        const ids: string[] = [];
        if (this.helpText) {
            ids.push(`${this.selectId}-help`);
        }
        if (this.error) {
            ids.push(`${this.selectId}-error`);
        }
        return ids.join(' ');
    }

    trackByValue(index: number, option: SelectOption): string | number {
        return option.value;
    }
}
