import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

export type StatsCardColor = 'primary' | 'success' | 'danger' | 'warning' | 'info';

@Component({
    selector: 'app-stats-card',
    standalone: true,
    templateUrl: './stats-card.component.html',
    styleUrls: ['./stats-card.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StatsCardComponent {
    @Input() label = '';
    @Input() value: number | string = 0;
    @Input() icon = '';
    @Input() color: StatsCardColor = 'primary';
    @Input() trend?: number;
    @Input() trendLabel = '';
}
