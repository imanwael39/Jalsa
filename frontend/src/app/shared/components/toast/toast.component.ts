import { ChangeDetectionStrategy, Component, EventEmitter, HostBinding, Input, OnDestroy, Output } from '@angular/core';

export type ToastType = 'success' | 'error' | 'warning' | 'info';

@Component({
    selector: 'app-toast',
    standalone: true,
    templateUrl: './toast.component.html',
    styleUrls: ['./toast.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToastComponent implements OnDestroy {
    @Input() message = '';
    @Input() title = '';
    @Input() type: ToastType = 'info';
    @Input() duration = 5000;
    @Input() autoDismiss = true;
    @Input() dismissible = true;

    @Output() closed = new EventEmitter<void>();

    private dismissTimer: ReturnType<typeof setTimeout> | null = null;
    isShowing = false;

    @HostBinding('class')
    get hostClasses(): string {
        return `toast-container`;
    }

    get typeIcon(): string {
        switch (this.type) {
            case 'success':
                return 'bi-check-circle-fill';
            case 'error':
                return 'bi-x-circle-fill';
            case 'warning':
                return 'bi-exclamation-triangle-fill';
            case 'info':
                return 'bi-info-circle-fill';
            default:
                return 'bi-info-circle-fill';
        }
    }

    get typeClass(): string {
        return `toast-${this.type}`;
    }

    show(): void {
        this.isShowing = true;

        if (this.autoDismiss && this.duration > 0) {
            this.dismissTimer = setTimeout(() => {
                this.close();
            }, this.duration);
        }
    }

    close(): void {
        if (this.dismissTimer) {
            clearTimeout(this.dismissTimer);
            this.dismissTimer = null;
        }

        this.isShowing = false;
        this.closed.emit();
    }

    onMouseEnter(): void {
        if (this.dismissTimer) {
            clearTimeout(this.dismissTimer);
            this.dismissTimer = null;
        }
    }

    onMouseLeave(): void {
        if (this.autoDismiss && this.duration > 0) {
            this.dismissTimer = setTimeout(() => {
                this.close();
            }, this.duration);
        }
    }

    ngOnDestroy(): void {
        if (this.dismissTimer) {
            clearTimeout(this.dismissTimer);
        }
    }
}
