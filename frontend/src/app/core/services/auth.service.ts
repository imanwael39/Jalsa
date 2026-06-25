import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private authenticatedSignal = signal(false);

    isAuthenticated(): boolean {
        return this.authenticatedSignal();
    }

    hasRole(_role: string): boolean {
        return false;
    }

    getToken(): string | null {
        return localStorage.getItem('jwt_token');
    }

    logout(): void {
        localStorage.removeItem('jwt_token');
        this.authenticatedSignal.set(false);
    }
}
