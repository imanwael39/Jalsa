import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-register',
    standalone: true,
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterComponent {}
