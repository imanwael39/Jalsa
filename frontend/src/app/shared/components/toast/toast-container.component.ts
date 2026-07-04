import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { NotificationService } from '../../../core/services/notification.service';
import { ToastComponent } from '../toast/toast.component';

@Component({
    selector: 'app-toast-container',
    standalone: true,
    imports: [ToastComponent],
    template: `
        <div class="toast-wrapper">
            @for (n of notifications(); track n.id) {
                <app-toast
                    [message]="n.message"
                    [type]="n.type"
                    [duration]="n.duration ?? 5000"
                    (closed)="dismiss(n.id)"
                ></app-toast>
            }
        </div>
    `,
    styles: `
        :host {
            position: fixed;
            top: 1rem;
            inset-inline-end: 1rem;
            z-index: 1080;
            display: flex;
            flex-direction: column;
            gap: 0.5rem;
            pointer-events: none;
        }
        .toast-wrapper {
            display: flex;
            flex-direction: column;
            gap: 0.5rem;
            pointer-events: auto;
        }
    `,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToastContainerComponent {
    private notificationService = inject(NotificationService);
    notifications = this.notificationService.notifications;

    dismiss(id: string): void {
        this.notificationService.dismiss(id);
    }
}
