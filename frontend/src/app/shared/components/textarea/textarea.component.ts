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
    selector: 'app-textarea',
    standalone: true,
    templateUrl: './textarea.component.html',
    styleUrl: './textarea.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TextareaComponent),
            multi: true,
        },
    ],
})
export class TextareaComponent implements ControlValueAccessor {
    @Input() label = '';
    @Input() placeholder = '';
    @Input() helpText = '';
    @Input() error = '';
    @Input() required = false;
    @Input() disabled = false;
    @Input() readonly = false;
    @Input() id = '';
    @Input() name = '';
    @Input() rows = 4;
    @Input() cols?: number;
    @Input() maxlength?: number;
    @Input() minlength?: number;
    @Input() wrap: 'hard' | 'soft' = 'soft';

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
        const textarea = event.target as HTMLTextAreaElement;
        this.value = textarea.value;
        this.onChange(this.value);
        this.onTouched();
    }

    onBlur(event: FocusEvent): void {
        this.onTouched();
        this.blur.emit(event);
    }

    private _generatedId = `textarea-${Math.random().toString(36).substring(2, 9)}`;

    get textareaId(): string {
        return this.id || (this.name ? `textarea-${this.name}` : this._generatedId);
    }

    get ariaDescribedBy(): string {
        const ids: string[] = [];
        if (this.helpText) {
            ids.push(`${this.textareaId}-help`);
        }
        if (this.error) {
            ids.push(`${this.textareaId}-error`);
        }
        return ids.join(' ');
    }
}
