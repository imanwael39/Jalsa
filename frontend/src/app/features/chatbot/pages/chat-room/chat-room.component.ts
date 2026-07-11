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
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import * as signalR from '@microsoft/signalr';
import { HttpClientService } from '../../../../core/api/http-client.service';
import { AuthService } from '../../../../core/services/auth.service';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { AiDisclaimerComponent } from '../../../../shared/components/ai-disclaimer/ai-disclaimer.component';
import { MarkdownPipe } from '../../../../shared/pipes';
import { AiTestingToolsComponent } from '../../components/ai-testing-tools/ai-testing-tools.component';
import { API } from '../../../../core/api/api-endpoints';
import { environment } from '../../../../../environments/environment';
import { ChatMessage } from '../../../../core/models';

interface ChatHistoryResponse {
    conversationId: string;
    patientId: string;
    patientName: string;
    status: string;
    messages: ChatMessage[];
}

interface DisplayMessage extends ChatMessage {
    failed?: boolean;
    pendingText?: string;
}

const ARABIC_RANGE = /[؀-ۿ]/;

@Component({
    selector: 'app-chat-room',
    standalone: true,
    imports: [FormsModule, SpinnerComponent, AiDisclaimerComponent, MarkdownPipe, AiTestingToolsComponent],
    templateUrl: './chat-room.component.html',
    styleUrl: './chat-room.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatRoomComponent implements OnInit, OnDestroy, AfterViewChecked {
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private http = inject(HttpClientService);
    private authService = inject(AuthService);
    private destroyRef = inject(DestroyRef);

    private messagesEndRef = viewChild<ElementRef<HTMLDivElement>>('messagesEnd');

    conversationId = signal<string>('');
    patientId = signal<string>('');
    patientName = signal<string>('');
    conversationStatus = signal<string>('Open');
    messages = signal<DisplayMessage[]>([]);
    loading = signal<boolean>(false);
    connecting = signal<boolean>(false);
    error = signal<string | null>(null);
    messageText = signal<string>('');
    sending = signal<boolean>(false);
    aiTyping = signal<boolean>(false);
    regenerating = signal<boolean>(false);
    copiedMessageId = signal<string | null>(null);
    streamingText = signal<string>('');
    isStreaming = computed(() => this.streamingText().length > 0);

    isTherapist = computed(() => this.authService.hasRole('Therapist'));
    isClosed = computed(() => this.conversationStatus() !== 'Open');
    lastTherapistQuestion = computed(() => {
        const msgs = this.messages();
        for (let i = msgs.length - 1; i >= 0; i--) {
            if (msgs[i].senderType === 'Therapist' && msgs[i].content) return msgs[i].content;
        }
        return null;
    });
    canRegenerate = computed(() => {
        const msgs = this.messages();
        return this.isTherapist() && !this.isClosed() && msgs.length > 0 && msgs[msgs.length - 1].senderType === 'AI';
    });

    private hubConnection: signalR.HubConnection | null = null;
    private shouldScrollToBottom = false;
    private aiTypingTimeout: ReturnType<typeof setTimeout> | null = null;

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id') ?? '';
        this.conversationId.set(id);
        this.loadHistory(id);
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
        this.loading.set(true);
        this.error.set(null);

        this.http
            .get<ChatHistoryResponse>(API.chat.history(conversationId))
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.patientId.set(data.patientId);
                    this.patientName.set(data.patientName);
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
            .withUrl(environment.signalRHubUrl, {
                accessTokenFactory: () => localStorage.getItem('jalsa_token') ?? '',
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Warning)
            .build();

        // Incremental text as the AI response streams in from Gemini. Accumulated locally
        // and rendered as a live-growing bubble until the final 'ReceiveMessage'/
        // 'ReceiveHumanMessage' event replaces it with the persisted message.
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

        // Broadcast for messages sent via the REST endpoint (Therapist replies, or a
        // Patient fallback when the SignalR connection is down). Guarded by id so a
        // sender doesn't see their own message twice.
        this.hubConnection.on('ReceiveHumanMessage', (incoming: ChatMessage) => {
            if (incoming.senderType === 'AI') {
                this.clearAiTyping();
                this.streamingText.set('');
            }
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

        const isPatient = this.authService.hasRole('Patient');
        this.messageText.set('');

        const localId = crypto.randomUUID();

        try {
            if (isPatient && this.hubConnection?.state === signalR.HubConnectionState.Connected) {
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
                await this.hubConnection.invoke('SendMessage', this.conversationId(), this.patientId(), text);
                this.setAiTyping();
            } else {
                if (this.isTherapist()) this.setAiTyping();
                // Rendered once the 'ReceiveHumanMessage' broadcast arrives, since the
                // sender is also a member of this conversation's SignalR group.
                await this.http
                    .post<void>(API.chat.send, {
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
                    senderType: isPatient ? 'Patient' : 'Therapist',
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

    regenerateLastResponse(): void {
        if (!this.canRegenerate() || this.regenerating()) return;

        this.regenerating.set(true);
        this.error.set(null);
        this.setAiTyping();

        this.http
            .post<ChatMessage>(API.chat.regenerate(this.conversationId()), {})
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: msg => {
                    this.clearAiTyping();
                    this.streamingText.set('');
                    this.messages.update(prev => (prev.some(m => m.id === msg.id) ? prev : [...prev, msg]));
                    this.shouldScrollToBottom = true;
                    this.regenerating.set(false);
                },
                error: () => {
                    this.clearAiTyping();
                    this.streamingText.set('');
                    this.error.set('فشل إعادة توليد الرد.');
                    this.regenerating.set(false);
                },
            });
    }

    clearConversation(): void {
        if (this.isClosed()) return;
        if (!confirm('هل تريد إغلاق هذه المحادثة؟ لن تتمكن من إرسال رسائل جديدة بعد ذلك.')) return;

        this.http
            .patch<{ status: string }>(API.chat.close(this.conversationId()), {})
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: res => this.conversationStatus.set(res.status ?? 'Closed'),
                error: () => this.error.set('فشل إغلاق المحادثة.'),
            });
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
        this.router.navigate(['/chatbot']);
    }

    formatTime(dateStr: string): string {
        return new Date(dateStr).toLocaleTimeString('ar-EG', {
            hour: '2-digit',
            minute: '2-digit',
        });
    }

    isAiMessage(msg: ChatMessage): boolean {
        return msg.senderType === 'AI';
    }

    isTherapistMessage(msg: ChatMessage): boolean {
        return msg.senderType === 'Therapist';
    }

    messageDir(msg: ChatMessage): 'rtl' | 'ltr' {
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
