export interface AdminUser {
    id: string;
    email: string;
    fullName: string;
    roles: string[];
    isActive: boolean;
    isLockedOut: boolean;
    lastLoginAt: string | null;
    createdAt: string;
}
