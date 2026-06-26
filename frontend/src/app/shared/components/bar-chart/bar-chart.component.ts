import {
    ChangeDetectionStrategy,
    Component,
    ElementRef,
    Input,
    OnChanges,
    OnDestroy,
    SimpleChanges,
    ViewChild,
} from '@angular/core';
import { Chart, ChartConfiguration, ChartDataset, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
    selector: 'app-bar-chart',
    standalone: true,
    templateUrl: './bar-chart.component.html',
    styleUrls: ['./bar-chart.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BarChartComponent implements OnChanges, OnDestroy {
    @Input() title = '';
    @Input() labels: string[] = [];
    @Input() datasets: ChartDataset<'bar'>[] = [];
    @Input() loading = false;
    @Input() height = '250px';

    @ViewChild('chartCanvas') chartCanvas!: ElementRef<HTMLCanvasElement>;
    private chart: Chart | null = null;

    ngOnChanges(changes: SimpleChanges): void {
        if (
            (changes['datasets'] || changes['labels']) &&
            this.datasets.length > 0 &&
            this.labels.length > 0
        ) {
            this.renderChart();
        }
    }

    ngOnDestroy(): void {
        this.destroyChart();
    }

    private renderChart(): void {
        this.destroyChart();

        const config: ChartConfiguration<'bar'> = {
            type: 'bar',
            data: {
                labels: this.labels,
                datasets: this.datasets.map((ds) => ({
                    ...ds,
                    borderRadius: 4,
                })),
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: this.datasets.length > 1,
                        position: 'top',
                    },
                },
                scales: {
                    y: {
                        beginAtZero: true,
                    },
                },
            },
        };

        this.chart = new Chart(this.chartCanvas.nativeElement, config);
    }

    private destroyChart(): void {
        if (this.chart) {
            this.chart.destroy();
            this.chart = null;
        }
    }
}
