import { TestBed } from '@angular/core/testing';
import { AdminUsersStateService } from './admin-users-state.service';
import { AdminUser } from '../models';

function createUser(overrides: Partial<AdminUser> = {}): AdminUser {
    return {
        id: '1',
        email: 'user@test.com',
        fullName: 'Test User',
        roles: ['Therapist'],
        isActive: true,
        isDeleted: false,
        isLockedOut: false,
        lastLoginAt: null,
        createdAt: '2026-01-01T00:00:00Z',
        ...overrides,
    };
}

describe('AdminUsersStateService', () => {
    let service: AdminUsersStateService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(AdminUsersStateService);
    });

    it('should be created with empty initial state', () => {
        expect(service.users()).toEqual([]);
        expect(service.totalCount()).toBe(0);
        expect(service.loading()).toBe(false);
        expect(service.error()).toBeNull();
    });

    describe('setUsers', () => {
        it('sets the users list and total count', () => {
            const users = [createUser({ id: '1' }), createUser({ id: '2' })];
            service.setUsers(users, 2);
            expect(service.users()).toEqual(users);
            expect(service.totalCount()).toBe(2);
        });

        it('clears error when setting users', () => {
            service.setError('boom');
            service.setUsers([], 0);
            expect(service.error()).toBeNull();
        });
    });

    describe('updateUser', () => {
        it('updates a user by id in place', () => {
            const original = createUser({ id: '1', isActive: true });
            service.setUsers([original], 1);
            service.updateUser(createUser({ id: '1', isActive: false }));
            expect(service.users()[0].isActive).toBe(false);
        });

        it('leaves other users untouched', () => {
            const u1 = createUser({ id: '1' });
            const u2 = createUser({ id: '2', fullName: 'Other' });
            service.setUsers([u1, u2], 2);
            service.updateUser(createUser({ id: '1', fullName: 'Changed' }));
            expect(service.users()[1].fullName).toBe('Other');
        });
    });

    describe('reset', () => {
        it('clears all state', () => {
            service.setUsers([createUser()], 1);
            service.setLoading(true);
            service.setError('err');

            service.reset();

            expect(service.users()).toEqual([]);
            expect(service.totalCount()).toBe(0);
            expect(service.loading()).toBe(false);
            expect(service.error()).toBeNull();
        });
    });
});
