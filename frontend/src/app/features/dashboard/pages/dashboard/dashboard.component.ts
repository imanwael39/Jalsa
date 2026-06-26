import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DashboardService } from '../../../../core/services/dashboard.service';
import { DashboardStateService } from '../../../../core/state/dashboard-state.service';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [SpinnerComponent],
    templateUrl: './dashboard.component.html',
    styleUrl: './dashboard.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent implements OnInit {
    private dashboardService = inject(DashboardService);
    private state = inject(DashboardStateService);
    private destroyRef = inject(DestroyRef);

    summary = this.state.summary;
    loading = this.state.loading;
    error = this.state.error;

    ngOnInit(): void {
        this.loadDashboard();
    }

    loadDashboard(): void {
        this.state.setLoading(true);
        this.dashboardService.getDashboardSummary().pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe({
            next: (data) => {
                this.state.setSummary(data);
                this.state.setLoading(false);
            },
            error: (err) => {
                this.state.setError(err.message || 'Failed to load dashboard data');
                this.state.setLoading(false);
            },
        });
    }
}
