import { Injectable, signal } from '@angular/core';

export interface Notification {
    id: string;
    message: string;
    type: 'success' | 'error' | 'warning' | 'info';
    duration?: number;
}

@Injectable({ providedIn: 'root' })
export class NotificationService {
    private notificationsSignal = signal<Notification[]>([]);
    readonly notifications = this.notificationsSignal.asReadonly();

    show(message: string, type: Notification['type'] = 'info', duration: number = 5000) {
        const id = Date.now().toString(36) + Math.random().toString(36).slice(2);
        const notification: Notification = { id, message, type, duration };
        this.notificationsSignal.update(list => [...list, notification]);

        if (duration > 0) {
            setTimeout(() => this.dismiss(id), duration);
        }
    }

    success(message: string, duration?: number) {
        this.show(message, 'success', duration);
    }

    error(message: string, duration?: number) {
        this.show(message, 'error', duration);
    }

    warning(message: string, duration?: number) {
        this.show(message, 'warning', duration);
    }

    info(message: string, duration?: number) {
        this.show(message, 'info', duration);
    }

    dismiss(id: string) {
        this.notificationsSignal.update(list => list.filter(n => n.id !== id));
    }

    clear() {
        this.notificationsSignal.set([]);
    }
}
