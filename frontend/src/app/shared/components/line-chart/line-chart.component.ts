import {
    AfterViewInit,
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
    selector: 'app-line-chart',
    standalone: true,
    templateUrl: './line-chart.component.html',
    styleUrls: ['./line-chart.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LineChartComponent implements OnChanges, AfterViewInit, OnDestroy {
    @Input() title = '';
    @Input() labels: string[] = [];
    @Input() datasets: ChartDataset<'line'>[] = [];
    @Input() loading = false;
    @Input() height = '250px';

    @ViewChild('chartCanvas') chartCanvas!: ElementRef<HTMLCanvasElement>;
    private chart: Chart | null = null;
    // ngOnChanges fires before the view (and @ViewChild) exists, so an initial
    // input already carrying data would otherwise crash on chartCanvas being
    // undefined. Defer any render request that arrives before ngAfterViewInit.
    private viewReady = false;

    ngOnChanges(changes: SimpleChanges): void {
        if (this.viewReady && this.hasRenderableData(changes)) {
            this.renderChart();
        }
    }

    ngAfterViewInit(): void {
        this.viewReady = true;
        if (this.datasets.length > 0 && this.labels.length > 0) {
            this.renderChart();
        }
    }

    ngOnDestroy(): void {
        this.destroyChart();
    }

    private hasRenderableData(changes: SimpleChanges): boolean {
        return !!(changes['datasets'] || changes['labels']) && this.datasets.length > 0 && this.labels.length > 0;
    }

    private renderChart(): void {
        this.destroyChart();

        const config: ChartConfiguration<'line'> = {
            type: 'line',
            data: {
                labels: this.labels,
                datasets: this.datasets.map(ds => ({
                    ...ds,
                    tension: 0.3,
                    pointRadius: 4,
                    pointHoverRadius: 6,
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
                    tooltip: {
                        mode: 'index',
                        intersect: false,
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
