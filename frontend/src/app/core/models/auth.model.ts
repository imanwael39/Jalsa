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

export interface LoginResponse {
    token: string;
}

export interface RegisterRequest {
    fullName: string;
    email: string;
    password: string;
    licenseNumber?: string;
    specialization?: string;
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

export interface VerifyOtpRequest {
    email: string;
    otp: string;
}

export interface ChangePasswordRequest {
    currentPassword: string;
    newPassword: string;
}

export interface UpdateProfileRequest {
    firstName?: string;
    lastName?: string;
    email?: string;
}
