import { Injectable, inject, signal } from '@angular/core';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';

export interface InAppNotification {
    id: string;
    type: string;
    title: string;
    body: string | null;
    isRead: boolean;
    readAt: string | null;
    createdAt: string;
}

interface NotificationsResponse {
    notifications: InAppNotification[];
    unreadCount: number;
}

@Injectable({ providedIn: 'root' })
export class InAppNotificationService {
    private http = inject(HttpClientService);

    private notificationsSignal = signal<InAppNotification[]>([]);
    private unreadCountSignal = signal(0);

    readonly notifications = this.notificationsSignal.asReadonly();
    readonly unreadCount = this.unreadCountSignal.asReadonly();

    private pollInterval: ReturnType<typeof setInterval> | null = null;

    load(): void {
        this.http.get<NotificationsResponse>(API.notifications.base).subscribe({
            next: res => {
                this.notificationsSignal.set(res.notifications);
                this.unreadCountSignal.set(res.unreadCount);
            },
        });
    }

    startPolling(intervalMs = 30_000): void {
        this.load();
        this.pollInterval = setInterval(() => this.load(), intervalMs);
    }

    stopPolling(): void {
        if (this.pollInterval !== null) {
            clearInterval(this.pollInterval);
            this.pollInterval = null;
        }
    }

    markRead(id: string): void {
        this.http.patch<{ id: string; isRead: boolean }>(API.notifications.markRead(id), {}).subscribe({
            next: () => {
                this.notificationsSignal.update(list => list.map(n => (n.id === id ? { ...n, isRead: true } : n)));
                this.unreadCountSignal.update(c => Math.max(0, c - 1));
            },
        });
    }

    markAllRead(): void {
        this.http.patch<{ markedRead: number }>(API.notifications.markAllRead, {}).subscribe({
            next: () => {
                this.notificationsSignal.update(list => list.map(n => ({ ...n, isRead: true })));
                this.unreadCountSignal.set(0);
            },
        });
    }
}
