import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { NotificationListComponent } from './notification-list.component';
import { InAppNotificationService, InAppNotification } from '../../../../core/services/in-app-notification.service';

const mockNotifications: InAppNotification[] = [
    {
        id: 'notif-1',
        type: 'ExerciseReminder',
        title: 'تذكير بالتمرين',
        body: 'يرجى إكمال التمرين',
        isRead: false,
        readAt: null,
        createdAt: '2026-07-08T10:00:00Z',
    },
    {
        id: 'notif-2',
        type: 'CrisisAlert',
        title: 'تنبيه أزمة',
        body: 'المريض: ...',
        isRead: true,
        readAt: '2026-07-08T11:00:00Z',
        createdAt: '2026-07-08T09:00:00Z',
    },
];

describe('NotificationListComponent', () => {
    let component: NotificationListComponent;
    let fixture: ComponentFixture<NotificationListComponent>;
    let notifServiceSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (): void => {
        notifServiceSpy = {
            notifications: signal(mockNotifications),
            unreadCount: signal(1),
            newNotification: signal(null),
            load: vi.fn(),
            markRead: vi.fn(),
            markAllRead: vi.fn(),
            startPolling: vi.fn(),
            stopPolling: vi.fn(),
        };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: InAppNotificationService, useValue: notifServiceSpy },
                { provide: Router, useValue: routerSpy },
            ],
        });

        fixture = TestBed.createComponent(NotificationListComponent);
        component = fixture.componentInstance;
    };

    it('should create and load notifications on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(notifServiceSpy['load']).toHaveBeenCalled();
    });

    it('should show all notifications by default', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.filteredNotifications()).toHaveLength(2);
    });

    it('should filter to unread-only when toggled', (): void => {
        setup();
        fixture.detectChanges();
        component.toggleUnreadOnly();
        expect(component.showUnreadOnly()).toBe(true);
        expect(component.filteredNotifications()).toEqual([mockNotifications[0]]);
    });

    it('should toggle back to showing all notifications', (): void => {
        setup();
        fixture.detectChanges();
        component.toggleUnreadOnly();
        component.toggleUnreadOnly();
        expect(component.showUnreadOnly()).toBe(false);
        expect(component.filteredNotifications()).toHaveLength(2);
    });

    it('should mark all as read', (): void => {
        setup();
        fixture.detectChanges();
        component.markAllRead();
        expect(notifServiceSpy['markAllRead']).toHaveBeenCalled();
    });

    it('should mark a notification as read on click', (): void => {
        setup();
        fixture.detectChanges();
        component.onNotificationClick(mockNotifications[0]);
        expect(notifServiceSpy['markRead']).toHaveBeenCalledWith('notif-1');
        expect(routerSpy['navigate']).not.toHaveBeenCalled();
    });

    it('should navigate to crisis-alerts when a CrisisAlert notification is clicked', (): void => {
        setup();
        fixture.detectChanges();
        component.onNotificationClick(mockNotifications[1]);
        expect(notifServiceSpy['markRead']).toHaveBeenCalledWith('notif-2');
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/crisis-alerts']);
    });

    it('should resolve the correct icon class per notification type', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.iconClass('CrisisAlert')).toBe('bi-exclamation-triangle-fill');
        expect(component.iconClass('ExerciseReminder')).toBe('bi-clipboard-check');
        expect(component.iconClass('Other')).toBe('bi-bell');
    });
});
