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
}
