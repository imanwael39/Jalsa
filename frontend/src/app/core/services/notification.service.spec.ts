import { TestBed } from '@angular/core/testing';
import { NotificationService } from './notification.service';

describe('NotificationService', () => {
    let service: NotificationService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(NotificationService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should start with no notifications', () => {
        expect(service.notifications().length).toBe(0);
    });

    it('should add a notification on success()', () => {
        service.success('Operation completed');
        expect(service.notifications().length).toBe(1);
        const n = service.notifications()[0];
        expect(n.message).toBe('Operation completed');
        expect(n.type).toBe('success');
    });

    it('should add a notification on error()', () => {
        service.error('Something went wrong');
        expect(service.notifications().length).toBe(1);
        expect(service.notifications()[0].type).toBe('error');
    });

    it('should add a notification on warning()', () => {
        service.warning('Be careful');
        expect(service.notifications().length).toBe(1);
        expect(service.notifications()[0].type).toBe('warning');
    });

    it('should add a notification on info()', () => {
        service.info('Just so you know');
        expect(service.notifications().length).toBe(1);
        expect(service.notifications()[0].type).toBe('info');
    });

    it('should dismiss a notification by id', () => {
        service.info('Test');
        const id = service.notifications()[0].id;
        service.dismiss(id);
        expect(service.notifications().length).toBe(0);
    });

    it('should clear all notifications', () => {
        service.success('One');
        service.error('Two');
        service.warning('Three');
        expect(service.notifications().length).toBe(3);
        service.clear();
        expect(service.notifications().length).toBe(0);
    });

    it('should auto-dismiss after duration', async () => {
        vi.useFakeTimers();
        service.show('Quick', 'info', 100);
        expect(service.notifications().length).toBe(1);
        vi.advanceTimersByTime(100);
        expect(service.notifications().length).toBe(0);
        vi.useRealTimers();
    });

    it('should generate unique ids for each notification', () => {
        service.success('A');
        service.success('B');
        const ids = service.notifications().map(n => n.id);
        expect(ids[0]).not.toBe(ids[1]);
    });
});
