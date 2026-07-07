import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AdminService } from '../../../../core/services/admin.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { CheckboxComponent } from '../../../../shared/components/checkbox/checkbox.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-settings',
    standalone: true,
    imports: [ReactiveFormsModule, InputComponent, CheckboxComponent, ButtonComponent, SpinnerComponent],
    templateUrl: './settings.component.html',
    styleUrl: './settings.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SettingsComponent implements OnInit {
    private adminService = inject(AdminService);
    private notification = inject(NotificationService);
    private fb = inject(FormBuilder);
    private destroyRef = inject(DestroyRef);

    loading = signal(false);
    saving = signal(false);
    error = signal<string | null>(null);

    form = this.fb.nonNullable.group({
        siteName: ['', [Validators.required, Validators.maxLength(100)]],
        defaultLanguage: ['ar', Validators.required],
        passwordMinLength: [8, [Validators.required, Validators.min(6), Validators.max(32)]],
        sessionTimeoutMinutes: [60, [Validators.required, Validators.min(5), Validators.max(480)]],
        maintenanceMode: [false],
    });

    ngOnInit(): void {
        this.loading.set(true);
        this.adminService
            .getSettings()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: settings => {
                    this.form.patchValue(settings);
                    this.loading.set(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.message || 'فشل تحميل إعدادات النظام.');
                    this.loading.set(false);
                },
            });
    }

    save(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.saving.set(true);
        this.adminService
            .updateSettings(this.form.getRawValue())
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.notification.success('تم حفظ الإعدادات بنجاح');
                    this.saving.set(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || 'فشل حفظ الإعدادات.');
                    this.saving.set(false);
                },
            });
    }
}
