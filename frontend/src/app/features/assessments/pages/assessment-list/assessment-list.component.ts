import { Component, ChangeDetectionStrategy, DestroyRef, computed, inject, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PatientAssessmentService } from '../../../../core/services/patient-assessment.service';
import { PatientAssessmentStateService } from '../../../../core/state/patient-assessment-state.service';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

@Component({
    selector: 'app-assessment-list',
    standalone: true,
    imports: [RouterLink, DatePipe, EmptyStateComponent, StatusArPipe],
    templateUrl: './assessment-list.component.html',
    styleUrl: './assessment-list.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AssessmentListComponent implements OnInit {
    private patientAssessmentService = inject(PatientAssessmentService);
    private state = inject(PatientAssessmentStateService);
    private destroyRef = inject(DestroyRef);

    list = this.state.list;
    loading = this.state.loading;
    error = this.state.error;

    pending = computed(() => this.list().filter(a => a.status === 'Assigned'));
    completed = computed(() => this.list().filter(a => a.status === 'Completed'));

    ngOnInit(): void {
        this.loadList();
    }

    loadList(): void {
        this.state.setLoading(true);
        this.patientAssessmentService
            .getList()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.state.setList(data);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.error || err.error?.message || 'فشل تحميل التقييمات');
                    this.state.setLoading(false);
                },
            });
    }
}
