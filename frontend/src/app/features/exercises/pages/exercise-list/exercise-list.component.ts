import { Component, ChangeDetectionStrategy, inject, signal, OnInit, OnDestroy, DestroyRef } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ExerciseService } from '../../../../core/services/exercise.service';
import { ExerciseStateService } from '../../../../core/state/exercise-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { TableComponent, TableColumn } from '../../../../shared/components/table/table.component';
import { ColumnCellDirective } from '../../../../shared/components/table/column-cell.directive';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';
import { TruncatePipe } from '../../../../shared/pipes/truncate.pipe';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

@Component({
    selector: 'app-exercise-list',
    standalone: true,
    imports: [TableComponent, ColumnCellDirective, ButtonComponent, ModalComponent, TruncatePipe, StatusArPipe],
    templateUrl: './exercise-list.component.html',
    styleUrl: './exercise-list.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExerciseListComponent implements OnInit, OnDestroy {
    private router = inject(Router);
    private exerciseService = inject(ExerciseService);
    private state = inject(ExerciseStateService);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    exercises = this.state.exercises;
    loading = this.state.loading;
    error = this.state.error;

    showDeleteModal = signal(false);
    deletingExerciseId = signal<string | null>(null);

    columns: TableColumn[] = [
        { key: 'description', label: 'الوصف' },
        { key: 'frequency', label: 'التكرار' },
        { key: 'status', label: 'الحالة' },
        { key: 'dueDate', label: 'تاريخ الاستحقاق' },
        { key: 'actions', label: 'الإجراءات', align: 'center' },
    ];

    ngOnInit(): void {
        this.loadExercises();
    }

    ngOnDestroy(): void {
        this.state.reset();
    }

    loadExercises(): void {
        this.state.setLoading(true);
        this.state.setError(null);
        this.exerciseService
            .getExercises()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.state.setExercises(data);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.message || err.error?.error || 'فشل تحميل التمارين');
                    this.state.setLoading(false);
                },
            });
    }

    navigateToAssign(): void {
        this.router.navigate(['/exercises/assign']);
    }

    openDeleteModal(id: string): void {
        this.deletingExerciseId.set(id);
        this.showDeleteModal.set(true);
    }

    closeDeleteModal(): void {
        this.showDeleteModal.set(false);
        this.deletingExerciseId.set(null);
    }

    deleteExercise(): void {
        const id = this.deletingExerciseId();
        if (!id) return;

        this.exerciseService
            .deleteExercise(id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.state.removeExercise(id);
                    this.notification.success('تم حذف التمرين بنجاح');
                    this.closeDeleteModal();
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || err.error?.error || 'فشل حذف التمرين');
                },
            });
    }

    formatDate(date: string | null): string {
        if (!date) return '-';
        return new Date(date).toLocaleDateString();
    }
}
