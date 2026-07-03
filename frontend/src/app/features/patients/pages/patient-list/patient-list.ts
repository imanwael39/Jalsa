import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable, Subject, debounceTime, distinctUntilChanged, switchMap, map } from 'rxjs';
import { PatientService } from '../../../../core/services/patient.service';
import { PatientStateService } from '../../../../core/state/patient-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Patient } from '../../../../core/models';
import { TableComponent, TableColumn } from '../../../../shared/components/table/table.component';
import { ColumnCellDirective } from '../../../../shared/components/table/column-cell.directive';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

@Component({
    selector: 'app-patient-list',
    standalone: true,
    imports: [FormsModule, TableComponent, ColumnCellDirective, ButtonComponent, StatusArPipe],
    templateUrl: './patient-list.html',
    styleUrl: './patient-list.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientList implements OnInit {
    private patientService = inject(PatientService);
    private state = inject(PatientStateService);
    private notification = inject(NotificationService);
    private router = inject(Router);
    private route = inject(ActivatedRoute);
    private destroyRef = inject(DestroyRef);

    patients = this.state.patients;
    loading = this.state.loading;
    error = this.state.error;

    searchTerm = '';
    currentPage = 1;
    pageSize = 10;
    includeArchived = false;
    /**
     * The backend's patient list endpoint returns a plain array with no total count,
     * so true page-count pagination isn't possible. We fetch one extra row per page
     * to detect whether a next page exists, instead of fabricating a total.
     */
    hasNextPage = false;

    private searchSubject = new Subject<string>();

    columns: TableColumn[] = [
        { key: 'fullName', label: 'الاسم', sortable: true },
        { key: 'email', label: 'البريد الإلكتروني', sortable: true },
        { key: 'phone', label: 'الهاتف' },
        { key: 'dateOfBirth', label: 'تاريخ الميلاد', sortable: true },
        { key: 'gender', label: 'الجنس' },
        { key: 'status', label: 'الحالة' },
        { key: 'actions', label: 'الإجراءات', align: 'center' },
    ];

    ngOnInit(): void {
        const searchFromUrl = this.route.snapshot.queryParamMap.get('search');
        if (searchFromUrl) {
            this.searchTerm = searchFromUrl;
        }

        this.loadPatients();

        this.searchSubject
            .pipe(
                debounceTime(300),
                distinctUntilChanged(),
                takeUntilDestroyed(this.destroyRef),
                switchMap(() => {
                    this.currentPage = 1;
                    return this.fetchPatients();
                })
            )
            .subscribe();
    }

    onSearch(term: string): void {
        this.searchSubject.next(term);
    }

    onPageChange(page: number): void {
        this.currentPage = page;
        this.fetchPatients().pipe(takeUntilDestroyed(this.destroyRef)).subscribe();
    }

    onToggleArchived(): void {
        this.currentPage = 1;
        this.fetchPatients().pipe(takeUntilDestroyed(this.destroyRef)).subscribe();
    }

    private fetchPatients(): Observable<Patient[]> {
        this.state.setLoading(true);
        return this.patientService
            .getPatients({
                searchTerm: this.searchTerm || undefined,
                status: this.includeArchived ? undefined : 'Active',
                page: this.currentPage,
                pageSize: this.pageSize + 1,
            })
            .pipe(
                map(result => {
                    this.hasNextPage = result.length > this.pageSize;
                    return this.hasNextPage ? result.slice(0, this.pageSize) : result;
                })
            );
    }

    loadPatients(): void {
        this.fetchPatients()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: result => {
                    this.state.setPatients(result);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.message || err.error?.error || 'فشل تحميل قائمة المرضى');
                    this.state.setLoading(false);
                },
            });
    }

    archivePatient(id: string): void {
        this.patientService
            .archivePatient(id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.notification.success('تم أرشفة المريض بنجاح');
                    this.loadPatients();
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || err.error?.error || 'فشل أرشفة المريض');
                },
            });
    }

    restorePatient(id: string): void {
        this.patientService
            .restorePatient(id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.notification.success('تم استعادة المريض بنجاح');
                    this.loadPatients();
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || err.error?.error || 'فشل استعادة المريض');
                },
            });
    }

    onRowClick(patient: Patient): void {
        this.router.navigate(['/patients', patient.id]);
    }

    navigateToNew(): void {
        this.router.navigate(['/patients/new']);
    }

    navigateToView(id: string): void {
        this.router.navigate(['/patients', id]);
    }

    navigateToEdit(id: string): void {
        this.router.navigate(['/patients', id, 'edit']);
    }

    formatDate(date: string | null): string {
        if (!date) return '-';
        return new Date(date).toLocaleDateString();
    }

    trackByPatientId(index: number, patient: Patient): string {
        return patient.id;
    }
}
