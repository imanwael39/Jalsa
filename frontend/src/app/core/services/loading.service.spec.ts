import { TestBed } from '@angular/core/testing';
import { LoadingService } from './loading.service';

describe('LoadingService', () => {
    let service: LoadingService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(LoadingService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should start with loading = false', () => {
        expect(service.isLoading()).toBe(false);
    });

    it('should set loading to true on show()', () => {
        service.show();
        expect(service.isLoading()).toBe(true);
    });

    it('should set loading to false on hide()', () => {
        service.show();
        service.hide();
        expect(service.isLoading()).toBe(false);
    });

    it('should handle multiple show/hide calls correctly', () => {
        service.show();
        service.show();
        expect(service.isLoading()).toBe(true);
        service.hide();
        expect(service.isLoading()).toBe(true);
        service.hide();
        expect(service.isLoading()).toBe(false);
    });

    it('should not go negative on hide() when not loading', () => {
        service.hide();
        expect(service.isLoading()).toBe(false);
        service.show();
        service.hide();
        expect(service.isLoading()).toBe(false);
    });

    it('should reset to initial state', () => {
        service.show();
        service.show();
        expect(service.isLoading()).toBe(true);
        service.reset();
        expect(service.isLoading()).toBe(false);
        service.hide();
        expect(service.isLoading()).toBe(false);
    });
});
