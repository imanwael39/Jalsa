import { Component, ChangeDetectionStrategy, inject, signal, OnInit, DestroyRef } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PatientService } from '../../../../core/services/patient.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { IntakeForm as IntakeFormModel } from '../../../../core/models';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-intake-form',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, SpinnerComponent],
    templateUrl: './intake-form.html',
    styleUrl: './intake-form.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class IntakeForm implements OnInit {
    private fb = inject(FormBuilder);
    private patientService = inject(PatientService);
    private notification = inject(NotificationService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    patientId = signal<string>('');
    loading = signal(false);
    error = signal<string | null>(null);
    ocrLoading = signal(false);
    ocrSuccess = signal(false);
    selectedFileName = signal<string | null>(null);

    form = this.fb.group({
        presentingProblem: [''],
        psychiatricHistory: [''],
        familyHistory: [''],
        medications: [''],
        socialHistory: [''],
    });

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.patientId.set(id);
            this.loadIntakeForm();
        } else {
            this.error.set('معرف المريض مطلوب');
        }
    }

    loadIntakeForm(): void {
        this.loading.set(true);
        this.error.set(null);
        this.patientService
            .getIntakeForm(this.patientId())
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: (intakeForm: IntakeFormModel) => {
                    this.form.patchValue({
                        presentingProblem: intakeForm.presentingProblem ?? '',
                        psychiatricHistory: intakeForm.psychiatricHistory ?? '',
                        familyHistory: intakeForm.familyHistory ?? '',
                        medications: intakeForm.medications ?? '',
                        socialHistory: intakeForm.socialHistory ?? '',
                    });
                    this.loading.set(false);
                },
                error: () => {
                    this.loading.set(false);
                },
            });
    }

    onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            const file = input.files[0];
            this.selectedFileName.set(file.name);
            this.uploadImage(file);
        }
    }

    uploadImage(file: File): void {
        this.ocrLoading.set(true);
        this.ocrSuccess.set(false);
        this.error.set(null);
        this.patientService
            .uploadIntakeImage(this.patientId(), file)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: result => {
                    this.ocrLoading.set(false);
                    this.ocrSuccess.set(true);
                    if (result.extractedData) {
                        this.form.patchValue({
                            presentingProblem:
                                result.extractedData['presentingProblem'] ??
                                result.extractedData['reasonForVisit'] ??
                                '',
                            psychiatricHistory:
                                result.extractedData['psychiatricHistory'] ??
                                result.extractedData['medicalHistory'] ??
                                '',
                            familyHistory: result.extractedData['familyHistory'] ?? '',
                            medications: result.extractedData['medications'] ?? '',
                            socialHistory:
                                result.extractedData['socialHistory'] ?? result.extractedData['otherInfo'] ?? '',
                        });
                        this.notification.success('تم استخراج البيانات بنجاح');
                    }
                },
                error: err => {
                    this.ocrLoading.set(false);
                    this.error.set(err?.message || 'فشل رفع الصورة');
                    this.notification.error('فشل استخراج البيانات. يرجى إدخال البيانات يدوياً.');
                },
            });
    }

    onSubmit(): void {
        this.loading.set(true);
        this.error.set(null);
        this.patientService
            .saveIntakeForm(this.patientId(), this.form.value)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.loading.set(false);
                    this.notification.success('تم حفظ استمارة الاستقبال بنجاح');
                    this.router.navigate(['/patients', this.patientId()]);
                },
                error: err => {
                    this.error.set(err?.message || 'فشل حفظ استمارة الاستقبال');
                    this.loading.set(false);
                },
            });
    }

    goBack(): void {
        this.router.navigate(['/patients', this.patientId()]);
    }

    clearFileSelection(): void {
        this.selectedFileName.set(null);
        this.ocrSuccess.set(false);
    }
}
