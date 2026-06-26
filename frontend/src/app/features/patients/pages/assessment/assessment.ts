import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-assessment',
    standalone: true,
    imports: [],
    templateUrl: './assessment.html',
    styleUrl: './assessment.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Assessment {}
