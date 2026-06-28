import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'app-empty-state',
    templateUrl: './empty-state.component.html',
    styleUrl: './empty-state.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EmptyStateComponent {
    @Input() title = 'لا توجد بيانات';
    @Input() message = '';
    @Input() icon = 'bi-inbox';
    @Input() image = '';
    @Input() actionLabel = '';
    @Input() actionDisabled = false;

    @Output() action = new EventEmitter<void>();

    onActionClick(): void {
        if (!this.actionDisabled) {
            this.action.emit();
        }
    }
}
