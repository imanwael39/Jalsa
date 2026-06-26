import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable, Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { PatientService } from '../../../../core/services/patient.service';
import { PatientStateService } from '../../../../core/state/patient-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Patient } from '../../../../core/models';
import { TableComponent, TableColumn } from '../../../../shared/components/table/table.component';
import { ColumnCellDirective } from '../../../../shared/components/table/column-cell.directive';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';

@Component({
    selector: 'app-patient-list',
    standalone: true,
    imports: [
        FormsModule,
        TableComponent,
        ColumnCellDirective,
        PaginationComponent,
        ButtonComponent,
        InputComponent,
    ],
    templateUrl: './patient-list.html',
    styleUrl: './patient-list.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientList implements OnInit {
    private patientService = inject(PatientService);
    private state = inject(PatientStateService);
    private notification = inject(NotificationService);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    patients = this.state.patients;
    loading = this.state.loading;
    error = this.state.error;

    searchTerm = '';
    currentPage = 1;
    pageSize = 10;
    includeArchived = false;
    totalItems = 0;

    private searchSubject = new Subject<string>();

    columns: TableColumn[] = [
        { key: 'fullName', label: 'Name', sortable: true },
        { key: 'email', label: 'Email', sortable: true },
        { key: 'phone', label: 'Phone' },
        { key: 'dateOfBirth', label: 'DOB', sortable: true },
        { key: 'gender', label: 'Gender' },
        { key: 'status', label: 'Status' },
        { key: 'actions', label: 'Actions', align: 'center' },
    ];

    ngOnInit(): void {
        this.loadPatients();

        this.searchSubject.pipe(
            debounceTime(300),
            distinctUntilChanged(),
            takeUntilDestroyed(this.destroyRef),
            switchMap(() => {
                this.currentPage = 1;
                return this.fetchPatients();
            }),
        ).subscribe();
    }

    onSearch(term: string): void {
        this.searchSubject.next(term);
    }

    onPageChange(page: number): void {
        this.currentPage = page;
        this.fetchPatients().pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe();
    }

    onToggleArchived(): void {
        this.currentPage = 1;
        this.fetchPatients().pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe();
    }

    private fetchPatients(): Observable<Patient[]> {
        this.state.setLoading(true);
        return this.patientService.getPatients({
            searchTerm: this.searchTerm || undefined,
            status: this.includeArchived ? undefined : 'Active',
            page: this.currentPage,
            pageSize: this.pageSize,
        });
    }

    loadPatients(): void {
        this.fetchPatients().pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe({
            next: (result) => {
                this.state.setPatients(result);
                this.totalItems = result.length;
                this.state.setLoading(false);
            },
            error: (err) => {
                this.state.setError(err.message || 'Failed to load patients');
                this.state.setLoading(false);
            },
        });
    }

    archivePatient(id: string): void {
        this.patientService.archivePatient(id).pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe({
            next: () => {
                this.notification.success('Patient archived successfully');
                this.loadPatients();
            },
            error: (err) => {
                this.notification.error(err.message || 'Failed to archive patient');
            },
        });
    }

    restorePatient(id: string): void {
        this.patientService.restorePatient(id).pipe(
            takeUntilDestroyed(this.destroyRef),
        ).subscribe({
            next: () => {
                this.notification.success('Patient restored successfully');
                this.loadPatients();
            },
            error: (err) => {
                this.notification.error(err.message || 'Failed to restore patient');
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
