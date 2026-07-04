import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { of, throwError } from 'rxjs';
import { ChatRoomComponent } from './chat-room.component';
import { HttpClientService } from '../../../../core/api/http-client.service';
import { ChatMessage } from 'src/app/core/models/chat.model';

const mockHistory = {
    conversationId: 'conv-1',
    patientId: 'pat-1',
    patientName: 'أحمد محمد',
    messages: [
        {
            id: 'msg-1',
            conversationId: 'conv-1',
            senderType: 'Patient',
            content: 'مرحبا',
            createdAt: '2024-01-15T10:00:00Z',
        },
        {
            id: 'msg-2',
            conversationId: 'conv-1',
            senderType: 'AI',
            content: 'كيف يمكنني مساعدتك؟',
            createdAt: '2024-01-15T10:01:00Z',
        },
    ],
};

interface SetupOptions {
    routeId?: string | null;
    getReturn?: ReturnType<typeof of | typeof throwError>;
}

describe('ChatRoomComponent', () => {
    let component: ChatRoomComponent;
    let fixture: ComponentFixture<ChatRoomComponent>;
    let httpSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { routeId = 'conv-1', getReturn } = opts;

        httpSpy = {
            get: vi.fn().mockReturnValue(getReturn ?? of(mockHistory)),
            post: vi.fn().mockReturnValue(of({})),
        };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: HttpClientService, useValue: httpSpy },
                { provide: Router, useValue: routerSpy },
                {
                    provide: ActivatedRoute,
                    useValue: {
                        snapshot: {
                            paramMap: {
                                get: (key: string): string | null => (key === 'id' ? routeId : null),
                            },
                        },
                    },
                },
            ],
        });

        fixture = TestBed.createComponent(ChatRoomComponent);
        component = fixture.componentInstance;
    };

    beforeAll((): void => {
        Element.prototype.scrollIntoView = vi.fn();
    });

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load history on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(httpSpy['get']).toHaveBeenCalled();
        expect(component.conversationId()).toBe('conv-1');
        expect(component.patientName()).toBe('أحمد محمد');
    });

    it('should handle load error when conversation ID not found', (): void => {
        setup({ routeId: null, getReturn: throwError((): Error => new Error('Not found')) });
        fixture.detectChanges();
        expect(component.error()).toBe('فشل تحميل المحادثة. يرجى المحاولة مرة أخرى.');
    });

    it('should handle load error', (): void => {
        setup({ getReturn: throwError((): Error => new Error('Load failed')) });
        fixture.detectChanges();
        expect(component.error()).toBe('فشل تحميل المحادثة. يرجى المحاولة مرة أخرى.');
    });

    it('should navigate back to chat list', (): void => {
        setup();
        fixture.detectChanges();
        component.goBack();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/chatbot']);
    });

    it('should identify AI messages', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.isAiMessage({ senderType: 'AI' } as ChatMessage)).toBe(true);
        expect(component.isAiMessage({ senderType: 'Patient' } as ChatMessage)).toBe(false);
    });

    it('should identify therapist messages', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.isTherapistMessage({ senderType: 'Therapist' } as ChatMessage)).toBe(true);
        expect(component.isTherapistMessage({ senderType: 'Patient' } as ChatMessage)).toBe(false);
    });

    it('should update message text', (): void => {
        setup();
        fixture.detectChanges();
        component.updateMessageText('Hello');
        expect(component.messageText()).toBe('Hello');
    });

    it('should format time', (): void => {
        setup();
        expect(typeof component.formatTime('2024-01-15T10:00:00Z')).toBe('string');
    });

    it('should send via the REST endpoint with the correct payload when not connected via SignalR', async () => {
        setup();
        fixture.detectChanges();
        component.updateMessageText('مرحباً');
        await component.sendMessage();

        expect(httpSpy['post']).toHaveBeenCalledWith('/api/chat/send', {
            conversationId: 'conv-1',
            content: 'مرحباً',
        });
    });

    it('should optimistically append the outgoing message and clear the input', async () => {
        setup();
        fixture.detectChanges();
        component.updateMessageText('مرحباً');
        await component.sendMessage();

        expect(component.messages()).toEqual(expect.arrayContaining([expect.objectContaining({ content: 'مرحباً' })]));
        expect(component.messageText()).toBe('');
    });
});
