import {
    Component,
    ChangeDetectionStrategy,
    inject,
    OnInit,
    OnDestroy,
    DestroyRef,
    signal,
    ElementRef,
    viewChild,
    AfterViewChecked,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import * as signalR from '@microsoft/signalr';
import { HttpClientService } from '../../../../core/api/http-client.service';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { API } from '../../../../core/api/api-endpoints';
import { environment } from '../../../../../environments/environment';

export interface ChatMessage {
    id: string;
    conversationId: string;
    senderType: 'Patient' | 'AI' | 'Therapist';
    content: string | null;
    createdAt: string;
}

interface ChatHistoryResponse {
    conversationId: string;
    patientId: string;
    patientName: string;
    messages: ChatMessage[];
}

@Component({
    selector: 'app-chat-room',
    standalone: true,
    imports: [FormsModule, SpinnerComponent],
    templateUrl: './chat-room.component.html',
    styleUrl: './chat-room.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatRoomComponent implements OnInit, OnDestroy, AfterViewChecked {
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private http = inject(HttpClientService);
    private destroyRef = inject(DestroyRef);

    private messagesEndRef = viewChild<ElementRef<HTMLDivElement>>('messagesEnd');

    conversationId = signal<string>('');
    patientId = signal<string>('');
    patientName = signal<string>('');
    messages = signal<ChatMessage[]>([]);
    loading = signal<boolean>(false);
    connecting = signal<boolean>(false);
    error = signal<string | null>(null);
    messageText = signal<string>('');
    sending = signal<boolean>(false);

    private hubConnection: signalR.HubConnection | null = null;
    private shouldScrollToBottom = false;

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
            .build();

        this.hubConnection.on('ReceiveMessage', (sender: string, content: string) => {
            const incoming: ChatMessage = {
                id: crypto.randomUUID(),
                conversationId: this.conversationId(),
                senderType: sender === 'AI' ? 'AI' : 'Patient',
                content,
                createdAt: new Date().toISOString(),
            };
            this.messages.update(prev => [...prev, incoming]);
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
        if (!text || this.sending()) return;

        this.sending.set(true);

        const outgoing: ChatMessage = {
            id: crypto.randomUUID(),
            conversationId: this.conversationId(),
            senderType: 'Therapist',
            content: text,
            createdAt: new Date().toISOString(),
        };

        this.messages.update(prev => [...prev, outgoing]);
        this.messageText.set('');
        this.shouldScrollToBottom = true;

        try {
            if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
                await this.hubConnection.invoke('SendMessage', this.conversationId(), this.patientId(), text);
            } else {
                await this.http
                    .post<void>(API.chat.send, {
                        conversationId: this.conversationId(),
                        patientId: this.patientId(),
                        message: text,
                    })
                    .toPromise();
            }
        } catch {
            this.error.set('فشل إرسال الرسالة. يرجى المحاولة مرة أخرى.');
        } finally {
            this.sending.set(false);
        }
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
