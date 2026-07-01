import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { provideRouter } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { of, throwError } from 'rxjs';
import { ForgotPasswordComponent } from './forgot-password.component';
import { AuthService } from '../../../../core/services/auth.service';
import { NotificationService } from '../../../../core/services/notification.service';

interface SetupOptions {
    forgotPasswordReturn?: ReturnType<typeof of> | ReturnType<typeof throwError>;
}

describe('ForgotPasswordComponent', () => {
    let component: ForgotPasswordComponent;
    let fixture: ComponentFixture<ForgotPasswordComponent>;
    let authServiceSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { forgotPasswordReturn } = opts;

        authServiceSpy = {
            forgotPassword: vi.fn().mockReturnValue(forgotPasswordReturn ?? of({})),
            currentUser: signal(null),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };

        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule, ForgotPasswordComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: AuthService, useValue: authServiceSpy },
                { provide: NotificationService, useValue: notificationSpy },
            ],
        });

        fixture = TestBed.createComponent(ForgotPasswordComponent);
        component = fixture.componentInstance;
    };

    it('should create', () => {
        setup();
        expect(component).toBeTruthy();
    });
    it('should have invalid form when empty', () => {
        setup();
        expect(component.forgotForm.invalid).toBe(true);
    });
    it('should return empty email error when untouched', () => {
        setup();
        expect(component.getEmailError()).toBe('');
    });
    it('should return required email error when touched and empty', () => {
        setup();
        component.forgotForm.get('email')!.markAsTouched();
        expect(component.getEmailError()).toBe('البريد الإلكتروني مطلوب');
    });
    it('should return invalid email error for bad format', () => {
        setup();
        component.forgotForm.patchValue({ email: 'bad' });
        component.forgotForm.get('email')!.markAsTouched();
        expect(component.getEmailError()).toBe('يرجى إدخال بريد إلكتروني صحيح');
    });
    it('should not submit when form invalid', () => {
        setup();
        component.onSubmit();
        expect(authServiceSpy['forgotPassword']).not.toHaveBeenCalled();
    });
    it('should call authService.forgotPassword on valid submit', () => {
        setup();
        component.forgotForm.patchValue({ email: 't@t.com' });
        component.onSubmit();
        expect(authServiceSpy['forgotPassword']).toHaveBeenCalledWith('t@t.com');
    });
    it('should set submitted on success', () => {
        setup();
        component.forgotForm.patchValue({ email: 't@t.com' });
        component.onSubmit();
        expect(component.submitted()).toBe(true);
    });
    it('should show error on failure', () => {
        setup({ forgotPasswordReturn: throwError(() => ({ error: { message: 'البريد غير موجود' } })) });
        component.forgotForm.patchValue({ email: 't@t.com' });
        component.onSubmit();
        expect(component.error()).toBe('البريد غير موجود');
    });
    it('should set loading back to false after error', () => {
        setup({ forgotPasswordReturn: throwError(() => new Error('fail')) });
        component.forgotForm.patchValue({ email: 't@t.com' });
        component.onSubmit();
        expect(component.loading()).toBe(false);
    });
});
