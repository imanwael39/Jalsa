import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, convertToParamMap, provideRouter, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { of, throwError } from 'rxjs';
import { ResetPasswordComponent } from './reset-password.component';
import { AuthService } from '../../../../core/services/auth.service';
import { NotificationService } from '../../../../core/services/notification.service';

interface SetupOptions {
    resetPasswordReturn?: ReturnType<typeof of> | ReturnType<typeof throwError>;
    queryParamEmail?: string;
}

describe('ResetPasswordComponent', () => {
    let component: ResetPasswordComponent;
    let fixture: ComponentFixture<ResetPasswordComponent>;
    let authServiceSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { resetPasswordReturn, queryParamEmail } = opts;

        authServiceSpy = {
            resetPassword: vi.fn().mockReturnValue(resetPasswordReturn ?? of({})),
            currentUser: signal(null),
        };
        routerSpy = { navigate: vi.fn() };
        notificationSpy = { success: vi.fn(), error: vi.fn() };

        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule, ResetPasswordComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: AuthService, useValue: authServiceSpy },
                { provide: NotificationService, useValue: notificationSpy },
                { provide: Router, useValue: routerSpy },
                {
                    provide: ActivatedRoute,
                    useValue: {
                        snapshot: {
                            queryParamMap: convertToParamMap(queryParamEmail ? { email: queryParamEmail } : {}),
                        },
                    },
                },
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
    it('should return required email error', () => {
        setup();
        component.resetForm.get('email')!.markAsTouched();
        expect(component.getEmailError()).toBe('البريد الإلكتروني مطلوب');
    });
    it('should return invalid email error', () => {
        setup();
        component.resetForm.patchValue({ email: 'bad' });
        component.resetForm.get('email')!.markAsTouched();
        expect(component.getEmailError()).toBe('يرجى إدخال بريد إلكتروني صحيح');
    });
    it('should return required otp error', () => {
        setup();
        component.resetForm.get('otp')!.markAsTouched();
        expect(component.getOtpError()).toBe('رمز التحقق مطلوب');
    });
    it('should return minlength otp error', () => {
        setup();
        component.resetForm.patchValue({ otp: '12' });
        component.resetForm.get('otp')!.markAsTouched();
        expect(component.getOtpError()).toBe('يجب أن يحتوي الرمز على 6 أحرف على الأقل');
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
    it('should call authService.resetPassword on valid submit', () => {
        setup();
        component.resetForm.patchValue({
            email: 't@t.com',
            otp: '123456',
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
    it('should set success on reset success', () => {
        setup();
        component.resetForm.patchValue({
            email: 't@t.com',
            otp: '123456',
            password: 'Passw0rd123',
            confirmPassword: 'Passw0rd123',
        });
        component.onSubmit();
        expect(component.success()).toBe(true);
    });
    it('should show error on reset failure', () => {
        setup({ resetPasswordReturn: throwError(() => ({ error: { message: 'رمز غير صحيح' } })) });
        component.resetForm.patchValue({
            email: 't@t.com',
            otp: '000000',
            password: 'Passw0rd123',
            confirmPassword: 'Passw0rd123',
        });
        component.onSubmit();
        expect(component.error()).toBe('رمز غير صحيح');
    });
    it('should prefill and lock the email field when provided via query params', () => {
        setup({ queryParamEmail: 'carried@over.com' });
        expect(component.resetForm.get('email')!.value).toBe('carried@over.com');
        expect(component.emailPrefilled()).toBe(true);
    });
    it('should leave the email field empty and editable without a query param', () => {
        setup();
        expect(component.resetForm.get('email')!.value).toBe('');
        expect(component.emailPrefilled()).toBe(false);
    });
    it('should set loading back to false after error', () => {
        setup({ resetPasswordReturn: throwError(() => new Error('fail')) });
        component.resetForm.patchValue({
            email: 't@t.com',
            otp: '123456',
            password: 'Passw0rd123',
            confirmPassword: 'Passw0rd123',
        });
        component.onSubmit();
        expect(component.loading()).toBe(false);
    });
});
