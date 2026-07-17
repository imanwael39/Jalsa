import { ChangeDetectionStrategy, Component, computed, inject, input, signal } from '@angular/core';
import { HttpClientService } from '../../../../core/api/http-client.service';
import { API } from '../../../../core/api/api-endpoints';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { AiGenerationDiagnostics } from '../../../../core/models';

type ToolAction = 'summary' | 'report' | 'chat' | null;

@Component({
    selector: 'app-ai-testing-tools',
    standalone: true,
    imports: [SpinnerComponent],
    templateUrl: './ai-testing-tools.component.html',
    styleUrl: './ai-testing-tools.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AiTestingToolsComponent {
    private http = inject(HttpClientService);

    patientId = input.required<string>();
    conversationId = input<string | null>(null);
    lastQuestion = input<string | null>(null);

    expanded = signal<boolean>(false);
    loading = signal<ToolAction>(null);
    error = signal<string | null>(null);
    result = signal<AiGenerationDiagnostics | null>(null);
    resultLabel = signal<string>('');
    embeddingCount = signal<number | null>(null);

    hasResult = computed(() => this.result() !== null);

    toggle(): void {
        this.expanded.update(v => !v);
    }

    generateSummary(): void {
        this.run('summary', 'ملخص المريض', () =>
            this.http.get<AiGenerationDiagnostics>(API.ai.diagnosticsSummary(this.patientId(), 'ar'))
        );
    }

    generateReport(): void {
        this.run('report', 'مسودة التقرير', () =>
            this.http.get<AiGenerationDiagnostics>(API.ai.diagnosticsReportDraft(this.patientId(), 'ar'))
        );
    }

    generateChatAnswer(): void {
        const question = this.lastQuestion();
        const conversationId = this.conversationId();
        if (!question || !conversationId) return;

        this.run('chat', 'رد الدردشة (تشخيصي)', () =>
            this.http.post<AiGenerationDiagnostics>(API.ai.diagnosticsChat, {
                conversationId,
                patientId: this.patientId(),
                question,
                language: /[؀-ۿ]/.test(question) ? 'ar' : 'en',
            })
        );
    }

    loadEmbeddingCount(): void {
        this.http.get<{ embeddingCount: number }>(API.ai.diagnosticsEmbeddingCount(this.patientId())).subscribe({
            next: res => this.embeddingCount.set(res.embeddingCount),
            error: () => this.embeddingCount.set(null),
        });
    }

    private run(action: ToolAction, label: string, fn: () => import('rxjs').Observable<AiGenerationDiagnostics>): void {
        this.loading.set(action);
        this.error.set(null);
        this.resultLabel.set(label);

        fn().subscribe({
            next: res => {
                this.result.set(res);
                this.loading.set(null);
                this.embeddingCount.set(res.embeddingCount);
            },
            error: () => {
                this.error.set('فشل تنفيذ الأداة التشخيصية.');
                this.loading.set(null);
            },
        });
    }

    async copyOutput(): Promise<void> {
        const output = this.result()?.output;
        if (output) {
            await navigator.clipboard.writeText(output);
        }
    }
}
