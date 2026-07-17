import {
    Component,
    ChangeDetectionStrategy,
    inject,
    OnInit,
    OnDestroy,
    DestroyRef,
    signal,
    computed,
    ElementRef,
    viewChild,
    AfterViewChecked,
} from '@angular/core';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import * as signalR from '@microsoft/signalr';
import { HttpClientService } from '../../../../core/api/http-client.service';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { AiDisclaimerComponent } from '../../../../shared/components/ai-disclaimer/ai-disclaimer.component';
import { MarkdownPipe } from '../../../../shared/pipes';
import { API } from '../../../../core/api/api-endpoints';
import { environment } from '../../../../../environments/environment';
import { PatientSupportChatMessage, PatientSupportConversation } from '../../../../core/models';

interface PatientSupportChatHistoryResponse {
    conversationId: string;
    status: string;
    messages: PatientSupportChatMessage[];
}

interface DisplayMessage extends PatientSupportChatMessage {
    failed?: boolean;
    pendingText?: string;
}

const ARABIC_RANGE = /[؀-ۿ]/;

/**
 * Patient-only emotional support chat. Sends always go over the live SignalR connection
 * (patientSupportChatHubUrl) — this is intentional: the AI reply is only generated inside
 * the hub's SendMessage handler, which has no dependency on clinical patient context.
 */
