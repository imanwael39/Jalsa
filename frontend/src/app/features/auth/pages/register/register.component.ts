import { Component, ChangeDetectionStrategy, inject, signal, computed, DestroyRef, OnInit } from '@angular/core';
import {
    AbstractControl,
    NonNullableFormBuilder,
    ReactiveFormsModule,
    ValidationErrors,
    Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '../../../../core/services/auth.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { passwordMatchValidator } from '../../../../shared/validators/password-match.validator';
import type { TherapistOption } from '../../../../core/models/auth.model';

function therapistRequiredForPatientValidator(group: AbstractControl): ValidationErrors | null {
    const role = group.get('role')?.value;
    const therapistId = group.get('therapistId')?.value;
    return role === 'Patient' && !therapistId ? { therapistRequired: true } : null;
}

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent, RouterLink],
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterComponent implements OnInit {
    private fb = inject(NonNullableFormBuilder);
    private authService = inject(AuthService);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    loading = signal(false);
    registerError = signal<string | null>(null);
    success = signal(false);
    therapistOptions = signal<TherapistOption[]>([]);

    registerForm = this.fb.group(
        {
            fullName: ['', [Validators.required, Validators.minLength(2)]],
            email: ['', [Validators.required, Validators.email]],
            password: [
                '',
                [
                    Validators.required,
                    Validators.minLength(8),
                    Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/),
                ],
            ],
            confirmPassword: ['', [Validators.required]],
            role: ['Therapist', [Validators.required]],
            licenseNumber: [''],
            specialization: [''],
            therapistId: [''],
        },
        { validators: [passwordMatchValidator, therapistRequiredForPatientValidator] }
    );

    private readonly roleValue = toSignal(this.registerForm.get('role')!.valueChanges, {
        initialValue: this.registerForm.get('role')!.value,
    });

    readonly isTherapist = computed(() => this.roleValue() === 'Therapist');
    readonly isPatient = computed(() => this.roleValue() === 'Patient');

    ngOnInit(): void {
        this.authService
            .getTherapistOptions()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: therapists => this.therapistOptions.set(therapists),
                error: () => this.therapistOptions.set([]),
            });
    }

    getFullNameError(): string {
        const c = this.registerForm.get('fullName');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'الاسم الكامل مطلوب';
        if (c.errors['minlength']) return 'يجب أن يحتوي الاسم على حرفين على الأقل';
        return '';
    }

    getEmailError(): string {
        const c = this.registerForm.get('email');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'البريد الإلكتروني مطلوب';
        if (c.errors['email']) return 'يرجى إدخال بريد إلكتروني صحيح';
        return '';
    }

    getPasswordError(): string {
        const c = this.registerForm.get('password');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'كلمة المرور مطلوبة';
        if (c.errors['minlength']) return 'يجب ألا تقل كلمة المرور عن 8 أحرف';
        if (c.errors['pattern']) return 'يجب أن تحتوي كلمة المرور على حرف كبير وحرف صغير ورقم';
        return '';
    }

    getConfirmPasswordError(): string {
        const c = this.registerForm.get('confirmPassword');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'تأكيد كلمة المرور مطلوب';
        return '';
    }

    getPasswordMismatchError(): string {
        if (this.registerForm.errors?.['passwordMismatch'] && this.registerForm.get('confirmPassword')?.touched) {
            return 'كلمتا المرور غير متطابقتين';
        }
        return '';
    }

    getTherapistError(): string {
        const c = this.registerForm.get('therapistId');
        if (!c?.touched) return '';
        if (this.registerForm.errors?.['therapistRequired']) {
            return 'يجب اختيار المعالج المسؤول عن حسابك';
        }
        return '';
    }

    onSubmit(): void {
        if (this.registerForm.invalid) {
            this.registerForm.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        this.registerError.set(null);

        const { fullName, email, password, role, licenseNumber, specialization, therapistId } =
            this.registerForm.getRawValue();

        this.authService
            .register({
                fullName,
                email,
                password,
                role,
                licenseNumber: licenseNumber || undefined,
                specialization: specialization || undefined,
                therapistId: therapistId || undefined,
            })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.loading.set(false);
                    this.success.set(true);
                    setTimeout(() => {
                        this.router.navigate(['/auth/login']);
                    }, 2000);
                },
                error: err => {
                    this.loading.set(false);
                    this.registerError.set(
                        err.error?.message || err.error?.error || 'حدث خطأ غير متوقع، يرجى المحاولة مرة أخرى'
                    );
                },
            });
    }
}
