import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'app-auth-layout',
    standalone: true,
    imports: [RouterOutlet],
    templateUrl: './auth-layout.component.html',
    styleUrls: ['./auth-layout.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthLayoutComponent {
    readonly currentYear = new Date().getFullYear();
}
