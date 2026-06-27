import { Component, ChangeDetectionStrategy, inject, signal, input, OnInit, DestroyRef } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PatientService } from '../../../../core/services/patient.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Assessment as AssessmentModel } from '../../../../core/models';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-assessment',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, SpinnerComponent, DatePipe],
    templateUrl: './assessment.html',
    styleUrl: './assessment.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Assessment implements OnInit {
    private fb = inject(FormBuilder);
    private patientService = inject(PatientService);
    private notification = inject(NotificationService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    patientIdInput = input<string | null>(null);

    patientId = signal<string>('');
    assessments = signal<AssessmentModel[]>([]);
    loading = signal(false);
    error = signal<string | null>(null);
    submitLoading = signal(false);

    form = this.fb.group({
        templateId: ['', [Validators.required]],
        title: [''],
        totalScore: [null as number | null, [Validators.required, Validators.min(0)]],
        assessmentDate: [this.getCurrentDate(), [Validators.required]],
    });

    assessmentTypes = [
        { value: 'phq-9', label: 'PHQ-9 (Depression)' },
        { value: 'gad-7', label: 'GAD-7 (Anxiety)' },
        { value: 'dass-21', label: 'DASS-21 (Stress)' },
        { value: 'pcl-5', label: 'PCL-5 (PTSD)' },
        { value: 'other', label: 'Other' },
    ];

    ngOnInit(): void {
        const inputId = this.patientIdInput();
        const id = inputId || this.route.snapshot.paramMap.get('id');
        if (id) {
            this.patientId.set(id);
            this.loadAssessments();
        } else {
            this.error.set('Patient ID required');
        }
    }

    loadAssessments(): void {
        this.loading.set(true);
        this.error.set(null);
        this.patientService
            .getAssessments(this.patientId())
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.assessments.set(data);
                    this.loading.set(false);
                },
                error: err => {
                    this.error.set(err?.message || 'Failed to load assessments');
                    this.loading.set(false);
                },
            });
    }

    onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.submitLoading.set(true);
        this.error.set(null);

        const formValue = this.form.value;
        const assessmentData: Partial<AssessmentModel> = {
            templateId: formValue.templateId ?? '',
            title: formValue.title ?? this.getAssessmentLabel(formValue.templateId ?? ''),
            totalScore: formValue.totalScore ?? null,
            assessmentDate: formValue.assessmentDate ?? null,
            status: 'Completed',
        };

        this.patientService
            .addAssessment(this.patientId(), assessmentData)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.submitLoading.set(false);
                    this.notification.success('Assessment added successfully');
                    this.form.reset({
                        templateId: '',
                        title: '',
                        totalScore: null,
                        assessmentDate: this.getCurrentDate(),
                    });
                    this.loadAssessments();
                },
                error: err => {
                    this.error.set(err?.message || 'Failed to add assessment');
                    this.submitLoading.set(false);
                },
            });
    }

    goBack(): void {
        this.router.navigate(['/patients', this.patientId()]);
    }

    isFieldInvalid(fieldName: string): boolean {
        const field = this.form.get(fieldName);
        return field !== null && field.invalid && field.touched;
    }

    getFieldError(fieldName: string): string {
        const field = this.form.get(fieldName);
        if (!field || !field.errors || !field.touched) return '';

        if (field.errors['required']) {
            return `${this.getFieldLabel(fieldName)} is required`;
        }
        if (field.errors['min']) {
            return `${this.getFieldLabel(fieldName)} must be at least ${field.errors['min'].min}`;
        }
        return '';
    }

    private getFieldLabel(fieldName: string): string {
        const labels: Record<string, string> = {
            templateId: 'Assessment type',
            title: 'Title',
            totalScore: 'Score',
            assessmentDate: 'Date',
        };
        return labels[fieldName] || fieldName;
    }

    private getAssessmentLabel(templateId: string): string {
        const type = this.assessmentTypes.find(t => t.value === templateId);
        return type?.label ?? templateId;
    }

    private getCurrentDate(): string {
        return new Date().toISOString().split('T')[0];
    }

    trackByAssessmentId(index: number, assessment: AssessmentModel): string {
        return assessment.id;
    }
}
