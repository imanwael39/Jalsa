import { Injectable, signal } from '@angular/core';
import { AdminUser } from '../models';

@Injectable({
    providedIn: 'root',
})
export class AdminUsersStateService {
    private usersSignal = signal<AdminUser[]>([]);
    private totalCountSignal = signal(0);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly users = this.usersSignal.asReadonly();
    readonly totalCount = this.totalCountSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    setUsers(users: AdminUser[], totalCount: number): void {
        this.usersSignal.set(users);
        this.totalCountSignal.set(totalCount);
        this.errorSignal.set(null);
    }

    updateUser(updated: AdminUser): void {
        this.usersSignal.update(list => list.map(u => (u.id === updated.id ? updated : u)));
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.usersSignal.set([]);
        this.totalCountSignal.set(0);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
