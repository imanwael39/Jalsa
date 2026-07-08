import { Component, ChangeDetectionStrategy, OnInit, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { InAppNotificationService, InAppNotification } from '../../../../core/services/in-app-notification.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';

@Component({
    selector: 'app-notification-list',
    standalone: true,
    imports: [DatePipe, ButtonComponent, EmptyStateComponent],
    templateUrl: './notification-list.component.html',
    styleUrl: './notification-list.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotificationListComponent implements OnInit {
    private notifService = inject(InAppNotificationService);
    private router = inject(Router);

    unreadCount = this.notifService.unreadCount;
    showUnreadOnly = signal(false);

    filteredNotifications = computed(() =>
        this.showUnreadOnly()
            ? this.notifService.notifications().filter(n => !n.isRead)
            : this.notifService.notifications()
    );

    ngOnInit(): void {
        this.notifService.load();
    }

    toggleUnreadOnly(): void {
        this.showUnreadOnly.update(v => !v);
    }

    markAllRead(): void {
        this.notifService.markAllRead();
    }

    onNotificationClick(notification: InAppNotification): void {
        this.notifService.markRead(notification.id);
        if (notification.type === 'CrisisAlert') {
            this.router.navigate(['/crisis-alerts']);
        }
    }

    iconClass(type: string): string {
        if (type === 'CrisisAlert') return 'bi-exclamation-triangle-fill';
        if (type === 'ExerciseReminder') return 'bi-clipboard-check';
        return 'bi-bell';
    }
}
