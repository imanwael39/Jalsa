import { TestBed } from '@angular/core/testing';
import { AppStateService } from './app-state.service';

describe('AppStateService', () => {
    let service: AppStateService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(AppStateService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should start with light theme', () => {
        expect(service.theme()).toBe('light');
    });

    it('should toggle theme from light to dark', () => {
        service.toggleTheme();
        expect(service.theme()).toBe('dark');
    });

    it('should toggle theme from dark to light', () => {
        service.toggleTheme();
        service.toggleTheme();
        expect(service.theme()).toBe('light');
    });

    it('should start with sidebar expanded', () => {
        expect(service.sidebarCollapsed()).toBe(false);
    });

    it('should toggle sidebar collapsed state', () => {
        service.toggleSidebar();
        expect(service.sidebarCollapsed()).toBe(true);
        service.toggleSidebar();
        expect(service.sidebarCollapsed()).toBe(false);
    });

    it('should set sidebar collapsed explicitly', () => {
        service.setSidebarCollapsed(true);
        expect(service.sidebarCollapsed()).toBe(true);
        service.setSidebarCollapsed(false);
        expect(service.sidebarCollapsed()).toBe(false);
    });
});
