import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { InAppNotificationService, InAppNotification } from './in-app-notification.service';
import { HttpClientService } from '../api/http-client.service';

const mockHubConnection = {
    on: vi.fn(),
    start: vi.fn().mockResolvedValue(undefined),
    stop: vi.fn().mockResolvedValue(undefined),
};

vi.mock('@microsoft/signalr', () => ({
    HubConnectionBuilder: vi.fn().mockImplementation(function (this: Record<string, unknown>) {
        this['withUrl'] = vi.fn().mockReturnThis();
        this['withAutomaticReconnect'] = vi.fn().mockReturnThis();
        this['build'] = vi.fn(() => mockHubConnection);
    }),
}));

const mockNotification: InAppNotification = {
    id: 'notif-1',
    type: 'ExerciseReminder',
    title: 'تذكير بالتمرين',
    body: 'يرجى إكمال التمرين',
    isRead: false,
    readAt: null,
    createdAt: '2026-07-08T10:00:00Z',
};

describe('InAppNotificationService', () => {
    let service: InAppNotificationService;
    let httpSpy: Record<string, ReturnType<typeof vi.fn>>;

    beforeEach(() => {
        vi.useFakeTimers();
        httpSpy = {
            get: vi.fn().mockReturnValue(of({ notifications: [mockNotification], unreadCount: 1 })),
            patch: vi.fn().mockReturnValue(of({})),
        };

        TestBed.configureTestingModule({
            providers: [{ provide: HttpClientService, useValue: httpSpy }],
        });

        service = TestBed.inject(InAppNotificationService);
    });

    afterEach(() => {
        service.stopPolling();
        mockHubConnection.on.mockClear();
        mockHubConnection.start.mockClear();
        mockHubConnection.stop.mockClear();
        vi.useRealTimers();
    });

    it('should load notifications and unread count', () => {
        service.load();
        expect(httpSpy['get']).toHaveBeenCalled();
        expect(service.notifications()).toEqual([mockNotification]);
        expect(service.unreadCount()).toBe(1);
    });

    it('should not throw when load fails', () => {
        httpSpy['get'].mockReturnValue(throwError(() => new Error('network error')));
        expect(() => service.load()).not.toThrow();
    });

    it('should start polling and open a SignalR connection', () => {
        service.startPolling();
        expect(httpSpy['get']).toHaveBeenCalledTimes(1);
        expect(mockHubConnection.start).toHaveBeenCalledTimes(1);
        expect(mockHubConnection.on).toHaveBeenCalledWith('ReceiveNotification', expect.any(Function));
    });

    it('should poll again after the interval elapses', () => {
        service.startPolling(30_000);
        expect(httpSpy['get']).toHaveBeenCalledTimes(1);
        vi.advanceTimersByTime(30_000);
        expect(httpSpy['get']).toHaveBeenCalledTimes(2);
    });

    it('should stop polling and the SignalR connection', () => {
        service.startPolling();
        service.stopPolling();
        expect(mockHubConnection.stop).toHaveBeenCalledTimes(1);

        vi.advanceTimersByTime(60_000);
        expect(httpSpy['get']).toHaveBeenCalledTimes(1);
    });

    it('should prepend a pushed notification and bump the unread count', () => {
        service.startPolling();
        const handler = mockHubConnection.on.mock.calls.find(call => call[0] === 'ReceiveNotification')?.[1];
        expect(handler).toBeDefined();

        const pushed: InAppNotification = { ...mockNotification, id: 'notif-2', title: 'إشعار جديد' };
        handler!(pushed);

        expect(service.notifications()[0]).toEqual(pushed);
        expect(service.unreadCount()).toBe(2);
        expect(service.newNotification()).toEqual(pushed);
    });

    it('should optimistically mark a notification as read', () => {
        service.load();
        service.markRead('notif-1');

        expect(service.notifications()[0].isRead).toBe(true);
        expect(service.unreadCount()).toBe(0);
        expect(httpSpy['patch']).toHaveBeenCalled();
    });

    it('should optimistically mark all notifications as read', () => {
        service.load();
        service.markAllRead();

        expect(service.notifications().every(n => n.isRead)).toBe(true);
        expect(service.unreadCount()).toBe(0);
        expect(httpSpy['patch']).toHaveBeenCalled();
    });
});
