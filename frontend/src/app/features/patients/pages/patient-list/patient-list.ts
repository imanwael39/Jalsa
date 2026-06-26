import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-patient-list',
    standalone: true,
    imports: [],
    templateUrl: './patient-list.html',
    styleUrl: './patient-list.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientList {}
