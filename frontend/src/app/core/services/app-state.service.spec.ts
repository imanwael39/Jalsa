import { TestBed } from '@angular/core/testing';
import { AppStateService } from './app-state.service';

describe('AppStateService', () => {
    let service: AppStateService;

    beforeEach(() => {
        localStorage.clear();
        document.documentElement.removeAttribute('data-theme');
        TestBed.configureTestingModule({});
        service = TestBed.inject(AppStateService);
    });

    afterEach(() => {
        localStorage.clear();
        document.documentElement.removeAttribute('data-theme');
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should start with light theme', () => {
        expect(service.theme()).toBe('light');
    });

    it('should restore a previously persisted dark theme', () => {
        localStorage.setItem('jalsa_theme', 'dark');
        TestBed.resetTestingModule();
        TestBed.configureTestingModule({});
        expect(TestBed.inject(AppStateService).theme()).toBe('dark');
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

    it('should apply the theme as a data-theme attribute on the document', () => {
        service.toggleTheme();
        TestBed.flushEffects();
        expect(document.documentElement.getAttribute('data-theme')).toBe('dark');
    });

    it('should persist the theme choice to localStorage', () => {
        service.toggleTheme();
        TestBed.flushEffects();
        expect(localStorage.getItem('jalsa_theme')).toBe('dark');
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
