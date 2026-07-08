import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { AppointmentDetailComponent } from './appointment-detail.component';
import { PatientSessionService } from '../../../../core/services/patient-session.service';
import { PatientSessionDetail } from '../../../../core/models';

const mockDetail: PatientSessionDetail = {
    id: 'sess-1',
    sessionNumber: 1,
    sessionDate: '2026-07-15',
    durationMinutes: 50,
    sessionType: 'فردية',
    status: 'Scheduled',
    therapistName: 'د. أحمد سالم',
    patientRequestType: null,
    patientRequestNote: null,
    patientRequestStatus: null,
    patientRequestedAt: null,
    canRequestChange: true,
};

interface SetupOptions {
    routeId?: string | null;
    getSessionReturn?: Observable<unknown>;
    requestRescheduleReturn?: Observable<unknown>;
    requestCancelReturn?: Observable<unknown>;
}

describe('AppointmentDetailComponent', () => {
    let component: AppointmentDetailComponent;
    let fixture: ComponentFixture<AppointmentDetailComponent>;
    let patientSessionServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { routeId = 'sess-1', getSessionReturn, requestRescheduleReturn, requestCancelReturn } = opts;

        patientSessionServiceSpy = {
            getSession: vi.fn().mockReturnValue(getSessionReturn ?? of(mockDetail)),
            requestReschedule: vi.fn().mockReturnValue(requestRescheduleReturn ?? of(undefined)),
            requestCancel: vi.fn().mockReturnValue(requestCancelReturn ?? of(undefined)),
        };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: PatientSessionService, useValue: patientSessionServiceSpy },
                { provide: Router, useValue: routerSpy },
                {
                    provide: ActivatedRoute,
                    useValue: {
                        snapshot: {
                            paramMap: {
                                get: (key: string): string | null => (key === 'id' ? routeId : null),
                            },
                        },
                    },
                },
            ],
        });

        fixture = TestBed.createComponent(AppointmentDetailComponent);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load session on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(patientSessionServiceSpy['getSession']).toHaveBeenCalledWith('sess-1');
        expect(component.session()).toEqual(mockDetail);
    });

    it('should handle load error', (): void => {
        setup({ getSessionReturn: throwError(() => ({ error: { error: 'فشل التحميل' } })) });
        fixture.detectChanges();
        expect(component.error()).toBe('فشل التحميل');
    });

    it('should open the reschedule request modal with a fresh form', (): void => {
        setup();
        fixture.detectChanges();
        component.requestForm.setValue({ note: 'leftover text' });

        component.openRequestModal('Reschedule');

        expect(component.showModal()).toBe(true);
        expect(component.activeRequestKind()).toBe('Reschedule');
        expect(component.requestForm.getRawValue().note).toBe('');
    });

    it('should close the modal and clear the active request kind', (): void => {
        setup();
        fixture.detectChanges();
        component.openRequestModal('Cancel');

        component.closeModal();

        expect(component.showModal()).toBe(false);
        expect(component.activeRequestKind()).toBeNull();
    });

    it('should not submit an invalid request form', (): void => {
        setup();
        fixture.detectChanges();
        component.openRequestModal('Reschedule');
        component.requestForm.setValue({ note: '' });

        component.submitRequest();

        expect(patientSessionServiceSpy['requestReschedule']).not.toHaveBeenCalled();
        expect(component.requestForm.controls.note.touched).toBe(true);
    });

    it('should submit a reschedule request and reload the session', (): void => {
        setup();
        fixture.detectChanges();
        component.openRequestModal('Reschedule');
        component.requestForm.setValue({ note: 'أحتاج موعداً آخر' });

        component.submitRequest();

        expect(patientSessionServiceSpy['requestReschedule']).toHaveBeenCalledWith('sess-1', {
            note: 'أحتاج موعداً آخر',
        });
        expect(component.showModal()).toBe(false);
        expect(patientSessionServiceSpy['getSession']).toHaveBeenCalledTimes(2);
    });

    it('should submit a cancel request', (): void => {
        setup();
        fixture.detectChanges();
        component.openRequestModal('Cancel');
        component.requestForm.setValue({ note: 'لن أتمكن من الحضور' });

        component.submitRequest();

        expect(patientSessionServiceSpy['requestCancel']).toHaveBeenCalledWith('sess-1', {
            note: 'لن أتمكن من الحضور',
        });
    });

    it('should surface an error when the request submission fails', (): void => {
        setup({ requestRescheduleReturn: throwError(() => ({ error: { error: 'فشل الإرسال' } })) });
        fixture.detectChanges();
        component.openRequestModal('Reschedule');
        component.requestForm.setValue({ note: 'أحتاج موعداً آخر' });

        component.submitRequest();

        expect(component.error()).toBe('فشل الإرسال');
        expect(component.submitting()).toBe(false);
    });

    it('should navigate back to the appointments list', (): void => {
        setup();
        fixture.detectChanges();
        component.goBack();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/appointments']);
    });
});
