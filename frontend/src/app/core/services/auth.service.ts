import { Injectable, signal } from '@angular/core';

export type UserRole = 'Therapist' | 'Patient' | 'Administrator';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private authenticatedSignal = signal(false);
    private rolesSignal = signal<UserRole[]>([]);

    isAuthenticated(): boolean {
        return this.authenticatedSignal();
    }

    hasRole(role: string): boolean {
        return this.rolesSignal().includes(role as UserRole);
    }

    hasAnyRole(roles: string[]): boolean {
        const userRoles = this.rolesSignal();
        return roles.some((role) => userRoles.includes(role as UserRole));
    }

    getRoles(): UserRole[] {
        return this.rolesSignal();
    }

    setRoles(roles: UserRole[]): void {
        this.rolesSignal.set(roles);
    }

    getToken(): string | null {
        return localStorage.getItem('jwt_token');
    }

    logout(): void {
        localStorage.removeItem('jwt_token');
        this.authenticatedSignal.set(false);
        this.rolesSignal.set([]);
    }
}
