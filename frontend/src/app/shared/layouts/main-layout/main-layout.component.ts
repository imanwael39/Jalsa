import { Component, inject, ChangeDetectionStrategy, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppStateService } from '../../../core/services/app-state.service';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { HeaderComponent } from '../header/header.component';
import { FooterComponent } from '../footer/footer.component';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';

@Component({
    selector: 'app-main-layout',
    standalone: true,
    imports: [RouterOutlet, SidebarComponent, HeaderComponent, FooterComponent, BreadcrumbComponent],
    templateUrl: './main-layout.component.html',
    styleUrls: ['./main-layout.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MainLayoutComponent {
    private readonly appState = inject(AppStateService);

    readonly sidebarCollapsed = this.appState.sidebarCollapsed;
    readonly mobileSidebarOpen = signal(false);

    toggleSidebar(): void {
        this.appState.toggleSidebar();
    }

    toggleMobileSidebar(): void {
        this.mobileSidebarOpen.update(open => !open);
    }

    closeMobileSidebar(): void {
        this.mobileSidebarOpen.set(false);
    }
}
