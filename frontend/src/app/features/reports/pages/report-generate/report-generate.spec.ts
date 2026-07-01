import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { Observable, of, throwError } from 'rxjs';
import { ReportGenerate } from './report-generate';
import { ReportService } from '../../../../core/services/report.service';
import { ReportStateService } from '../../../../core/state/report-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { ReferralReport } from '../../../../core/models';

const mockReport: ReferralReport = {
    id: 'rep-1',
    patientId: 'pat-1',
    therapistId: 'th-1',
    generatedByTherapistId: 'th-1',
    status: 'Draft',
    currentVersionId: null,
    createdAt: '2024-01-15T10:00:00Z',
    updatedAt: '2024-01-15T10:00:00Z',
    currentVersion: null,
    versions: [],
};

interface SetupOptions {
    routePatientId?: string | null;
    generateReturn?: Observable<unknown>;
}

describe('ReportGenerate', () => {
    let component: ReportGenerate;
    let fixture: ComponentFixture<ReportGenerate>;
    let reportServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { routePatientId = 'pat-1', generateReturn } = opts;

        reportServiceSpy = {
            generateReport: vi.fn().mockReturnValue(generateReturn ?? of(mockReport)),
        };
        stateSpy = { addReport: vi.fn() };
        notificationSpy = { success: vi.fn(), error: vi.fn() };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule],
            providers: [
                provideRouter([]),
                { provide: ReportService, useValue: reportServiceSpy },
                { provide: ReportStateService, useValue: stateSpy },
                { provide: NotificationService, useValue: notificationSpy },
                { provide: Router, useValue: routerSpy },
                {
                    provide: ActivatedRoute,
                    useValue: {
                        snapshot: {
                            paramMap: {
                                get: (key: string): string | null => (key === 'patientId' ? routePatientId : null),
                            },
                        },
                    },
                },
            ],
        });

        fixture = TestBed.createComponent(ReportGenerate);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should set patientId from route on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.patientId()).toBe('pat-1');
    });

    it('should submit form and generate report', (): void => {
        setup();
        fixture.detectChanges();
        component.onSubmit();
        expect(reportServiceSpy['generateReport']).toHaveBeenCalledWith(
            expect.objectContaining({ patientId: 'pat-1' })
        );
        expect(stateSpy['addReport']).toHaveBeenCalledWith(mockReport);
        expect(notificationSpy['success']).toHaveBeenCalled();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/reports', mockReport.id]);
    });

    it('should include therapist instructions in generation', (): void => {
        setup();
        fixture.detectChanges();
        component.form.patchValue({ therapistInstructions: 'Focus on anxiety' });
        component.onSubmit();
        expect(reportServiceSpy['generateReport']).toHaveBeenCalledWith(
            expect.objectContaining({ therapistInstructions: 'Focus on anxiety' })
        );
    });

    it('should handle generation error', (): void => {
        setup({ generateReturn: throwError((): Error => new Error('Generation failed')) });
        fixture.detectChanges();
        component.onSubmit();
        expect(component.error()).toBe('Generation failed');
        expect(component.generating()).toBe(false);
    });

    it('should cancel and navigate back', (): void => {
        setup();
        fixture.detectChanges();
        component.cancel();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/reports/patient', 'pat-1']);
    });

    it('should set generating state during submission', (): void => {
        const generateSubject = new Observable<ReferralReport>(sub => {
            setTimeout(() => sub.next(mockReport));
        });
        setup({ generateReturn: generateSubject });
        fixture.detectChanges();
        component.onSubmit();
        expect(component.generating()).toBe(true);
    });
});
