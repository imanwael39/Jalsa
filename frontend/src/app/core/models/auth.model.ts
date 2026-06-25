export interface LoginRequest {
    email: string;
    password: string;
}

export interface AuthResponse {
    token: string;
    expiresAt: string;
    refreshToken: string;
    refreshTokenExpiresAt: string;
    email: string;
    roles: string[];
}

export interface RegisterRequest {
    email: string;
    password: string;
    role?: string;
}

export interface RefreshTokenRequest {
    refreshToken: string;
}

export interface RevokeTokenRequest {
    refreshToken: string;
}

export interface ForgotPasswordRequest {
    email: string;
}

export interface ResetPasswordRequest {
    email: string;
    otp: string;
    newPassword: string;
}
