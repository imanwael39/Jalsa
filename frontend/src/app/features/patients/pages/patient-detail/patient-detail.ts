import { Component, ChangeDetectionStrategy, inject, OnInit, OnDestroy, DestroyRef, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PatientService } from '../../../../core/services/patient.service';
import { PatientStateService } from '../../../../core/state/patient-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';
import { DatePipe } from '@angular/common';
import { SessionList } from '../../../sessions/pages/session-list/session-list';
import { PatientExerciseComponent } from '../../../exercises/pages/patient-exercise/patient-exercise.component';
import { ReportList } from '../../../reports/pages/report-list/report-list';
import { Assessment } from '../assessment/assessment';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

@Component({
    selector: 'app-patient-detail',
    standalone: true,
    imports: [
        RouterLink,
        ButtonComponent,
        SpinnerComponent,
        ModalComponent,
        DatePipe,
        SessionList,
        PatientExerciseComponent,
        ReportList,
        Assessment,
        StatusArPipe,
    ],
    templateUrl: './patient-detail.html',
    styleUrl: './patient-detail.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientDetail implements OnInit, OnDestroy {
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private patientService = inject(PatientService);
    private state = inject(PatientStateService);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    patient = this.state.selectedPatient;
    loading = signal(true);
    error = signal<string | null>(null);
    activeTab = signal<'overview' | 'sessions' | 'exercises' | 'reports' | 'assessments'>('overview');

    showArchiveModal = signal(false);
    showDeleteModal = signal(false);
    actionLoading = signal(false);

    ngOnDestroy(): void {
        this.state.clearSelected();
    }

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.loadPatient(id);
        } else {
            this.error.set('معرف المريض غير موجود');
            this.loading.set(false);
        }
    }

    loadPatient(id: string): void {
        this.loading.set(true);
        this.error.set(null);
        this.patientService
            .getPatient(id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: patient => {
                    this.state.selectPatient(patient);
                    this.loading.set(false);
                },
                error: err => {
                    this.error.set(err?.message || 'فشل تحميل بيانات المريض');
                    this.loading.set(false);
                },
            });
    }

    setTab(tab: 'overview' | 'sessions' | 'exercises' | 'reports' | 'assessments'): void {
        this.activeTab.set(tab);
    }

    goBack(): void {
        this.router.navigate(['/patients']);
    }

    navigateToEdit(): void {
        const p = this.patient();
        if (p) {
            this.router.navigate(['/patients', p.id, 'edit']);
        }
    }

    navigateToIntake(): void {
        const p = this.patient();
        if (p) {
            this.router.navigate(['/patients', p.id, 'intake']);
        }
    }

    navigateToAssessments(): void {
        const p = this.patient();
        if (p) {
            this.router.navigate(['/patients', p.id, 'assessments']);
        }
    }

    openArchiveModal(): void {
        this.showArchiveModal.set(true);
    }

    closeArchiveModal(): void {
        this.showArchiveModal.set(false);
    }

    openDeleteModal(): void {
        this.showDeleteModal.set(true);
    }

    closeDeleteModal(): void {
        this.showDeleteModal.set(false);
    }

    archivePatient(): void {
        const p = this.patient();
        if (!p) return;

        this.actionLoading.set(true);
        this.patientService
            .archivePatient(p.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.state.updatePatient({ ...p, status: 'Archived' });
                    this.notification.success('تم أرشفة المريض بنجاح');
                    this.closeArchiveModal();
                    this.actionLoading.set(false);
                },
                error: err => {
                    this.notification.error(err?.message || 'فشل أرشفة المريض');
                    this.actionLoading.set(false);
                },
            });
    }

    restorePatient(): void {
        const p = this.patient();
        if (!p) return;

        this.actionLoading.set(true);
        this.patientService
            .restorePatient(p.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.state.updatePatient({ ...p, status: 'Active' });
                    this.notification.success('تم استعادة المريض بنجاح');
                    this.closeArchiveModal();
                    this.actionLoading.set(false);
                },
                error: err => {
                    this.notification.error(err?.message || 'فشل استعادة المريض');
                    this.actionLoading.set(false);
                },
            });
    }

    deletePatient(): void {
        const p = this.patient();
        if (!p) return;

        this.actionLoading.set(true);
        this.patientService
            .deletePatient(p.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.state.removePatient(p.id);
                    this.state.clearSelected();
                    this.notification.success('تم حذف المريض بنجاح');
                    this.closeDeleteModal();
                    this.router.navigate(['/patients']);
                },
                error: err => {
                    this.notification.error(err?.message || 'فشل حذف المريض');
                    this.actionLoading.set(false);
                },
            });
    }

    isArchived(): boolean {
        return this.patient()?.status === 'Archived';
    }

    formatDate(date: string | null): string {
        if (!date) return 'غير متوفر';
        return new Date(date).toLocaleDateString();
    }

    calculateAge(dateOfBirth: string | null): number {
        if (!dateOfBirth) return 0;
        const today = new Date();
        const birth = new Date(dateOfBirth);
        let age = today.getFullYear() - birth.getFullYear();
        const monthDiff = today.getMonth() - birth.getMonth();
        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birth.getDate())) {
            age--;
        }
        return age;
    }
}
