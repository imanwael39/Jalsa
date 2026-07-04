import { Component, ChangeDetectionStrategy, inject, signal, DestroyRef, OnInit } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AuthService } from '../../../../core/services/auth.service';
import { PasswordResetStateService } from '../../../../core/services/password-reset-state.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';

@Component({
    selector: 'app-verify-otp',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent, RouterLink],
    templateUrl: './verify-otp.component.html',
    styleUrls: ['./verify-otp.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class VerifyOtpComponent implements OnInit {
    private fb = inject(NonNullableFormBuilder);
    private authService = inject(AuthService);
    private passwordResetState = inject(PasswordResetStateService);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    loading = signal(false);
    error = signal<string | null>(null);

    otpForm = this.fb.group({
        otp: ['', [Validators.required, Validators.minLength(6)]],
    });

    ngOnInit(): void {
        if (!this.passwordResetState.getEmail()) {
            this.router.navigate(['/auth/forgot-password']);
        }
    }

    getOtpError(): string {
        const c = this.otpForm.get('otp');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'رمز التحقق مطلوب';
        if (c.errors['minlength']) return 'يجب أن يحتوي الرمز على 6 أحرف على الأقل';
        return '';
    }

    onSubmit(): void {
        if (this.otpForm.invalid) {
            this.otpForm.markAllAsTouched();
            return;
        }

        const email = this.passwordResetState.getEmail();
        if (!email) {
            this.router.navigate(['/auth/forgot-password']);
            return;
        }

        this.loading.set(true);
        this.error.set(null);

        const { otp } = this.otpForm.getRawValue();

        this.authService
            .verifyOtp({ email, otp })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.loading.set(false);
                    this.passwordResetState.setOtp(otp);
                    this.router.navigate(['/auth/reset-password']);
                },
                error: err => {
                    this.loading.set(false);
                    this.error.set(err.error?.message || err.error?.error || 'حدث خطأ، يرجى المحاولة مرة أخرى');
                },
            });
    }
}
