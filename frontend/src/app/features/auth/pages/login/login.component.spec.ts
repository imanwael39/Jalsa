import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { provideRouter, Router, ActivatedRoute } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { of, Subject, Observable } from 'rxjs';
import { LoginComponent } from './login.component';
import { AuthService } from '../../../../core/services/auth.service';
import { NotificationService } from '../../../../core/services/notification.service';

interface SetupOptions {
    loginReturn?: unknown;
    currentUserRoles?: string[];
}

describe('LoginComponent', () => {
    let component: LoginComponent;
    let fixture: ComponentFixture<LoginComponent>;
    let authServiceSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { loginReturn, currentUserRoles } = opts;

        authServiceSpy = {
            login: vi.fn().mockReturnValue(loginReturn ?? of({ token: 'mock-token', roles: ['Therapist'] })),
            currentUser: signal(
                currentUserRoles
                    ? { id: '1', email: 't@t.com', firstName: 'T', lastName: 'U', roles: currentUserRoles }
                    : null
            ),
        };
        routerSpy = { navigateByUrl: vi.fn(), navigate: vi.fn() };
        notificationSpy = { success: vi.fn(), error: vi.fn() };

        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule, LoginComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: AuthService, useValue: authServiceSpy },
                { provide: NotificationService, useValue: notificationSpy },
                { provide: Router, useValue: routerSpy },
                {
                    provide: ActivatedRoute,
                    useValue: { snapshot: { paramMap: { get: (): null => null }, queryParams: {} } },
                },
            ],
        });

        fixture = TestBed.createComponent(LoginComponent);
        component = fixture.componentInstance;
    };

    it('should create', () => {
        setup();
        expect(component).toBeTruthy();
    });
    it('should have invalid form when empty', () => {
        setup();
        expect(component.loginForm.invalid).toBe(true);
    });
    it('should return empty email error when untouched', () => {
        setup();
        expect(component.getEmailError()).toBe('');
    });
    it('should return required email error when touched and empty', () => {
        setup();
        component.loginForm.get('email')!.markAsTouched();
        expect(component.getEmailError()).toBe('البريد الإلكتروني مطلوب');
    });
    it('should return invalid email error for bad format', () => {
        setup();
        component.loginForm.patchValue({ email: 'invalid' });
        component.loginForm.get('email')!.markAsTouched();
        expect(component.getEmailError()).toBe('يرجى إدخال بريد إلكتروني صحيح');
    });
    it('should return required password error when touched and empty', () => {
        setup();
        component.loginForm.get('password')!.markAsTouched();
        expect(component.getPasswordError()).toBe('كلمة المرور مطلوبة');
    });
    it('should not submit when form invalid', () => {
        setup();
        component.onSubmit();
        expect(authServiceSpy['login']).not.toHaveBeenCalled();
    });
    it('should call authService.login on valid submit', () => {
        setup();
        component.loginForm.patchValue({ email: 't@t.com', password: '123456' });
        component.onSubmit();
        expect(authServiceSpy['login']).toHaveBeenCalledWith({ email: 't@t.com', password: '123456' });
    });
    it('should navigate Therapist to /dashboard on success', () => {
        const subj = new Subject<unknown>();
        setup({ loginReturn: subj });
        component.loginForm.patchValue({ email: 't@t.com', password: '123456' });
        authServiceSpy['currentUser'] = signal({
            id: '1',
            email: 't@t.com',
            firstName: 'T',
            lastName: 'U',
            roles: ['Therapist'],
        });
        component.onSubmit();
        subj.next({ token: 'token' });
        expect(routerSpy['navigateByUrl']).toHaveBeenCalledWith('/dashboard');
    });
    it('should navigate Patient to my-exercises on success', () => {
        const subj = new Subject<unknown>();
        setup({ loginReturn: subj, currentUserRoles: ['Patient'] });
        component.loginForm.patchValue({ email: 'p@t.com', password: '123456' });
        component.onSubmit();
        subj.next({ token: 'token' });
        expect(routerSpy['navigateByUrl']).toHaveBeenCalledWith('/exercises/my-exercises');
    });
    it('should show error on login failure', () => {
        setup({ loginReturn: new Observable(sub => sub.error({ error: { message: 'خطأ' } })) });
        component.loginForm.patchValue({ email: 't@t.com', password: 'wrong' });
        component.onSubmit();
        expect(component.loading()).toBe(false);
    });
    it('should set loading back to false after error', () => {
        const subj = new Subject<unknown>();
        setup({ loginReturn: subj });
        component.loginForm.patchValue({ email: 't@t.com', password: 'wrong' });
        component.onSubmit();
        subj.error(new Error('fail'));
        expect(component.loading()).toBe(false);
    });
});