@Component({
    selector: 'app-patient-support-chat-room',
    standalone: true,
    imports: [FormsModule, SpinnerComponent, AiDisclaimerComponent, MarkdownPipe],
    templateUrl: './patient-support-chat-room.component.html',
    styleUrl: './patient-support-chat-room.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientSupportChatRoomComponent implements OnInit, OnDestroy, AfterViewChecked {
    private router = inject(Router);
    private http = inject(HttpClientService);
    private destroyRef = inject(DestroyRef);

    private messagesEndRef = viewChild<ElementRef<HTMLDivElement>>('messagesEnd');

    conversationId = signal<string>('');
    conversationStatus = signal<string>('Open');
    messages = signal<DisplayMessage[]>([]);
    loading = signal<boolean>(false);
    connecting = signal<boolean>(false);
    error = signal<string | null>(null);
    messageText = signal<string>('');
    sending = signal<boolean>(false);
    aiTyping = signal<boolean>(false);
    copiedMessageId = signal<string | null>(null);
    streamingText = signal<string>('');
    isStreaming = computed(() => this.streamingText().length > 0);

    isClosed = computed(() => this.conversationStatus() !== 'Open');

    private hubConnection: signalR.HubConnection | null = null;
    private shouldScrollToBottom = false;
    private aiTypingTimeout: ReturnType<typeof setTimeout> | null = null;

    ngOnInit(): void {
        this.loading.set(true);
        this.error.set(null);

        this.http
            .get<PatientSupportConversation>(API.supportChat.conversation)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: conv => {
                    this.conversationId.set(conv.id);
                    this.conversationStatus.set(conv.status ?? 'Open');
                    this.loadHistory(conv.id);
                },
                error: () => {
                    this.error.set('فشل تحميل المحادثة. يرجى المحاولة مرة أخرى.');
                    this.loading.set(false);
                },
            });
    }

    ngAfterViewChecked(): void {
        if (this.shouldScrollToBottom) {
            this.scrollToBottom();
            this.shouldScrollToBottom = false;
        }
    }

    ngOnDestroy(): void {
        this.stopConnection();
        if (this.aiTypingTimeout) clearTimeout(this.aiTypingTimeout);
    }

    loadHistory(conversationId: string): void {
        this.http
            .get<PatientSupportChatHistoryResponse>(API.supportChat.history(conversationId))
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.conversationStatus.set(data.status ?? 'Open');
                    this.messages.set(data.messages ?? []);
                    this.loading.set(false);
                    this.shouldScrollToBottom = true;
                    this.startConnection();
                },
                error: () => {
                    this.error.set('فشل تحميل المحادثة. يرجى المحاولة مرة أخرى.');
                    this.loading.set(false);
                },
            });
    }

    async startConnection(): Promise<void> {
        this.connecting.set(true);

        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(environment.patientSupportChatHubUrl, {
                accessTokenFactory: () => localStorage.getItem('jalsa_token') ?? '',
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Warning)
            .build();

        this.hubConnection.on('ReceiveMessageChunk', (delta: string) => {
            this.aiTyping.set(false);
            this.streamingText.update(prev => prev + delta);
            this.shouldScrollToBottom = true;
        });

        this.hubConnection.on('ReceiveMessage', (_sender: string, content: string) => {
            const incoming: DisplayMessage = {
                id: crypto.randomUUID(),
                conversationId: this.conversationId(),
                senderType: 'AI',
                content,
                createdAt: new Date().toISOString(),
            };
            this.clearAiTyping();
            this.streamingText.set('');
            this.messages.update(prev => [...prev, incoming]);
            this.shouldScrollToBottom = true;
        });

        // Broadcast for the REST fallback path (used only if the SignalR connection is down).
        this.hubConnection.on('ReceiveHumanMessage', (incoming: PatientSupportChatMessage) => {
            this.messages.update(prev => (prev.some(m => m.id === incoming.id) ? prev : [...prev, incoming]));
            this.shouldScrollToBottom = true;
        });

        try {
            await this.hubConnection.start();
            await this.hubConnection.invoke('JoinConversation', this.conversationId());
            this.connecting.set(false);
        } catch {
            this.connecting.set(false);
            this.error.set('فشل الاتصال بالمحادثة المباشرة. المحادثة تعمل في وضع محدود.');
        }
    }

    async sendMessage(): Promise<void> {
        const text = this.messageText().trim();
        if (!text || this.sending() || this.isClosed()) return;

        await this.dispatchMessage(text);
    }

    async retryMessage(msg: DisplayMessage): Promise<void> {
        if (!msg.pendingText || this.sending()) return;
        this.messages.update(prev => prev.filter(m => m.id !== msg.id));
        await this.dispatchMessage(msg.pendingText);
    }

    private async dispatchMessage(text: string): Promise<void> {
        this.sending.set(true);
        this.error.set(null);
        this.messageText.set('');

        const localId = crypto.randomUUID();

        try {
            if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
                // Own message isn't echoed back by the hub, so append it optimistically.
                const outgoing: DisplayMessage = {
                    id: localId,
                    conversationId: this.conversationId(),
                    senderType: 'Patient',
                    content: text,
                    createdAt: new Date().toISOString(),
                };
                this.messages.update(prev => [...prev, outgoing]);
                this.shouldScrollToBottom = true;
                await this.hubConnection.invoke('SendMessage', this.conversationId(), text);
                this.setAiTyping();
            } else {
                await this.http
                    .post<void>(API.supportChat.send, {
                        conversationId: this.conversationId(),
                        content: text,
                    })
                    .toPromise();
            }
        } catch {
            this.clearAiTyping();
            this.streamingText.set('');
            this.error.set('فشل إرسال الرسالة. يرجى المحاولة مرة أخرى.');
            this.messages.update(prev => [
                ...prev,
                {
                    id: localId,
                    conversationId: this.conversationId(),
                    senderType: 'Patient',
                    content: text,
                    createdAt: new Date().toISOString(),
                    failed: true,
                    pendingText: text,
                },
            ]);
            this.shouldScrollToBottom = true;
        } finally {
            this.sending.set(false);
        }
    }

    async copyMessage(msg: DisplayMessage): Promise<void> {
        if (!msg.content) return;
        await navigator.clipboard.writeText(msg.content);
        this.copiedMessageId.set(msg.id);
        setTimeout(() => {
            if (this.copiedMessageId() === msg.id) this.copiedMessageId.set(null);
        }, 1500);
    }

    onKeyDown(event: KeyboardEvent): void {
        if (event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault();
            this.sendMessage();
        }
    }

    updateMessageText(value: string): void {
        this.messageText.set(value);
    }

    goBack(): void {
        this.router.navigate(['/']);
    }

    formatTime(dateStr: string): string {
        return new Date(dateStr).toLocaleTimeString('ar-EG', {
            hour: '2-digit',
            minute: '2-digit',
        });
    }

    isAiMessage(msg: PatientSupportChatMessage): boolean {
        return msg.senderType === 'AI';
    }

    messageDir(msg: PatientSupportChatMessage): 'rtl' | 'ltr' {
        return msg.content && ARABIC_RANGE.test(msg.content) ? 'rtl' : 'ltr';
    }

    private setAiTyping(): void {
        this.aiTyping.set(true);
        if (this.aiTypingTimeout) clearTimeout(this.aiTypingTimeout);
        this.aiTypingTimeout = setTimeout(() => this.aiTyping.set(false), 25000);
    }

    private clearAiTyping(): void {
        this.aiTyping.set(false);
        if (this.aiTypingTimeout) {
            clearTimeout(this.aiTypingTimeout);
            this.aiTypingTimeout = null;
        }
    }

    private scrollToBottom(): void {
        const el = this.messagesEndRef();
        if (el) {
            el.nativeElement.scrollIntoView({ behavior: 'smooth' });
        }
    }

    private stopConnection(): void {
        if (this.hubConnection) {
            this.hubConnection.stop().catch(() => {});
            this.hubConnection = null;
        }
    }
}
