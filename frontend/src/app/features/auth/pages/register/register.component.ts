import { Component, ChangeDetectionStrategy, inject, signal, DestroyRef } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AuthService } from '../../../../core/services/auth.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { passwordMatchValidator } from '../../../../shared/validators/password-match.validator';

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent, RouterLink],
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterComponent {
    private fb = inject(NonNullableFormBuilder);
    private authService = inject(AuthService);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    loading = signal(false);
    registerError = signal<string | null>(null);
    success = signal(false);

    registerForm = this.fb.group({
        firstName: ['', [Validators.required, Validators.minLength(2)]],
        lastName: ['', [Validators.required, Validators.minLength(2)]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
        role: ['Therapist', [Validators.required]],
    }, { validators: passwordMatchValidator });

    getFirstNameError(): string {
        const control = this.registerForm.get('firstName');
        if (!control || !control.errors || !control.touched) {
            return '';
        }
        if (control.errors['required']) {
            return 'First name is required';
        }
        if (control.errors['minlength']) {
            return 'First name must be at least 2 characters';
        }
        return '';
    }

    getLastNameError(): string {
        const control = this.registerForm.get('lastName');
        if (!control || !control.errors || !control.touched) {
            return '';
        }
        if (control.errors['required']) {
            return 'Last name is required';
        }
        if (control.errors['minlength']) {
            return 'Last name must be at least 2 characters';
        }
        return '';
    }

    getEmailError(): string {
        const control = this.registerForm.get('email');
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

    getPasswordError(): string {
        const control = this.registerForm.get('password');
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
        const control = this.registerForm.get('confirmPassword');
        if (!control || !control.errors || !control.touched) {
            return '';
        }
        if (control.errors['required']) {
            return 'Please confirm your password';
        }
        return '';
    }

    getPasswordMismatchError(): string {
        if (this.registerForm.errors?.['passwordMismatch'] && this.registerForm.get('confirmPassword')?.touched) {
            return 'Passwords do not match';
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

        const { firstName, lastName, email, password, role } = this.registerForm.getRawValue();

        this.authService.register({ firstName, lastName, email, password, role }).pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe({
            next: () => {
                this.loading.set(false);
                this.success.set(true);
                setTimeout(() => {
                    this.router.navigate(['/auth/login']);
                }, 2000);
            },
            error: (err) => {
                this.loading.set(false);
                this.registerError.set(
                    err.error?.message || err.error?.error || 'Registration failed. Please try again.',
                );
            },
        });
    }
}
