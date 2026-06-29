import { ChangeDetectionStrategy, Component, EventEmitter, HostListener, Input, Output } from '@angular/core';

export type ModalSize = 'sm' | 'lg' | 'xl';

@Component({
    selector: 'app-modal',
    standalone: true,
    templateUrl: './modal.component.html',
    styleUrl: './modal.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ModalComponent {
    @Input() title = '';
    @Input() size: ModalSize = 'lg';
    @Input() closeOnBackdropClick = true;
    @Input() closeOnEscape = true;
    @Input() showModal = false;

    @Output() showModalChange = new EventEmitter<boolean>();
    @Output() closed = new EventEmitter<void>();
    @Output() opened = new EventEmitter<void>();

    close(): void {
        if (this.showModal) {
            this.showModal = false;
            this.showModalChange.emit(false);
            this.closed.emit();
        }
    }

    open(): void {
        if (!this.showModal) {
            this.showModal = true;
            this.showModalChange.emit(true);
            this.opened.emit();
        }
    }

    onBackdropClick(event: MouseEvent): void {
        if (this.closeOnBackdropClick && event.target === event.currentTarget) {
            this.close();
        }
    }

    onDialogContentClick(event: MouseEvent): void {
        event.stopPropagation();
    }

    @HostListener('document:keydown.escape')
    onEscapeKeydown(): void {
        if (this.closeOnEscape) {
            this.close();
        }
    }

    get sizeClass(): string {
        switch (this.size) {
            case 'sm':
                return 'modal-sm';
            case 'lg':
                return 'modal-lg';
            case 'xl':
                return 'modal-xl';
            default:
                return '';
        }
    }
}
