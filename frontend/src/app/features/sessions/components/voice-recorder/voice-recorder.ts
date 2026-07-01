import { Component, ChangeDetectionStrategy, input, output, signal, inject, DestroyRef } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SessionService } from '../../../../core/services/session.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-voice-recorder',
    standalone: true,
    imports: [ButtonComponent, SpinnerComponent],
    templateUrl: './voice-recorder.html',
    styleUrl: './voice-recorder.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class VoiceRecorder {
    sessionId = input<string | null>(null);
    /** Emits the Whisper-transcribed text once the recording finishes uploading. */
    uploaded = output<string>();

    private sessionService = inject(SessionService);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    uploading = signal(false);
    selectedFile = signal<File | null>(null);

    onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.selectedFile.set(input.files[0]);
        }
    }

    upload(): void {
        const file = this.selectedFile();
        const sid = this.sessionId();
        if (!file || !sid) return;

        this.uploading.set(true);
        this.sessionService
            .uploadVoiceMemo(sid, file)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: voiceMemo => {
                    this.uploading.set(false);
                    this.notification.success('تم رفع المذكرة الصوتية بنجاح');
                    this.uploaded.emit(voiceMemo.transcript || '');
                    this.selectedFile.set(null);
                },
                error: (err: HttpErrorResponse) => {
                    this.uploading.set(false);
                    this.notification.error(err.error?.message || err.error?.error || 'فشل رفع المذكرة الصوتية');
                },
            });
    }
}
