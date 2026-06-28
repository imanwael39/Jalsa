import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal } from '@angular/core';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DatePipe } from '@angular/common';
import { HttpClientService } from '../../../../core/api/http-client.service';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';

export interface ChatConversation {
    id: string;
    patientId: string;
    patientName: string;
    status: string;
    lastActivityAt: string | null;
    createdAt: string;
    messageCount: number;
}

@Component({
    selector: 'app-chat-list',
    standalone: true,
    imports: [SpinnerComponent, EmptyStateComponent, DatePipe],
    templateUrl: './chat-list.component.html',
    styleUrl: './chat-list.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatListComponent implements OnInit {
    private router = inject(Router);
    private http = inject(HttpClientService);
    private destroyRef = inject(DestroyRef);

    conversations = signal<ChatConversation[]>([]);
    loading = signal<boolean>(false);
    error = signal<string | null>(null);

    ngOnInit(): void {
        this.loadConversations();
    }

    loadConversations(): void {
        this.loading.set(true);
        this.error.set(null);

        this.http
            .get<ChatConversation[]>('/api/chat/conversations')
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.conversations.set(data);
                    this.loading.set(false);
                },
                error: () => {
                    this.error.set('فشل تحميل المحادثات. يرجى المحاولة مرة أخرى.');
                    this.loading.set(false);
                },
            });
    }

    openConversation(id: string): void {
        this.router.navigate(['/chatbot', id]);
    }

    formatDate(date: string | null): string {
        if (!date) return '-';
        return new Date(date).toLocaleDateString('ar-EG', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
        });
    }

    getStatusLabel(status: string): string {
        return status === 'Open' ? 'مفتوحة' : 'مغلقة';
    }

    isOpenStatus(status: string): boolean {
        return status === 'Open';
    }
}
