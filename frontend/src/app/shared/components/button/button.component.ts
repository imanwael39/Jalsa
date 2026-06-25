import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

export type ButtonVariant = 'primary' | 'secondary' | 'danger' | 'ghost';
export type ButtonSize = 'sm' | 'md' | 'lg';

@Component({
    selector: 'app-button',
    standalone: true,
    templateUrl: './button.component.html',
    styleUrls: ['./button.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ButtonComponent {
    @Input() variant: ButtonVariant = 'primary';
    @Input() size: ButtonSize = 'md';
    @Input() loading = false;
    @Input() disabled = false;
    @Input() type: 'button' | 'submit' | 'reset' = 'button';
    @Input() ariaLabel?: string;

    @Output() clicked = new EventEmitter<MouseEvent>();

    get buttonClasses(): string {
        return `btn btn-${this.variant} btn-${this.size}` + (this.loading ? ' is-loading' : '');
    }

    get isDisabled(): boolean {
        return this.disabled || this.loading;
    }

    onClick(event: MouseEvent): void {
        if (!this.isDisabled) {
            this.clicked.emit(event);
        }
    }
}
