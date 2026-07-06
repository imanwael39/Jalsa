import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { ForbiddenComponent } from './forbidden.component';
import { AuthService } from '../../../../core/services/auth.service';

interface SetupOptions {
    currentUserRoles?: string[];
}

describe('ForbiddenComponent', () => {
    let component: ForbiddenComponent;
    let fixture: ComponentFixture<ForbiddenComponent>;
    let authServiceSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { currentUserRoles } = opts;

        authServiceSpy = {
            currentUser: signal(
                currentUserRoles
                    ? {
                          id: '1',
                          email: 't@t.com',
                          firstName: 'T',
                          lastName: 'U',
                          profileImageUrl: null,
                          roles: currentUserRoles,
                      }
                    : null
            ),
        };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            imports: [ForbiddenComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: AuthService, useValue: authServiceSpy },
                { provide: Router, useValue: routerSpy },
            ],
        });

        fixture = TestBed.createComponent(ForbiddenComponent);
        component = fixture.componentInstance;
    };

    it('should create', () => {
        setup();
        expect(component).toBeTruthy();
    });
    it('should navigate Patient to my-exercises', () => {
        setup({ currentUserRoles: ['Patient'] });
        component.goHome();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/exercises/my-exercises']);
    });
    it('should navigate Therapist to dashboard', () => {
        setup({ currentUserRoles: ['Therapist'] });
        component.goHome();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/dashboard']);
    });
    it('should navigate to dashboard when no roles', () => {
        setup();
        component.goHome();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/dashboard']);
    });
    it('should navigate Admin to dashboard', () => {
        setup({ currentUserRoles: ['Admin'] });
        component.goHome();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/dashboard']);
    });
});
