import { Injectable, inject, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { environment } from '../../../environments/environment';

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
    private newNotificationSignal = signal<InAppNotification | null>(null);

    readonly notifications = this.notificationsSignal.asReadonly();
    readonly unreadCount = this.unreadCountSignal.asReadonly();
    /** Emits the most recently pushed notification so consumers (e.g. a toast) can react to it. */
    readonly newNotification = this.newNotificationSignal.asReadonly();

    private pollInterval: ReturnType<typeof setInterval> | null = null;
    private hubConnection: signalR.HubConnection | null = null;

    load(): void {
        this.http.get<NotificationsResponse>(API.notifications.base).subscribe({
            next: res => {
                this.notificationsSignal.set(res.notifications);
                this.unreadCountSignal.set(res.unreadCount);
            },
            error: () => {
                /* silent — polling retries automatically */
            },
        });
    }

    startPolling(intervalMs = 30_000): void {
        this.load();
        this.pollInterval = setInterval(() => this.load(), intervalMs);
        this.startRealtimeConnection();
    }

    stopPolling(): void {
        if (this.pollInterval !== null) {
            clearInterval(this.pollInterval);
            this.pollInterval = null;
        }
        this.stopRealtimeConnection();
    }

    private startRealtimeConnection(): void {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(environment.notificationHubUrl, {
                accessTokenFactory: () => localStorage.getItem('jalsa_token') ?? '',
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Warning)
            .build();

        this.hubConnection.on('ReceiveNotification', (notification: InAppNotification) => {
            this.notificationsSignal.update(list => [notification, ...list]);
            this.unreadCountSignal.update(c => c + 1);
            this.newNotificationSignal.set(notification);
        });

        this.hubConnection.start().catch(() => {
            /* silent — 30s polling remains the source of truth if the socket never connects */
        });
    }

    private stopRealtimeConnection(): void {
        if (this.hubConnection) {
            this.hubConnection.stop().catch(() => {});
            this.hubConnection = null;
        }
    }

    markRead(id: string): void {
        this.notificationsSignal.update(list => list.map(n => (n.id === id ? { ...n, isRead: true } : n)));
        this.unreadCountSignal.update(c => Math.max(0, c - 1));
        this.http.patch<{ id: string; isRead: boolean }>(API.notifications.markRead(id), {}).subscribe({
            error: () => this.load(),
        });
    }

    markAllRead(): void {
        this.notificationsSignal.update(list => list.map(n => ({ ...n, isRead: true })));
        this.unreadCountSignal.set(0);
        this.http.patch<{ markedRead: number }>(API.notifications.markAllRead, {}).subscribe({
            error: () => this.load(),
        });
    }
}
