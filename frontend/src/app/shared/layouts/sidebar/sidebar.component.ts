import { Component, Output, EventEmitter, inject, ChangeDetectionStrategy, input } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NavigationService } from '../../../core/services/navigation.service';
import { AppStateService } from '../../../core/services/app-state.service';

@Component({
    selector: 'app-sidebar',
    standalone: true,
    imports: [RouterLink, RouterLinkActive],
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent {
    private readonly navService = inject(NavigationService);
    private readonly appState = inject(AppStateService);

    readonly collapsed = input(false);
    @Output() toggle = new EventEmitter<void>();

    readonly menuItems = this.navService.menuItems;

    toggleSidebar(): void {
        this.toggle.emit();
    }
    
}
