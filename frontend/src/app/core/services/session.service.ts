import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import {
    Session,
    SessionNote,
    VoiceMemo,
    CreateSessionRequest,
    UpdateSessionRequest,
} from '../models';

@Injectable({
    providedIn: 'root',
})
export class SessionService {
    private http = inject(HttpClientService);

    getSessions(patientId: string): Observable<Session[]> {
        return this.http.get<Session[]>(API.sessions.byPatient(patientId));
    }

    getSession(id: string): Observable<Session> {
        return this.http.get<Session>(API.sessions.byId(id));
    }

    createSession(data: CreateSessionRequest): Observable<Session> {
        return this.http.post<Session>(API.sessions.base, data);
    }

    updateSession(id: string, data: UpdateSessionRequest): Observable<Session> {
        return this.http.put<Session>(API.sessions.byId(id), data);
    }

    deleteSession(id: string): Observable<void> {
        return this.http.delete<void>(API.sessions.byId(id));
    }

    uploadVoiceMemo(sessionId: string, file: File): Observable<VoiceMemo> {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.upload<VoiceMemo>(API.sessions.voice(sessionId), formData);
    }

    getSummary(sessionId: string): Observable<{ summary: string }> {
        return this.http.get<{ summary: string }>(API.sessions.summary(sessionId));
    }

    getSessionNote(sessionId: string): Observable<SessionNote> {
        return this.http.get<SessionNote>(`${API.sessions.byId(sessionId)}/note`);
    }

    saveSessionNote(sessionId: string, note: Partial<SessionNote>): Observable<SessionNote> {
        return this.http.post<SessionNote>(`${API.sessions.byId(sessionId)}/note`, note);
    }

    getVoiceMemos(sessionId: string): Observable<VoiceMemo[]> {
        return this.http.get<VoiceMemo[]>(`${API.sessions.byId(sessionId)}/voice`);
    }

    deleteVoiceMemo(sessionId: string, voiceMemoId: string): Observable<void> {
        return this.http.delete<void>(`${API.sessions.byId(sessionId)}/voice/${voiceMemoId}`);
    }
}
