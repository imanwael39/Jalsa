import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-patient-form',
    standalone: true,
    imports: [],
    templateUrl: './patient-form.html',
    styleUrl: './patient-form.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientForm {}
