import { ChangeDetectionStrategy, Component, EventEmitter, HostBinding, Input, Output } from '@angular/core';

export type ButtonVariant =
    | 'primary'
    | 'secondary'
    | 'success'
    | 'danger'
    | 'warning'
    | 'info'
    | 'light'
    | 'dark'
    | 'link';

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
    @Input() icon?: string;

    @Output() clicked = new EventEmitter<MouseEvent>();

    get buttonClasses(): string {
        const classes = ['btn', `btn-${this.variant}`];

        if (this.size !== 'md') {
            classes.push(`btn-${this.size}`);
        }

        if (this.loading) {
            classes.push('btn-loading');
        }

        return classes.join(' ');
    }

    onClick(event: MouseEvent): void {
        if (!this.disabled && !this.loading) {
            this.clicked.emit(event);
        }
    }
}
