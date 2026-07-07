import { ChangeDetectionStrategy, Component, input } from '@angular/core';

export type StatsCardColor = 'primary' | 'success' | 'danger' | 'warning' | 'info';

@Component({
    selector: 'app-stats-card',
    standalone: true,
    templateUrl: './stats-card.component.html',
    styleUrls: ['./stats-card.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StatsCardComponent {
    label = input('');
    value = input<number | string>(0);
    icon = input('');
    color = input<StatsCardColor>('primary');
    trend = input<number>();
    trendLabel = input('');
}
