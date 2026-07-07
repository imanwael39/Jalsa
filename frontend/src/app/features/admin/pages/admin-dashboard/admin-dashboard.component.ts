import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal, computed } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ChartDataset } from 'chart.js';
import { AdminService } from '../../../../core/services/admin.service';
import { AdminDashboardStateService } from '../../../../core/state/admin-dashboard-state.service';
import { AdminDashboardSummary, SystemHealth } from '../../../../core/models';
import { StatsCardComponent } from '../../../../shared/components/stats-card/stats-card.component';
import { BarChartComponent } from '../../../../shared/components/bar-chart/bar-chart.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-admin-dashboard',
    standalone: true,
    imports: [StatsCardComponent, BarChartComponent, SpinnerComponent],
    templateUrl: './admin-dashboard.component.html',
    styleUrl: './admin-dashboard.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminDashboardComponent implements OnInit {
    private adminService = inject(AdminService);
    private state = inject(AdminDashboardStateService);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    summary = this.state.summary;
    loading = this.state.loading;
    error = this.state.error;

    systemHealth = signal<SystemHealth | null>(null);

    roleLabels = ['مدراء', 'معالجون', 'مرضى'];
    roleDatasets = computed<ChartDataset<'bar'>[]>(() => {
        const s = this.summary();
        if (!s) return [];
        return [
            {
                label: 'عدد المستخدمين',
                data: [s.totalAdmins, s.totalTherapists, s.totalPatients],
                backgroundColor: ['#2563eb', '#16a34a', '#f59e0b'],
            },
        ];
    });

    ngOnInit(): void {
        this.load();
    }

    load(): void {
        this.state.setLoading(true);
        this.adminService
            .getDashboard()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: (summary: AdminDashboardSummary) => {
                    this.state.setSummary(summary);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.message || 'فشل تحميل لوحة تحكم النظام.');
                    this.state.setLoading(false);
                },
            });

        this.adminService
            .getSystemHealth()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: health => this.systemHealth.set(health),
                error: () => this.systemHealth.set(null),
            });
    }

    formatDate(date: string): string {
        return new Date(date).toLocaleDateString('ar-EG', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
        });
    }

    goToUsers(): void {
        this.router.navigate(['/admin/users']);
    }
}
