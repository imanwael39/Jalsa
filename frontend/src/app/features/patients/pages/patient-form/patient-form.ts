import { Component, ChangeDetectionStrategy, inject, signal, OnInit, DestroyRef } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PatientService } from '../../../../core/services/patient.service';
import { PatientStateService } from '../../../../core/state/patient-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-patient-form',
    standalone: true,
    imports: [ReactiveFormsModule, InputComponent, ButtonComponent, SpinnerComponent],
    templateUrl: './patient-form.html',
    styleUrl: './patient-form.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientForm implements OnInit {
    private fb = inject(FormBuilder);
    private patientService = inject(PatientService);
    private state = inject(PatientStateService);
    private notification = inject(NotificationService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    patientId = signal<string | null>(null);
    loading = signal(false);
    isEdit = signal(false);
    error = signal<string | null>(null);

    // Password is only required (and shown) when creating a patient with an email —
    // editing an existing patient never touches the password, so skip the check there.
    private emailRequiresPasswordValidator = (group: AbstractControl): ValidationErrors | null => {
        if (this.isEdit()) return null;
        const email = group.get('email')?.value;
        const password = group.get('password')?.value;
        return email && !password ? { passwordRequiredForEmail: true } : null;
    };

    form = this.fb.group(
        {
            fullName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(200)]],
            email: ['', [Validators.email, Validators.maxLength(200)]],
            password: ['', [Validators.minLength(8), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/)]],
            phone: ['', [Validators.maxLength(20)]],
            dateOfBirth: [''],
            gender: [''],
            address: ['', [Validators.maxLength(500)]],
            referralSource: ['', [Validators.maxLength(200)]],
            chiefComplaint: ['', [Validators.maxLength(1000)]],
        },
        { validators: this.emailRequiresPasswordValidator }
    );

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.patientId.set(id);
            this.isEdit.set(true);
            this.loadPatient(id);
        }
    }

    loadPatient(id: string): void {
        this.loading.set(true);
        this.error.set(null);
        this.patientService
            .getPatient(id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: patient => {
                    this.form.patchValue({
                        fullName: patient.fullName,
                        email: patient.email ?? '',
                        phone: patient.phone ?? '',
                        dateOfBirth: patient.dateOfBirth ?? '',
                        gender: patient.gender ?? '',
                        address: patient.address ?? '',
                        referralSource: patient.referralSource ?? '',
                        chiefComplaint: patient.chiefComplaint ?? '',
                    });
                    this.state.selectPatient(patient);
                    this.loading.set(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.message || err.error?.error || 'فشل تحميل بيانات المريض');
                    this.loading.set(false);
                },
            });
    }

    onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        this.error.set(null);

        const formValue = this.form.value;

        const createData = {
            fullName: formValue.fullName ?? '',
            dateOfBirth: formValue.dateOfBirth || null,
            gender: formValue.gender || null,
            phone: formValue.phone || null,
            email: formValue.email || null,
            password: formValue.password || null,
            address: formValue.address || null,
            referralSource: formValue.referralSource || null,
            chiefComplaint: formValue.chiefComplaint || null,
        };

        if (this.isEdit()) {
            const updateData = {
                fullName: formValue.fullName ?? '',
                dateOfBirth: formValue.dateOfBirth || null,
                gender: formValue.gender || null,
                phone: formValue.phone || null,
                email: formValue.email || null,
                address: formValue.address || null,
                referralSource: formValue.referralSource || null,
                chiefComplaint: formValue.chiefComplaint || null,
            };
            this.patientService
                .updatePatient(this.patientId()!, updateData)
                .pipe(takeUntilDestroyed(this.destroyRef))
                .subscribe({
                    next: patient => {
                        this.state.updatePatient(patient);
                        this.notification.success('تم تحديث بيانات المريض بنجاح');
                        this.loading.set(false);
                        this.router.navigate(['/patients', this.patientId()]);
                    },
                    error: (err: HttpErrorResponse) => {
                        this.error.set(err.error?.message || err.error?.error || 'فشل تحديث بيانات المريض');
                        this.loading.set(false);
                    },
                });
        } else {
            this.patientService
                .createPatient(createData)
                .pipe(takeUntilDestroyed(this.destroyRef))
                .subscribe({
                    next: patient => {
                        this.state.addPatient(patient);
                        this.loading.set(false);
                        this.notification.success(
                            patient.userId ? 'تم إنشاء المريض وحساب دخول خاص به بنجاح' : 'تم إنشاء المريض بنجاح'
                        );
                        this.router.navigate(['/patients', patient.id]);
                    },
                    error: (err: HttpErrorResponse) => {
                        this.error.set(err.error?.message || err.error?.error || 'فشل إنشاء المريض');
                        this.loading.set(false);
                    },
                });
        }
    }

    goBack(): void {
        this.router.navigate(['/patients']);
    }

    isFieldInvalid(fieldName: string): boolean {
        const field = this.form.get(fieldName);
        return field !== null && field.invalid && field.touched;
    }

    getFieldError(fieldName: string): string {
        const field = this.form.get(fieldName);
        if (!field || !field.errors || !field.touched) return '';

        if (field.errors['required']) {
            return `${this.getFieldLabel(fieldName)} مطلوب`;
        }
        if (field.errors['minlength']) {
            return `${this.getFieldLabel(fieldName)} يجب أن يكون ${field.errors['minlength'].requiredLength} أحرف على الأقل`;
        }
        if (field.errors['maxlength']) {
            return `${this.getFieldLabel(fieldName)} يجب ألا يتجاوز ${field.errors['maxlength'].requiredLength} حرف`;
        }
        if (field.errors['email']) {
            return 'يرجى إدخال بريد إلكتروني صحيح';
        }
        return '';
    }

    getPasswordError(): string {
        const control = this.form.get('password');
        if (!control?.touched) return '';
        if (control.errors?.['minlength']) {
            return 'كلمة المرور يجب ألا تقل عن 8 أحرف';
        }
        if (control.errors?.['pattern']) {
            return 'يجب أن تحتوي كلمة المرور على حرف كبير وحرف صغير ورقم';
        }
        if (this.form.errors?.['passwordRequiredForEmail']) {
            return 'كلمة المرور مطلوبة عند إدخال بريد إلكتروني للمريض';
        }
        return '';
    }

    private getFieldLabel(fieldName: string): string {
        const labels: Record<string, string> = {
            fullName: 'الاسم الكامل',
            email: 'البريد الإلكتروني',
            password: 'كلمة المرور',
            phone: 'الهاتف',
            dateOfBirth: 'تاريخ الميلاد',
            gender: 'الجنس',
            address: 'العنوان',
            referralSource: 'مصدر الإحالة',
            chiefComplaint: 'الشكوى الرئيسية',
        };
        return labels[fieldName] || fieldName;
    }
}
