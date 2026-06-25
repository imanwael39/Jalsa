import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-auth-layout',
    standalone: true,
    templateUrl: './auth-layout.component.html',
    styleUrls: ['./auth-layout.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthLayoutComponent {
    readonly currentYear = new Date().getFullYear();
}
