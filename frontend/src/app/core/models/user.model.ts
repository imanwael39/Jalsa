export interface User {
    id: string;
    email: string;
    isActive: boolean;
    lastLoginAt: string | null;
    createdAt: string;
    updatedAt: string;
}

export interface Therapist {
    id: string;
    userId: string;
    fullName: string;
    licenseNumber: string;
    specialization: string | null;
    phone: string | null;
    profileImageUrl: string | null;
    bio: string | null;
    createdAt: string;
}
