import { Component, ChangeDetectionStrategy, DestroyRef, computed, inject, signal, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PatientAssessmentService } from '../../../../core/services/patient-assessment.service';
import { PatientAssessmentStateService } from '../../../../core/state/patient-assessment-state.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

const LIKERT_OPTIONS = [
    { value: 0, label: 'أبداً' },
    { value: 1, label: 'عدة أيام' },
    { value: 2, label: 'أكثر من نصف الأيام' },
    { value: 3, label: 'كل يوم تقريباً' },
];

@Component({
    selector: 'app-assessment-detail',
    standalone: true,
    imports: [DatePipe, ButtonComponent, StatusArPipe],
    templateUrl: './assessment-detail.component.html',
    styleUrl: './assessment-detail.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AssessmentDetailComponent implements OnInit {
    private patientAssessmentService = inject(PatientAssessmentService);
    private state = inject(PatientAssessmentStateService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    likertOptions = LIKERT_OPTIONS;

    detail = this.state.detail;
    loading = this.state.loading;
    saving = this.state.saving;
    error = this.state.error;

    submitError = signal<string | null>(null);
    savingQuestionId = signal<string | null>(null);

    isAssigned = computed(() => this.detail()?.status === 'Assigned');
    allAnswered = computed(() => {
        const d = this.detail();
        if (!d) return false;
        return d.questions.every(q => q.answerNumber !== null && q.answerNumber !== undefined);
    });
    answeredCount = computed(() => this.detail()?.questions.filter(q => q.answerNumber !== null).length ?? 0);
    totalCount = computed(() => this.detail()?.questions.length ?? 0);

    private assessmentId = '';

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (!id) {
            this.state.setError('معرف التقييم مطلوب');
            return;
        }
        this.assessmentId = id;
        this.loadDetail();
    }

    loadDetail(): void {
        this.state.setLoading(true);
        this.patientAssessmentService
            .getDetail(this.assessmentId)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.state.setDetail(data);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.error || err.error?.message || 'فشل تحميل التقييم');
                    this.state.setLoading(false);
                },
            });
    }

    selectAnswer(questionId: string, value: number): void {
        const current = this.detail();
        if (!current) return;

        // Optimistic local update so the UI feels instant while the save request is in flight.
        this.state.setDetail({
            ...current,
            questions: current.questions.map(q => (q.id === questionId ? { ...q, answerNumber: value } : q)),
        });

        this.savingQuestionId.set(questionId);
        this.patientAssessmentService
            .saveAnswer(this.assessmentId, questionId, { answerNumber: value })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    if (this.savingQuestionId() === questionId) this.savingQuestionId.set(null);
                },
                error: (err: HttpErrorResponse) => {
                    this.savingQuestionId.set(null);
                    this.submitError.set(err.error?.error || err.error?.message || 'فشل حفظ الإجابة');
                },
            });
    }

    submit(): void {
        if (!this.allAnswered()) return;

        this.submitError.set(null);
        this.state.setSaving(true);
        this.patientAssessmentService
            .submit(this.assessmentId)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.state.setDetail(data);
                    this.state.setSaving(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.submitError.set(err.error?.error || err.error?.message || 'فشل إرسال التقييم');
                    this.state.setSaving(false);
                },
            });
    }

    goBack(): void {
        this.router.navigate(['/assessments']);
    }
}
