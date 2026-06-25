import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-sidebar',
    standalone: true,
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent {
    @Input() collapsed = false;
    @Output() toggle = new EventEmitter<void>();

    toggleSidebar(): void {
        this.toggle.emit();
    }
}
