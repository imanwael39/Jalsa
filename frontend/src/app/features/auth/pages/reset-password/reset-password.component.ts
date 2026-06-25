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

    resetForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        otp: ['', [Validators.required, Validators.minLength(6)]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
    }, { validators: passwordMatchValidator });

    getEmailError(): string {
        const control = this.resetForm.get('email');
        if (!control || !control.errors || !control.touched) {
            return '';
        }
        if (control.errors['required']) {
            return 'Email is required';
        }
        if (control.errors['email']) {
            return 'Please enter a valid email address';
        }
        return '';
    }

    getOtpError(): string {
        const control = this.resetForm.get('otp');
        if (!control || !control.errors || !control.touched) {
            return '';
        }
        if (control.errors['required']) {
            return 'OTP is required';
        }
        if (control.errors['minlength']) {
            return 'OTP must be at least 6 characters';
        }
        return '';
    }

    getPasswordError(): string {
        const control = this.resetForm.get('password');
        if (!control || !control.errors || !control.touched) {
            return '';
        }
        if (control.errors['required']) {
            return 'Password is required';
        }
        if (control.errors['minlength']) {
            return 'Password must be at least 6 characters';
        }
        return '';
    }

    getConfirmPasswordError(): string {
        const control = this.resetForm.get('confirmPassword');
        if (!control || !control.errors || !control.touched) {
            return '';
        }
        if (control.errors['required']) {
            return 'Please confirm your password';
        }
        return '';
    }

    getPasswordMismatchError(): string {
        if (this.resetForm.errors?.['passwordMismatch'] && this.resetForm.get('confirmPassword')?.touched) {
            return 'Passwords do not match';
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

        this.authService.resetPassword({ email, otp, newPassword: password }).pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe({
            next: () => {
                this.loading.set(false);
                this.success.set(true);
                setTimeout(() => {
                    this.router.navigate(['/auth/login']);
                }, 3000);
            },
            error: (err) => {
                this.loading.set(false);
                this.error.set(
                    err.error?.message || 'Failed to reset password. Please try again.',
                );
            },
        });
    }
}
