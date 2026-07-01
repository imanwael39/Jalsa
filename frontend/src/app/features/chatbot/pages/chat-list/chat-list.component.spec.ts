import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { of, throwError } from 'rxjs';
import { ChatListComponent } from './chat-list.component';
import { HttpClientService } from '../../../../core/api/http-client.service';

const mockConversations = [
    {
        id: 'conv-1',
        patientId: 'pat-1',
        patientName: 'أحمد محمد',
        status: 'Open',
        lastActivityAt: '2024-01-15T10:00:00Z',
        createdAt: '2024-01-15T09:00:00Z',
        messageCount: 5,
    },
    {
        id: 'conv-2',
        patientId: 'pat-2',
        patientName: 'سارة أحمد',
        status: 'Closed',
        lastActivityAt: '2024-01-14T10:00:00Z',
        createdAt: '2024-01-14T09:00:00Z',
        messageCount: 3,
    },
];

interface SetupOptions {
    getReturn?: ReturnType<typeof of | typeof throwError>;
}

describe('ChatListComponent', () => {
    let component: ChatListComponent;
    let fixture: ComponentFixture<ChatListComponent>;
    let httpSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { getReturn } = opts;

        httpSpy = {
            get: vi.fn().mockReturnValue(getReturn ?? of(mockConversations)),
        };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: HttpClientService, useValue: httpSpy },
                { provide: Router, useValue: routerSpy },
            ],
        });

        fixture = TestBed.createComponent(ChatListComponent);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load conversations on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(httpSpy['get']).toHaveBeenCalledWith('/api/chat/conversations');
        expect(component.conversations().length).toBe(2);
    });

    it('should handle load error', (): void => {
        setup({ getReturn: throwError((): Error => new Error('Load failed')) });
        fixture.detectChanges();
        expect(component.error()).toBe('فشل تحميل المحادثات. يرجى المحاولة مرة أخرى.');
        expect(component.loading()).toBe(false);
    });

    it('should navigate to conversation room', (): void => {
        setup();
        fixture.detectChanges();
        component.openConversation('conv-1');
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/chatbot', 'conv-1']);
    });

    it('should format date', (): void => {
        setup();
        expect(typeof component.formatDate('2024-01-15T10:00:00Z')).toBe('string');
        expect(component.formatDate(null)).toBe('-');
    });

    it('should return correct status label', (): void => {
        setup();
        expect(component.getStatusLabel('Open')).toBe('مفتوحة');
        expect(component.getStatusLabel('Closed')).toBe('مغلقة');
    });

    it('should check open status', (): void => {
        setup();
        expect(component.isOpenStatus('Open')).toBe(true);
        expect(component.isOpenStatus('Closed')).toBe(false);
    });
});
