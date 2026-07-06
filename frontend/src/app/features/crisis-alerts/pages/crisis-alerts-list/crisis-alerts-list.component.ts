import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpClientService } from '../../../../core/api/http-client.service';
import { API } from '../../../../core/api/api-endpoints';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { CrisisAlert } from '../../../../core/models';

@Component({
    selector: 'app-crisis-alerts-list',
    standalone: true,
    imports: [SpinnerComponent, EmptyStateComponent],
    templateUrl: './crisis-alerts-list.component.html',
    styleUrl: './crisis-alerts-list.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CrisisAlertsListComponent implements OnInit {
    private http = inject(HttpClientService);
    private destroyRef = inject(DestroyRef);

    alerts = signal<CrisisAlert[]>([]);
    loading = signal<boolean>(false);
    error = signal<string | null>(null);
    resolvingId = signal<string | null>(null);
    openOnly = signal<boolean>(true);

    ngOnInit(): void {
        this.loadAlerts();
    }

    loadAlerts(): void {
        this.loading.set(true);
        this.error.set(null);

        this.http
            .get<CrisisAlert[]>(`${API.crisisAlerts.base}?openOnly=${this.openOnly()}`)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.alerts.set(data);
                    this.loading.set(false);
                },
                error: () => {
                    this.error.set('فشل تحميل التنبيهات. يرجى المحاولة مرة أخرى.');
                    this.loading.set(false);
                },
            });
    }

    toggleOpenOnly(): void {
        this.openOnly.update(v => !v);
        this.loadAlerts();
    }

    resolve(id: string): void {
        this.resolvingId.set(id);
        this.http
            .patch<CrisisAlert>(API.crisisAlerts.resolve(id), {})
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.alerts.update(list => list.filter(a => a.id !== id));
                    this.resolvingId.set(null);
                },
                error: () => {
                    this.error.set('فشل تحديث التنبيه. يرجى المحاولة مرة أخرى.');
                    this.resolvingId.set(null);
                },
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
}
