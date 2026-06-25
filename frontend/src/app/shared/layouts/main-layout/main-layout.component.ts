import { Component, inject, ChangeDetectionStrategy } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppStateService } from '../../../core/services/app-state.service';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { HeaderComponent } from '../header/header.component';
import { FooterComponent } from '../footer/footer.component';

@Component({
    selector: 'app-main-layout',
    standalone: true,
    imports: [RouterOutlet, SidebarComponent, HeaderComponent, FooterComponent],
    templateUrl: './main-layout.component.html',
    styleUrls: ['./main-layout.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MainLayoutComponent {
    private appState = inject(AppStateService);

    sidebarCollapsed = this.appState.sidebarCollapsed;

    toggleSidebar(): void {
        this.appState.toggleSidebar();
    }
}
