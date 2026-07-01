import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { ReportDetail } from './report-detail';
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
};

interface SetupOptions {
    routeId?: string | null;
    getReportReturn?: Observable<unknown>;
    approveReportReturn?: Observable<unknown>;
    deleteReportReturn?: Observable<unknown>;
}

describe('ReportDetail', () => {
    let component: ReportDetail;
    let fixture: ComponentFixture<ReportDetail>;
    let reportServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { routeId = 'rep-1', getReportReturn, approveReportReturn, deleteReportReturn } = opts;

        reportServiceSpy = {
            getReport: vi.fn().mockReturnValue(getReportReturn ?? of(mockReport)),
            approveReport: vi.fn().mockReturnValue(approveReportReturn ?? of({ ...mockReport, status: 'Approved' })),
            deleteReport: vi.fn().mockReturnValue(deleteReportReturn ?? of(undefined)),
            exportReport: vi.fn().mockReturnValue(of(new Blob(['test'], { type: 'text/html' }))),
        };
        stateSpy = {
            selectedReport: signal(mockReport),
            selectReport: vi.fn(),
            updateReport: vi.fn(),
            removeReport: vi.fn(),
            clearSelected: vi.fn(),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
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
                                get: (key: string): string | null => (key === 'id' ? routeId : null),
                            },
                        },
                    },
                },
            ],
        });

        fixture = TestBed.createComponent(ReportDetail);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load report on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(reportServiceSpy['getReport']).toHaveBeenCalledWith('rep-1');
        expect(stateSpy['selectReport']).toHaveBeenCalledWith(expect.objectContaining({ id: 'rep-1' }));
    });

    it('should set error when report ID not found', (): void => {
        setup({ routeId: null });
        fixture.detectChanges();
        expect(component.error()).toBe('معرّف التقرير غير موجود');
    });

    it('should set error when API call fails', (): void => {
        setup({ getReportReturn: throwError(() => ({ error: { message: 'API Error' } })) });
        fixture.detectChanges();
        expect(component.error()).toBe('API Error');
    });

    it('should approve report', (): void => {
        setup();
        fixture.detectChanges();
        component.approveReport();
        expect(reportServiceSpy['approveReport']).toHaveBeenCalledWith('rep-1');
        expect(stateSpy['selectReport']).toHaveBeenCalledWith(expect.objectContaining({ status: 'Approved' }));
        expect(stateSpy['updateReport']).toHaveBeenCalledWith(expect.objectContaining({ status: 'Approved' }));
        expect(notificationSpy['success']).toHaveBeenCalled();
    });

    it('should handle approve error', (): void => {
        setup({ approveReportReturn: throwError((): Error => new Error('Approve failed')) });
        fixture.detectChanges();
        component.approveReport();
        expect(component.approving()).toBe(false);
    });

    it('should delete report with confirmation', (): void => {
        const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);
        setup();
        fixture.detectChanges();
        component.deleteReport();
        expect(reportServiceSpy['deleteReport']).toHaveBeenCalledWith('rep-1');
        expect(stateSpy['removeReport']).toHaveBeenCalled();
        expect(stateSpy['clearSelected']).toHaveBeenCalled();
        expect(notificationSpy['success']).toHaveBeenCalled();
        confirmSpy.mockRestore();
    });

    it('should not delete when confirmation cancelled', (): void => {
        const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(false);
        setup();
        fixture.detectChanges();
        component.deleteReport();
        expect(reportServiceSpy['deleteReport']).not.toHaveBeenCalled();
        confirmSpy.mockRestore();
    });

    it('should export report and call service', (): void => {
        setup();
        fixture.detectChanges();
        component.exportReport();
        expect(reportServiceSpy['exportReport']).toHaveBeenCalledWith('rep-1');
    });

    it('should navigate back', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateBack();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/reports/patient', 'pat-1']);
    });

    it('should format date', (): void => {
        setup();
        expect(component.formatDate('2024-01-15T10:00:00Z')).toBeTruthy();
        expect(component.formatDate(null)).toBe('-');
    });

    it('should format datetime', (): void => {
        setup();
        expect(component.formatDateTime('2024-01-15T10:00:00Z')).toBeTruthy();
        expect(component.formatDateTime(null)).toBe('-');
    });

    it('should clear selected on destroy', (): void => {
        setup();
        fixture.detectChanges();
        fixture.destroy();
        expect(stateSpy['clearSelected']).toHaveBeenCalled();
    });
});
