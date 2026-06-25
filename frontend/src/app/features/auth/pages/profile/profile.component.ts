import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-profile',
    standalone: true,
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileComponent {}
