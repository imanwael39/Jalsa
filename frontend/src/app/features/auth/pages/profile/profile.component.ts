import { Component, ChangeDetectionStrategy, inject, signal, OnInit, DestroyRef } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AuthService } from '../../../../core/services/auth.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { passwordMatchValidator } from '../../../../shared/validators/password-match.validator';

@Component({
    selector: 'app-profile',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent],
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileComponent implements OnInit {
    private fb = inject(NonNullableFormBuilder);
    private authService = inject(AuthService);
    private destroyRef = inject(DestroyRef);

    loading = signal(true);
    updating = signal(false);
    changingPassword = signal(false);
    error = signal<string | null>(null);
    passwordError = signal<string | null>(null);
    success = signal(false);

    profileForm = this.fb.group({
        firstName: ['', [Validators.required, Validators.minLength(2)]],
        lastName: ['', [Validators.required, Validators.minLength(2)]],
        email: ['', [Validators.required, Validators.email]],
    });

    passwordForm = this.fb.group({
        currentPassword: ['', [Validators.required]],
        newPassword: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
    }, { validators: passwordMatchValidator });

    ngOnInit(): void {
        this.loadProfile();
    }

    loadProfile(): void {
        this.loading.set(true);
        this.authService.getProfile().pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe({
            next: (user) => {
                this.profileForm.patchValue({
                    firstName: user.firstName,
                    lastName: user.lastName,
                    email: user.email,
                });
                this.loading.set(false);
            },
            error: () => {
                this.loading.set(false);
                this.error.set('Failed to load profile. Please try again.');
            },
        });
    }

    getFirstNameError(): string {
        const control = this.profileForm.get('firstName');
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
        const control = this.profileForm.get('lastName');
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
        const control = this.profileForm.get('email');
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

    getCurrentPasswordError(): string {
        const control = this.passwordForm.get('currentPassword');
        if (!control || !control.errors || !control.touched) {
            return '';
        }
        if (control.errors['required']) {
            return 'Current password is required';
        }
        return '';
    }

    getNewPasswordError(): string {
        const control = this.passwordForm.get('newPassword');
        if (!control || !control.errors || !control.touched) {
            return '';
        }
        if (control.errors['required']) {
            return 'New password is required';
        }
        if (control.errors['minlength']) {
            return 'Password must be at least 6 characters';
        }
        return '';
    }

    getConfirmPasswordError(): string {
        const control = this.passwordForm.get('confirmPassword');
        if (!control || !control.errors || !control.touched) {
            return '';
        }
        if (control.errors['required']) {
            return 'Please confirm your password';
        }
        return '';
    }

    getPasswordMismatchError(): string {
        if (this.passwordForm.errors?.['passwordMismatch'] && this.passwordForm.get('confirmPassword')?.touched) {
            return 'Passwords do not match';
        }
        return '';
    }

    updateProfile(): void {
        if (this.profileForm.invalid) {
            this.profileForm.markAllAsTouched();
            return;
        }

        this.updating.set(true);
        this.error.set(null);

        const { firstName, lastName, email } = this.profileForm.getRawValue();

        this.authService.updateProfile({ firstName, lastName, email }).pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe({
            next: () => {
                this.updating.set(false);
                this.success.set(true);
                setTimeout(() => this.success.set(false), 3000);
            },
            error: (err) => {
                this.updating.set(false);
                this.error.set(err.error?.message || err.error?.error || 'Failed to update profile.');
            },
        });
    }

    changePassword(): void {
        if (this.passwordForm.invalid) {
            this.passwordForm.markAllAsTouched();
            return;
        }

        this.changingPassword.set(true);
        this.passwordError.set(null);

        const { currentPassword, newPassword } = this.passwordForm.getRawValue();

        this.authService.changePassword({ currentPassword, newPassword }).pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe({
            next: () => {
                this.changingPassword.set(false);
                this.passwordForm.reset();
                this.success.set(true);
                setTimeout(() => this.success.set(false), 3000);
            },
            error: (err) => {
                this.changingPassword.set(false);
                this.passwordError.set(err.error?.message || err.error?.error || 'Failed to change password.');
            },
        });
    }
}
