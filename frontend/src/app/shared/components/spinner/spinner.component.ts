import { ChangeDetectionStrategy, Component, HostBinding, Input } from '@angular/core';

export type SpinnerSize = 'sm' | 'md' | 'lg';

@Component({
    selector: 'app-spinner',
    standalone: true,
    templateUrl: './spinner.component.html',
    styleUrl: './spinner.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SpinnerComponent {
    @Input() size: SpinnerSize = 'md';
    @Input() colour = 'primary';
    @Input() overlay = false;
    @Input() label = 'جاري التحميل...';

    @HostBinding('class.spinner-overlay')
    get isOverlay(): boolean {
        return this.overlay;
    }

    @HostBinding('class.d-inline-flex')
    get isInline(): boolean {
        return !this.overlay;
    }

    @HostBinding('class.align-items-center')
    get alignCenter(): boolean {
        return !this.overlay;
    }

    @HostBinding('class.justify-content-center')
    get justifyCenter(): boolean {
        return !this.overlay;
    }

    get sizeClass(): string {
        switch (this.size) {
            case 'sm':
                return 'spinner-border-sm';
            case 'lg':
                return 'spinner-border-lg';
            default:
                return '';
        }
    }
}
