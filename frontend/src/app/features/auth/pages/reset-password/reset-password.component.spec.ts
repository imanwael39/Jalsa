import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { provideRouter, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { of, throwError } from 'rxjs';
import { ResetPasswordComponent } from './reset-password.component';
import { AuthService } from '../../../../core/services/auth.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { PasswordResetStateService } from '../../../../core/services/password-reset-state.service';

interface SetupOptions {
    resetPasswordReturn?: ReturnType<typeof of> | ReturnType<typeof throwError>;
    email?: string | null;
    otp?: string | null;
}

describe('ResetPasswordComponent', () => {
    let component: ResetPasswordComponent;
    let fixture: ComponentFixture<ResetPasswordComponent>;
    let authServiceSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;
    let passwordResetStateSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { resetPasswordReturn, email = 'saved@test.com', otp = '123456' } = opts;

        authServiceSpy = {
            resetPassword: vi.fn().mockReturnValue(resetPasswordReturn ?? of({})),
            currentUser: signal(null),
        };
        routerSpy = { navigate: vi.fn() };
        notificationSpy = { success: vi.fn(), error: vi.fn() };
        passwordResetStateSpy = {
            getEmail: vi.fn().mockReturnValue(email),
            getOtp: vi.fn().mockReturnValue(otp),
            setEmail: vi.fn(),
            setOtp: vi.fn(),
            clear: vi.fn(),
        };

        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule, ResetPasswordComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: AuthService, useValue: authServiceSpy },
                { provide: NotificationService, useValue: notificationSpy },
                { provide: Router, useValue: routerSpy },
                { provide: PasswordResetStateService, useValue: passwordResetStateSpy },
            ],
        });

        fixture = TestBed.createComponent(ResetPasswordComponent);
        component = fixture.componentInstance;
    };

    it('should create', () => {
        setup();
        expect(component).toBeTruthy();
    });
    it('should have invalid form when empty', () => {
        setup();
        expect(component.resetForm.invalid).toBe(true);
    });
    it('should redirect to forgot-password when email/otp missing', () => {
        setup({ email: null });
        component.ngOnInit();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/auth/forgot-password']);
    });
    it('should not redirect when email and otp are present', () => {
        setup();
        component.ngOnInit();
        expect(routerSpy['navigate']).not.toHaveBeenCalled();
    });
    it('should return required password error', () => {
        setup();
        component.resetForm.get('password')!.markAsTouched();
        expect(component.getPasswordError()).toBe('كلمة المرور مطلوبة');
    });
    it('should return required confirmPassword error', () => {
        setup();
        component.resetForm.get('confirmPassword')!.markAsTouched();
        expect(component.getConfirmPasswordError()).toBe('تأكيد كلمة المرور مطلوب');
    });
    it('should return mismatch error when passwords differ', () => {
        setup();
        component.resetForm.patchValue({ password: 'Passw0rd123', confirmPassword: 'different' });
        component.resetForm.get('confirmPassword')!.markAsTouched();
        expect(component.getPasswordMismatchError()).toBe('كلمتا المرور غير متطابقتين');
    });
    it('should not submit when form invalid', () => {
        setup();
        component.onSubmit();
        expect(authServiceSpy['resetPassword']).not.toHaveBeenCalled();
    });
    it('should call authService.resetPassword with the stored email/otp on valid submit', () => {
        setup({ email: 't@t.com', otp: '123456' });
        component.resetForm.patchValue({
            password: 'Passw0rd123',
            confirmPassword: 'Passw0rd123',
        });
        component.onSubmit();
        expect(authServiceSpy['resetPassword']).toHaveBeenCalledWith({
            email: 't@t.com',
            otp: '123456',
            newPassword: 'Passw0rd123',
        });
    });
    it('should set success and clear state on reset success', () => {
        setup();
        component.resetForm.patchValue({
            password: 'Passw0rd123',
            confirmPassword: 'Passw0rd123',
        });
        component.onSubmit();
        expect(component.success()).toBe(true);
        expect(passwordResetStateSpy['clear']).toHaveBeenCalled();
    });
    it('should show error on reset failure', () => {
        setup({ resetPasswordReturn: throwError(() => ({ error: { message: 'رمز غير صحيح' } })) });
        component.resetForm.patchValue({
            password: 'Passw0rd123',
            confirmPassword: 'Passw0rd123',
        });
        component.onSubmit();
        expect(component.error()).toBe('رمز غير صحيح');
    });
    it('should set loading back to false after error', () => {
        setup({ resetPasswordReturn: throwError(() => new Error('fail')) });
        component.resetForm.patchValue({
            password: 'Passw0rd123',
            confirmPassword: 'Passw0rd123',
        });
        component.onSubmit();
        expect(component.loading()).toBe(false);
    });
});
