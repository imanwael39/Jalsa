import { Component, ChangeDetectionStrategy, inject, signal, DestroyRef } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AuthService } from '../../../../core/services/auth.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent, RouterLink],
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
    private fb = inject(NonNullableFormBuilder);
    private authService = inject(AuthService);
    private router = inject(Router);
    private route = inject(ActivatedRoute);
    private destroyRef = inject(DestroyRef);

    loading = signal(false);
    loginError = signal<string | null>(null);

    loginForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
    });

    get returnUrl(): string {
        return this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
    }

    getEmailError(): string {
        const c = this.loginForm.get('email');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'البريد الإلكتروني مطلوب';
        if (c.errors['email']) return 'يرجى إدخال بريد إلكتروني صحيح';
        return '';
    }

    getPasswordError(): string {
        const c = this.loginForm.get('password');
        if (!c?.errors || !c.touched) return '';
        if (c.errors['required']) return 'كلمة المرور مطلوبة';
        if (c.errors['minlength']) return 'يجب ألا تقل كلمة المرور عن 6 أحرف';
        return '';
    }

    onSubmit(): void {
        if (this.loginForm.invalid) {
            this.loginForm.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        this.loginError.set(null);

        const { email, password } = this.loginForm.getRawValue();

        this.authService
            .login({ email, password })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.loading.set(false);
                    const roles = this.authService.currentUser()?.roles ?? [];
                    let target = this.returnUrl;
                    if (roles.includes('Patient')) {
                        target = '/exercises/my-exercises';
                    } else if (roles.includes('Admin') && target === '/dashboard') {
                        target = '/admin/dashboard';
                    }
                    this.router.navigateByUrl(target);
                },
                error: err => {
                    this.loading.set(false);
                    this.loginError.set(
                        err.error?.message || err.error?.error || 'البريد الإلكتروني أو كلمة المرور غير صحيحة'
                    );
                },
            });
    }
}
