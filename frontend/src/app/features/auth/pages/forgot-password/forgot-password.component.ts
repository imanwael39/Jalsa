import { Component, ChangeDetectionStrategy, inject, signal, DestroyRef } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AuthService } from '../../../../core/services/auth.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';

@Component({
    selector: 'app-forgot-password',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent, RouterLink],
    templateUrl: './forgot-password.component.html',
    styleUrls: ['./forgot-password.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ForgotPasswordComponent {
    private fb = inject(NonNullableFormBuilder);
    private authService = inject(AuthService);
    private destroyRef = inject(DestroyRef);

    loading = signal(false);
    submitted = signal(false);
    error = signal<string | null>(null);

    forgotForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
    });

    getEmailError(): string {
        const c = this.forgotForm.get('email');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'البريد الإلكتروني مطلوب';
        if (c.errors['email']) return 'يرجى إدخال بريد إلكتروني صحيح';
        return '';
    }

    onSubmit(): void {
        if (this.forgotForm.invalid) {
            this.forgotForm.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        this.error.set(null);

        const { email } = this.forgotForm.getRawValue();

        this.authService.forgotPassword(email).pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe({
            next: () => {
                this.loading.set(false);
                this.submitted.set(true);
            },
            error: (err) => {
                this.loading.set(false);
                this.error.set(
                    err.error?.message || err.error?.error || 'حدث خطأ، يرجى المحاولة مرة أخرى',
                );
            },
        });
    }
}
