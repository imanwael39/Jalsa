import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef } from '@angular/core';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ExerciseService } from '../../../../core/services/exercise.service';
import { ExerciseStateService } from '../../../../core/state/exercise-state.service';
import { TableComponent, TableColumn } from '../../../../shared/components/table/table.component';
import { ColumnCellDirective } from '../../../../shared/components/table/column-cell.directive';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { TruncatePipe } from '../../../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-exercise-list',
  standalone: true,
  imports: [TableComponent, ColumnCellDirective, ButtonComponent, TruncatePipe],
  templateUrl: './exercise-list.component.html',
  styleUrl: './exercise-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExerciseListComponent implements OnInit {
  private router = inject(Router);
  private exerciseService = inject(ExerciseService);
  private state = inject(ExerciseStateService);
  private destroyRef = inject(DestroyRef);

  exercises = this.state.exercises;
  loading = this.state.loading;
  error = this.state.error;

  columns: TableColumn[] = [
    { key: 'description', label: 'Description' },
    { key: 'frequency', label: 'Frequency' },
    { key: 'status', label: 'Status' },
    { key: 'dueDate', label: 'Due Date' },
  ];

  ngOnInit(): void {
    this.loadExercises();
  }

  loadExercises(): void {
    this.state.setLoading(true);
    this.state.setError(null);
    this.exerciseService.getExercises()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (data) => {
          this.state.setExercises(data);
          this.state.setLoading(false);
        },
        error: (err) => {
          this.state.setError(err.message || 'Failed to load exercises');
          this.state.setLoading(false);
        },
      });
  }

  navigateToAssign(): void {
    this.router.navigate(['/exercises/assign']);
  }

  formatDate(date: string | null): string {
    if (!date) return '-';
    return new Date(date).toLocaleDateString();
  }
}
