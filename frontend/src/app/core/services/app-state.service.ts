import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AppStateService {
    private themeSignal = signal<'light' | 'dark'>('light');
    readonly theme = this.themeSignal.asReadonly();

    private sidebarCollapsedSignal = signal<boolean>(false);
    readonly sidebarCollapsed = this.sidebarCollapsedSignal.asReadonly();

    toggleTheme() {
        this.themeSignal.update(current => current === 'light' ? 'dark' : 'light');
    }

    toggleSidebar() {
        this.sidebarCollapsedSignal.update(collapsed => !collapsed);
    }

    setSidebarCollapsed(collapsed: boolean) {
        this.sidebarCollapsedSignal.set(collapsed);
    }
}
