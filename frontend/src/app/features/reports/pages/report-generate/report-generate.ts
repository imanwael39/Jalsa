import { Component, ChangeDetectionStrategy, inject, signal, OnInit, DestroyRef } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReportService } from '../../../../core/services/report.service';
import { ReportStateService } from '../../../../core/state/report-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { AiDisclaimerComponent } from '../../../../shared/components/ai-disclaimer/ai-disclaimer.component';

@Component({
    selector: 'app-report-generate',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, SpinnerComponent, AiDisclaimerComponent],
    templateUrl: './report-generate.html',
    styleUrl: './report-generate.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReportGenerate implements OnInit {
    private fb = inject(FormBuilder);
    private reportService = inject(ReportService);
    private state = inject(ReportStateService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    patientId = signal('');
    generating = signal(false);
    error = signal<string | null>(null);

    form = this.fb.group({
        therapistInstructions: [''],
        language: ['ar'],
    });

    ngOnInit(): void {
        const pid = this.route.snapshot.paramMap.get('patientId') || '';
        this.patientId.set(pid);
    }

    onSubmit(): void {
        if (!this.patientId()) return;

        this.generating.set(true);
        this.error.set(null);

        const formValue = this.form.value;
        this.reportService
            .generateReport({
                patientId: this.patientId(),
                therapistInstructions: formValue.therapistInstructions || undefined,
                language: formValue.language || 'ar',
            })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: report => {
                    this.state.addReport(report);
                    this.notification.success('تم إنشاء التقرير بنجاح');
                    this.generating.set(false);
                    this.router.navigate(['/reports', report.id]);
                },
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.message || err.error?.error || 'فشل في إنشاء التقرير');
                    this.generating.set(false);
                },
            });
    }

    cancel(): void {
        this.router.navigate(['/reports/patient', this.patientId()]);
    }
}
