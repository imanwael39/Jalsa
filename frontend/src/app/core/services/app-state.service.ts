import { Injectable, signal, effect } from '@angular/core';

const THEME_STORAGE_KEY = 'jalsa_theme';

function restoreTheme(): 'light' | 'dark' {
    return localStorage.getItem(THEME_STORAGE_KEY) === 'dark' ? 'dark' : 'light';
}

@Injectable({ providedIn: 'root' })
export class AppStateService {
    private themeSignal = signal<'light' | 'dark'>(restoreTheme());
    readonly theme = this.themeSignal.asReadonly();

    private sidebarCollapsedSignal = signal<boolean>(false);
    readonly sidebarCollapsed = this.sidebarCollapsedSignal.asReadonly();

    constructor() {
        effect(() => {
            const theme = this.themeSignal();
            document.documentElement.setAttribute('data-theme', theme);
            localStorage.setItem(THEME_STORAGE_KEY, theme);
        });
    }

    toggleTheme(): void {
        this.themeSignal.update(current => (current === 'light' ? 'dark' : 'light'));
    }

    toggleSidebar(): void {
        this.sidebarCollapsedSignal.update(collapsed => !collapsed);
    }

    setSidebarCollapsed(collapsed: boolean): void {
        this.sidebarCollapsedSignal.set(collapsed);
    }
}
