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
        const emailControl = this.loginForm.get('email');
        if (!emailControl || !emailControl.errors || !emailControl.touched) {
            return '';
        }
        if (emailControl.errors['required']) {
            return 'Email is required';
        }
        if (emailControl.errors['email']) {
            return 'Please enter a valid email address';
        }
        return '';
    }

    getPasswordError(): string {
        const passwordControl = this.loginForm.get('password');
        if (!passwordControl || !passwordControl.errors || !passwordControl.touched) {
            return '';
        }
        if (passwordControl.errors['required']) {
            return 'Password is required';
        }
        if (passwordControl.errors['minlength']) {
            return 'Password must be at least 6 characters';
        }
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

        this.authService.login({ email, password }).pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe({
            next: () => {
                this.loading.set(false);
                this.router.navigateByUrl(this.returnUrl);
            },
            error: (err) => {
                this.loading.set(false);
                this.loginError.set(
                    err.error?.message || 'Invalid email or password. Please try again.',
                );
            },
        });
    }
}
