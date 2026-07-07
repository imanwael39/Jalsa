import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpClientService } from '../../../../core/api/http-client.service';
import { PatientService } from '../../../../core/services/patient.service';
import { AuthService } from '../../../../core/services/auth.service';
import { API } from '../../../../core/api/api-endpoints';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { ChatConversation, Patient } from '../../../../core/models';

const LAST_SEEN_KEY = 'jalsa_chat_last_seen';

@Component({
    selector: 'app-chat-list',
    standalone: true,
    imports: [FormsModule, SpinnerComponent, EmptyStateComponent],
    templateUrl: './chat-list.component.html',
    styleUrl: './chat-list.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatListComponent implements OnInit {
    private router = inject(Router);
    private http = inject(HttpClientService);
    private patientService = inject(PatientService);
    private authService = inject(AuthService);
    private destroyRef = inject(DestroyRef);

    conversations = signal<ChatConversation[]>([]);
    loading = signal<boolean>(false);
    error = signal<string | null>(null);
    searchTerm = signal<string>('');

    showPatientPicker = signal<boolean>(false);
    patients = signal<Patient[]>([]);
    patientSearchTerm = signal<string>('');
    patientPickerLoading = signal<boolean>(false);
    creatingConversation = signal<boolean>(false);

    isTherapist = computed(() => this.authService.hasRole('Therapist'));

    filteredConversations = computed(() => {
        const term = this.searchTerm().trim().toLowerCase();
        if (!term) return this.conversations();
        return this.conversations().filter(c => c.patientName?.toLowerCase().includes(term));
    });

    filteredPatients = computed(() => {
        const term = this.patientSearchTerm().trim().toLowerCase();
        if (!term) return this.patients();
        return this.patients().filter(p => p.fullName.toLowerCase().includes(term));
    });

    ngOnInit(): void {
        this.loadConversations();
    }

    loadConversations(): void {
        this.loading.set(true);
        this.error.set(null);

        this.http
            .get<ChatConversation[]>(API.chat.conversations)
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

    updateSearchTerm(value: string): void {
        this.searchTerm.set(value);
    }

    openConversation(id: string): void {
        this.markSeen(id);
        this.router.navigate(['/chatbot', id]);
    }

    openPatientPicker(): void {
        this.showPatientPicker.set(true);
        if (this.patients().length === 0) {
            this.patientPickerLoading.set(true);
            this.patientService
                .getPatients({ page: 1, pageSize: 100, status: 'Active' })
                .pipe(takeUntilDestroyed(this.destroyRef))
                .subscribe({
                    next: data => {
                        this.patients.set(data);
                        this.patientPickerLoading.set(false);
                    },
                    error: () => this.patientPickerLoading.set(false),
                });
        }
    }

    closePatientPicker(): void {
        this.showPatientPicker.set(false);
        this.patientSearchTerm.set('');
    }

    updatePatientSearchTerm(value: string): void {
        this.patientSearchTerm.set(value);
    }

    selectPatient(patient: Patient): void {
        if (this.creatingConversation()) return;

        const existing = this.conversations().find(c => c.patientId === patient.id);
        if (existing) {
            this.closePatientPicker();
            this.openConversation(existing.id);
            return;
        }

        this.creatingConversation.set(true);
        this.http
            .post<ChatConversation>(API.chat.conversations, { patientId: patient.id })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: conv => {
                    this.creatingConversation.set(false);
                    this.closePatientPicker();
                    this.conversations.update(prev => [conv, ...prev]);
                    this.openConversation(conv.id);
                },
                error: () => {
                    this.creatingConversation.set(false);
                    this.error.set('فشل إنشاء محادثة جديدة.');
                },
            });
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

    isUnread(conv: ChatConversation): boolean {
        if (!conv.lastActivityAt) return false;
        const lastSeen = this.getLastSeenMap()[conv.id];
        if (!lastSeen) return true;
        return new Date(conv.lastActivityAt).getTime() > new Date(lastSeen).getTime();
    }

    private markSeen(conversationId: string): void {
        const map = this.getLastSeenMap();
        map[conversationId] = new Date().toISOString();
        localStorage.setItem(LAST_SEEN_KEY, JSON.stringify(map));
    }

    private getLastSeenMap(): Record<string, string> {
        try {
            const raw = localStorage.getItem(LAST_SEEN_KEY);
            return raw ? JSON.parse(raw) : {};
        } catch {
            return {};
        }
    }
}
