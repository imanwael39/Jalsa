import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { HeaderComponent } from './header.component';
import { AuthService } from '../../../core/services/auth.service';
import { InAppNotificationService, InAppNotification } from '../../../core/services/in-app-notification.service';
import { NotificationService as ToastService } from '../../../core/services/notification.service';
import { AppStateService } from '../../../core/services/app-state.service';

const mockNotification: InAppNotification = {
    id: 'notif-1',
    type: 'ExerciseReminder',
    title: 'تذكير بالتمرين',
    body: 'يرجى إكمال التمرين',
    isRead: false,
    readAt: null,
    createdAt: '2026-07-08T10:00:00Z',
};

describe('HeaderComponent', () => {
    let fixture: ComponentFixture<HeaderComponent>;
    let notifServiceSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let toastSpy: Record<string, ReturnType<typeof vi.fn>>;
    let newNotificationSignal: ReturnType<typeof signal<InAppNotification | null>>;

    const setup = (): void => {
        newNotificationSignal = signal<InAppNotification | null>(null);
        notifServiceSpy = {
            notifications: signal([]),
            unreadCount: signal(0),
            newNotification: newNotificationSignal,
            load: vi.fn(),
            markRead: vi.fn(),
            markAllRead: vi.fn(),
            startPolling: vi.fn(),
            stopPolling: vi.fn(),
        };
        toastSpy = { success: vi.fn(), error: vi.fn(), warning: vi.fn(), info: vi.fn(), show: vi.fn() };

        TestBed.configureTestingModule({
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                {
                    provide: AuthService,
                    useValue: {
                        currentUser: signal({ firstName: 'أحمد', lastName: 'السيد', roles: ['Therapist'] }),
                        resolveAvatarUrl: () => null,
                        isAuthenticated: () => false,
                        hasRole: () => false,
                    },
                },
                { provide: InAppNotificationService, useValue: notifServiceSpy },
                { provide: ToastService, useValue: toastSpy },
                { provide: AppStateService, useValue: { theme: signal('light'), toggleTheme: vi.fn() } },
                { provide: Router, useValue: { navigate: vi.fn() } },
            ],
        });

        fixture = TestBed.createComponent(HeaderComponent);
    };

    it('should create', (): void => {
        setup();
        fixture.detectChanges();
        expect(fixture.componentInstance).toBeTruthy();
    });

    it('should show an info toast for a non-crisis pushed notification', (): void => {
        setup();
        fixture.detectChanges();

        newNotificationSignal.set(mockNotification);
        fixture.detectChanges();

        expect(toastSpy['info']).toHaveBeenCalledWith('تذكير بالتمرين: يرجى إكمال التمرين');
        expect(toastSpy['warning']).not.toHaveBeenCalled();
    });

    it('should show a warning toast for a pushed CrisisAlert notification', (): void => {
        setup();
        fixture.detectChanges();

        newNotificationSignal.set({ ...mockNotification, type: 'CrisisAlert', title: 'تنبيه أزمة', body: null });
        fixture.detectChanges();

        expect(toastSpy['warning']).toHaveBeenCalledWith('تنبيه أزمة');
        expect(toastSpy['info']).not.toHaveBeenCalled();
    });

    it('should not show a toast when there is no new notification', (): void => {
        setup();
        fixture.detectChanges();

        expect(toastSpy['info']).not.toHaveBeenCalled();
        expect(toastSpy['warning']).not.toHaveBeenCalled();
    });
});
