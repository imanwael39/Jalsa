import { Component, ChangeDetectionStrategy, inject, signal, DestroyRef } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AuthService } from '../../../../core/services/auth.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { passwordMatchValidator } from '../../../../shared/validators/password-match.validator';

@Component({
    selector: 'app-reset-password',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent, RouterLink],
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResetPasswordComponent {
    private fb = inject(NonNullableFormBuilder);
    private authService = inject(AuthService);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    loading = signal(false);
    success = signal(false);
    error = signal<string | null>(null);

    resetForm = this.fb.group(
        {
            email: ['', [Validators.required, Validators.email]],
            otp: ['', [Validators.required, Validators.minLength(6)]],
            password: [
                '',
                [
                    Validators.required,
                    Validators.minLength(8),
                    Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/),
                ],
            ],
            confirmPassword: ['', [Validators.required]],
        },
        { validators: passwordMatchValidator }
    );

    getEmailError(): string {
        const c = this.resetForm.get('email');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'البريد الإلكتروني مطلوب';
        if (c.errors['email']) return 'يرجى إدخال بريد إلكتروني صحيح';
        return '';
    }

    getOtpError(): string {
        const c = this.resetForm.get('otp');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'رمز التحقق مطلوب';
        if (c.errors['minlength']) return 'يجب أن يحتوي الرمز على 6 أحرف على الأقل';
        return '';
    }

    getPasswordError(): string {
        const c = this.resetForm.get('password');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'كلمة المرور مطلوبة';
        if (c.errors['minlength']) return 'يجب ألا تقل كلمة المرور عن 8 أحرف';
        if (c.errors['pattern']) return 'يجب أن تحتوي كلمة المرور على حرف كبير وحرف صغير ورقم';
        return '';
    }

    getConfirmPasswordError(): string {
        const c = this.resetForm.get('confirmPassword');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'تأكيد كلمة المرور مطلوب';
        return '';
    }

    getPasswordMismatchError(): string {
        if (this.resetForm.errors?.['passwordMismatch'] && this.resetForm.get('confirmPassword')?.touched) {
            return 'كلمتا المرور غير متطابقتين';
        }
        return '';
    }

    onSubmit(): void {
        if (this.resetForm.invalid) {
            this.resetForm.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        this.error.set(null);

        const { email, otp, password } = this.resetForm.getRawValue();

        this.authService
            .resetPassword({ email, otp, newPassword: password })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.loading.set(false);
                    this.success.set(true);
                    setTimeout(() => {
                        this.router.navigate(['/auth/login']);
                    }, 3000);
                },
                error: err => {
                    this.loading.set(false);
                    this.error.set(err.error?.message || err.error?.error || 'حدث خطأ، يرجى المحاولة مرة أخرى');
                },
            });
    }
}
