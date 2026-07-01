import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { provideRouter, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { of, Subject } from 'rxjs';
import { RegisterComponent } from './register.component';
import { AuthService } from '../../../../core/services/auth.service';
import { NotificationService } from '../../../../core/services/notification.service';

interface SetupOptions {
    registerReturn?: unknown;
}

describe('RegisterComponent', () => {
    let component: RegisterComponent;
    let fixture: ComponentFixture<RegisterComponent>;
    let authServiceSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { registerReturn } = opts;

        authServiceSpy = {
            register: vi.fn().mockReturnValue(registerReturn ?? of({})),
            currentUser: signal(null),
        };
        routerSpy = { navigate: vi.fn() };
        notificationSpy = { success: vi.fn(), error: vi.fn() };

        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule, RegisterComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: AuthService, useValue: authServiceSpy },
                { provide: NotificationService, useValue: notificationSpy },
                { provide: Router, useValue: routerSpy },
            ],
        });

        fixture = TestBed.createComponent(RegisterComponent);
        component = fixture.componentInstance;
    };

    it('should create', () => {
        setup();
        expect(component).toBeTruthy();
    });
    it('should have invalid form when empty', () => {
        setup();
        expect(component.registerForm.invalid).toBe(true);
    });
    it('should return required firstName error', () => {
        setup();
        component.registerForm.get('firstName')!.markAsTouched();
        expect(component.getFirstNameError()).toBe('الاسم الأول مطلوب');
    });
    it('should return minlength firstName error', () => {
        setup();
        component.registerForm.patchValue({ firstName: 'A' });
        component.registerForm.get('firstName')!.markAsTouched();
        expect(component.getFirstNameError()).toBe('يجب أن يحتوي الاسم على حرفين على الأقل');
    });
    it('should return required lastName error', () => {
        setup();
        component.registerForm.get('lastName')!.markAsTouched();
        expect(component.getLastNameError()).toBe('اسم العائلة مطلوب');
    });
    it('should return required email error', () => {
        setup();
        component.registerForm.get('email')!.markAsTouched();
        expect(component.getEmailError()).toBe('البريد الإلكتروني مطلوب');
    });
    it('should return required password error', () => {
        setup();
        component.registerForm.get('password')!.markAsTouched();
        expect(component.getPasswordError()).toBe('كلمة المرور مطلوبة');
    });
    it('should return minlength password error', () => {
        setup();
        component.registerForm.patchValue({ password: '123' });
        component.registerForm.get('password')!.markAsTouched();
        expect(component.getPasswordError()).toBe('يجب ألا تقل كلمة المرور عن 8 أحرف');
    });
    it('should return required confirmPassword error', () => {
        setup();
        component.registerForm.get('confirmPassword')!.markAsTouched();
        expect(component.getConfirmPasswordError()).toBe('تأكيد كلمة المرور مطلوب');
    });
    it('should return mismatch error when passwords differ', () => {
        setup();
        component.registerForm.patchValue({ password: '12345678', confirmPassword: 'different' });
        component.registerForm.get('confirmPassword')!.markAsTouched();
        expect(component.getPasswordMismatchError()).toBe('كلمتا المرور غير متطابقتين');
    });
    it('should not submit when form invalid', () => {
        setup();
        component.onSubmit();
        expect(authServiceSpy['register']).not.toHaveBeenCalled();
    });
    it('should call authService.register on valid submit', () => {
        setup();
        component.registerForm.patchValue({
            firstName: 'Test',
            lastName: 'User',
            email: 't@t.com',
            password: '12345678',
            confirmPassword: '12345678',
        });
        component.onSubmit();
        expect(authServiceSpy['register']).toHaveBeenCalled();
    });
    it('should set success on register success', () => {
        const subj = new Subject<unknown>();
        setup({ registerReturn: subj });
        component.registerForm.patchValue({
            firstName: 'Test',
            lastName: 'User',
            email: 't@t.com',
            password: '12345678',
            confirmPassword: '12345678',
        });
        component.onSubmit();
        subj.next({});
        expect(component.success()).toBe(true);
    });
    it('should show error on register failure', () => {
        const subj = new Subject<unknown>();
        setup({ registerReturn: subj });
        component.registerForm.patchValue({
            firstName: 'Test',
            lastName: 'User',
            email: 't@t.com',
            password: '12345678',
            confirmPassword: '12345678',
        });
        component.onSubmit();
        subj.error({ error: { message: 'فشل التسجيل' } });
        expect(component.registerError()).toBe('فشل التسجيل');
    });
    it('should have default role as Therapist', () => {
        setup();
        expect(component.registerForm.get('role')!.value).toBe('Therapist');
    });
});
