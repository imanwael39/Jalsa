import { Component, ChangeDetectionStrategy, input, signal, inject, OnInit, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SessionService } from '../../../../core/services/session.service';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-summary',
    standalone: true,
    imports: [SpinnerComponent],
    templateUrl: './summary.html',
    styleUrl: './summary.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Summary implements OnInit {
    sessionId = input.required<string>();

    private sessionService = inject(SessionService);
    private destroyRef = inject(DestroyRef);

    loading = signal(true);
    summary = signal<string | null>(null);
    error = signal<string | null>(null);

    ngOnInit(): void {
        if (this.sessionId()) {
            this.loadSummary();
        }
    }

    loadSummary(): void {
        this.loading.set(true);
        this.sessionService
            .getSummary(this.sessionId())
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: result => {
                    this.summary.set(result.summary);
                    this.loading.set(false);
                },
                error: err => {
                    this.error.set(err.message || 'فشل تحميل الملخص');
                    this.loading.set(false);
                },
            });
    }
}
