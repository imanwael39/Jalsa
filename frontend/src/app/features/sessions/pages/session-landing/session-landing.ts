import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonComponent } from '../../../../shared/components/button/button.component';

@Component({
    selector: 'app-session-landing',
    standalone: true,
    imports: [ButtonComponent],
    template: `
        <div class="landing-container">
            <div class="landing-content text-center">
                <div class="landing-icon">
                    <i class="bi bi-calendar-event" aria-hidden="true"></i>
                </div>
                <h2>الجلسات</h2>
                <p class="text-muted">يرجى اختيار مريض أولاً لعرض جلساته.</p>
                <app-button variant="primary" (clicked)="goToPatients()" ariaLabel="الذهاب لقائمة المرضى">
                    <i class="bi bi-people" aria-hidden="true"></i> عرض المرضى
                </app-button>
            </div>
        </div>
    `,
    styles: [
        `
            .landing-container {
                display: flex;
                justify-content: center;
                align-items: center;
                min-height: 60vh;
            }
            .landing-content {
                max-width: 400px;
            }
            .landing-icon {
                font-size: 4rem;
                color: var(--primary);
                margin-bottom: 1rem;
            }
            h2 {
                margin-bottom: 0.5rem;
            }
            p {
                margin-bottom: 1.5rem;
            }
        `,
    ],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SessionLanding {
    private router = inject(Router);

    goToPatients(): void {
        this.router.navigate(['/patients']);
    }
}
