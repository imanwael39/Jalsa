import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { signal } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { ReportList } from './report-list';
import { ReportService } from '../../../../core/services/report.service';
import { ReportStateService } from '../../../../core/state/report-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { ReferralReport } from '../../../../core/models';

const mockReports: ReferralReport[] = [
    {
        id: 'rep-1',
        patientId: 'pat-1',
        therapistId: 'th-1',
        generatedByTherapistId: 'th-1',
        status: 'Draft',
        currentVersionId: 'ver-1',
        createdAt: '2024-01-15T10:00:00Z',
        updatedAt: '2024-01-15T11:00:00Z',
        currentVersion: {
            id: 'ver-1',
            reportId: 'rep-1',
            versionNumber: 1,
            content: 'Report content',
            createdByTherapistId: 'th-1',
            approvedAt: null,
            changeNote: null,
            createdAt: '2024-01-15T10:00:00Z',
        },
        versions: [
            {
                id: 'ver-1',
                reportId: 'rep-1',
                versionNumber: 1,
                content: 'Report content',
                createdByTherapistId: 'th-1',
                approvedAt: null,
                changeNote: null,
                createdAt: '2024-01-15T10:00:00Z',
            },
        ],
    },
];

interface SetupOptions {
    patientId?: string | null;
    routePatientId?: string | null;
    getReportsReturn?: Observable<unknown>;
    deleteReportReturn?: Observable<unknown>;
}

describe('ReportList', () => {
    let component: ReportList;
    let fixture: ComponentFixture<ReportList>;
    let reportServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { patientId = null, routePatientId = 'pat-1', getReportsReturn, deleteReportReturn } = opts;

        reportServiceSpy = {
            getPatientReports: vi.fn().mockReturnValue(getReportsReturn ?? of(mockReports)),
            deleteReport: vi.fn().mockReturnValue(deleteReportReturn ?? of(undefined)),
        };
        stateSpy = {
            reports: signal(mockReports),
            loading: signal(false),
            error: signal(null),
            setReports: vi.fn(),
            setLoading: vi.fn(),
            setError: vi.fn(),
            removeReport: vi.fn(),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
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

        fixture = TestBed.createComponent(ReportList);
        component = fixture.componentInstance;
        if (patientId !== null) {
            fixture.componentRef.setInput('patientIdInput', patientId);
        }
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load reports from route param on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(reportServiceSpy['getPatientReports']).toHaveBeenCalledWith('pat-1');
    });

    it('should load reports from input when route param absent', (): void => {
        setup({ routePatientId: null, patientId: 'pat-2' });
        fixture.detectChanges();
        expect(reportServiceSpy['getPatientReports']).toHaveBeenCalledWith('pat-2');
    });

    it('should not load when no patient ID available', (): void => {
        setup({ routePatientId: null, patientId: null });
        fixture.detectChanges();
        expect(reportServiceSpy['getPatientReports']).not.toHaveBeenCalled();
    });

    it('should handle load error', (): void => {
        setup({ getReportsReturn: throwError((): Error => new Error('Load failed')) });
        fixture.detectChanges();
        expect(stateSpy['setError']).toHaveBeenCalled();
    });

    it('should navigate to report detail on row click', (): void => {
        setup();
        fixture.detectChanges();
        component.onRowClick(mockReports[0]);
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/reports', 'rep-1']);
    });

    it('should navigate to generate report', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToGenerate();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/reports/generate', 'pat-1']);
    });

    it('should navigate to view report', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToView('rep-1');
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/reports', 'rep-1']);
    });

    it('should delete report with confirmation', (): void => {
        const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);
        setup();
        fixture.detectChanges();
        component.deleteReport('rep-1');
        expect(reportServiceSpy['deleteReport']).toHaveBeenCalledWith('rep-1');
        expect(stateSpy['removeReport']).toHaveBeenCalledWith('rep-1');
        expect(notificationSpy['success']).toHaveBeenCalled();
        confirmSpy.mockRestore();
    });

    it('should not delete when confirmation cancelled', (): void => {
        const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(false);
        setup();
        fixture.detectChanges();
        component.deleteReport('rep-1');
        expect(reportServiceSpy['deleteReport']).not.toHaveBeenCalled();
        confirmSpy.mockRestore();
    });

    it('should return version count', (): void => {
        setup();
        expect(component.getVersionCount(mockReports[0])).toBe(1);
    });

    it('should format date', (): void => {
        setup();
        expect(component.formatDate('2024-01-15T10:00:00Z')).toBeTruthy();
        expect(component.formatDate(null)).toBe('-');
    });
});
