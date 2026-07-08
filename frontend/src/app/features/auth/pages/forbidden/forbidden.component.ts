import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
    selector: 'app-forbidden',
    standalone: true,
    changeDetection: ChangeDetectionStrategy.OnPush,
    template: `
        <div class="forbidden-page">
            <div class="forbidden-card">
                <div class="forbidden-icon">
                    <i class="bi bi-shield-exclamation"></i>
                </div>
                <h1 class="forbidden-title">غير مصرح</h1>
                <p class="forbidden-body">ليس لديك صلاحية الوصول إلى هذه الصفحة.</p>
                <button class="btn-home" (click)="goHome()">العودة للرئيسية</button>
            </div>
        </div>
    `,
    styles: [
        `
            .forbidden-page {
                min-height: 100vh;
                display: flex;
                align-items: center;
                justify-content: center;
                background: var(--background);
                direction: rtl;
            }
            .forbidden-card {
                background: var(--surface);
                border: 1px solid var(--border-color);
                border-radius: var(--radius-lg);
                padding: 3rem 2rem;
                text-align: center;
                max-width: 400px;
                width: 90%;
                box-shadow: var(--shadow-md);
            }
            .forbidden-icon {
                font-size: 3rem;
                color: var(--danger);
                margin-bottom: 1rem;
            }
            .forbidden-title {
                font-size: 1.5rem;
                font-weight: 700;
                color: var(--text-primary);
                margin-bottom: 0.5rem;
            }
            .forbidden-body {
                color: var(--text-muted);
                margin-bottom: 1.5rem;
            }
            .btn-home {
                background: var(--primary);
                color: var(--text-inverse);
                border: none;
                border-radius: var(--radius-sm);
                padding: 0.625rem 1.5rem;
                font-size: 1rem;
                font-family: inherit;
                cursor: pointer;
                transition: opacity var(--transition-fast);
            }
            .btn-home:hover {
                opacity: 0.9;
            }
        `,
    ],
})
export class ForbiddenComponent {
    private router = inject(Router);
    private authService = inject(AuthService);

    goHome(): void {
        const roles = this.authService.currentUser()?.roles ?? [];
        if (roles.includes('Patient')) {
            this.router.navigate(['/dashboard/patient']);
        } else if (roles.includes('Admin')) {
            this.router.navigate(['/admin/dashboard']);
        } else {
            this.router.navigate(['/dashboard']);
        }
    }
}
