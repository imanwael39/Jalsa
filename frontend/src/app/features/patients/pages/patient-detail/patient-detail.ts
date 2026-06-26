import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-patient-detail',
    standalone: true,
    imports: [],
    templateUrl: './patient-detail.html',
    styleUrl: './patient-detail.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientDetail {}
