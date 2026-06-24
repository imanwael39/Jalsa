export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    token: string;
    refreshToken: string;
    expiresAt: string;
}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    role: string;
}

export interface RegisterResponse {
    userId: string;
}

export interface RefreshTokenRequest {
    refreshToken: string;
}

export interface RefreshTokenResponse {
    token: string;
    refreshToken: string;
    expiresAt: string;
}

export interface User {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
    isActive: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface UpdateProfileRequest {
    firstName?: string;
    lastName?: string;
    email?: string;
}
