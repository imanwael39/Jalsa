import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-exercise-list',
  standalone: true,
  imports: [],
  templateUrl: './exercise-list.component.html',
  styleUrls: ['./exercise-list.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExerciseListComponent {
}
