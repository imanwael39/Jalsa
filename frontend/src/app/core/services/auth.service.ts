import { Injectable, signal, computed } from '@angular/core';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { NotificationService } from './notification.service';
import type { User } from '../models/user.model';
import type {
    AuthResponse,
    LoginRequest,
    RegisterRequest,
    UpdateProfileRequest,
    ChangePasswordRequest,
    ResetPasswordRequest,
} from '../models/auth.model';

interface DecodedToken {
    sub: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
    exp: number;
    iat: number;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
    private readonly TOKEN_KEY = 'jalsa_token';
    private readonly REFRESH_TOKEN_KEY = 'jalsa_refresh_token';

    private userSignal = signal<User | null>(null);
    private loadingSignal = signal<boolean>(false);

    readonly currentUser = this.userSignal.asReadonly();
    readonly isLoading = this.loadingSignal.asReadonly();

    readonly isAuthenticated = computed(() => this.userSignal() !== null && this.getToken() !== null);

    constructor(
        private http: HttpClientService,
        private notification: NotificationService,
    ) {
        this.restoreSession();
    }

    login(credentials: LoginRequest): Observable<AuthResponse> {
        this.loadingSignal.set(true);
        return this.http.post<AuthResponse>(API.auth.login, credentials).pipe(
            tap((response) => {
                this.handleAuthentication(response);
                this.notification.success('تم تسجيل الدخول بنجاح');
            }),
            catchError((error) => {
                this.loadingSignal.set(false);
                return throwError(() => error);
            }),
        );
    }

    register(userData: RegisterRequest): Observable<unknown> {
        this.loadingSignal.set(true);
        return this.http.post(API.auth.register, userData).pipe(
            tap(() => {
                this.loadingSignal.set(false);
                this.notification.success('تم إنشاء الحساب بنجاح');
            }),
            catchError((error) => {
                this.loadingSignal.set(false);
                return throwError(() => error);
            }),
        );
    }

    logout(): void {
        localStorage.removeItem(this.TOKEN_KEY);
        localStorage.removeItem(this.REFRESH_TOKEN_KEY);
        this.userSignal.set(null);
        this.notification.info('تم تسجيل الخروج');
    }

    refreshToken(): Observable<AuthResponse> {
        const refreshToken = this.getRefreshToken();
        return this.http.post<AuthResponse>(API.auth.refresh, { refreshToken }).pipe(
            tap((response) => {
                this.handleAuthentication(response);
            }),
        );
    }

    getProfile(): Observable<User> {
        return this.http.get<User>(API.auth.profile);
    }

    updateProfile(data: UpdateProfileRequest): Observable<User> {
        return this.http.put<User>(API.auth.profile, data).pipe(
            tap((user) => {
                this.userSignal.set(user);
                this.notification.success('تم تحديث الملف الشخصي بنجاح');
            }),
        );
    }

    changePassword(data: ChangePasswordRequest): Observable<unknown> {
        return this.http.post(API.auth.profile + '/change-password', data).pipe(
            tap(() => {
                this.notification.success('تم تغيير كلمة المرور بنجاح');
            }),
        );
    }

    forgotPassword(email: string): Observable<unknown> {
        return this.http.post(API.auth.forgotPassword, { email }).pipe(
            tap(() => {
                this.notification.success('تم إرسال رمز التحقق إلى بريدك الإلكتروني');
            }),
        );
    }

    resetPassword(data: ResetPasswordRequest): Observable<unknown> {
        return this.http.post(API.auth.resetPassword, data).pipe(
            tap(() => {
                this.notification.success('تم إعادة تعيين كلمة المرور بنجاح');
            }),
        );
    }

    getToken(): string | null {
        return localStorage.getItem(this.TOKEN_KEY);
    }

    private setToken(token: string): void {
        localStorage.setItem(this.TOKEN_KEY, token);
    }

    private getRefreshToken(): string | null {
        return localStorage.getItem(this.REFRESH_TOKEN_KEY);
    }

    private setRefreshToken(refreshToken: string): void {
        localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
    }

    private handleAuthentication(response: AuthResponse): void {
        this.setToken(response.token);
        this.setRefreshToken(response.refreshToken);

        const user: User = {
            id: '',
            email: response.email,
            firstName: '',
            lastName: '',
            roles: response.roles,
            isActive: true,
            lastLoginAt: null,
            createdAt: '',
            updatedAt: '',
        };

        this.userSignal.set(user);
        this.loadingSignal.set(false);
    }

    private decodeToken(token: string): User | null {
        try {
            const decoded = jwtDecode<DecodedToken>(token);
            return {
                id: decoded.sub,
                email: decoded.email,
                firstName: decoded.firstName,
                lastName: decoded.lastName,
                roles: decoded.roles || [],
                isActive: true,
                lastLoginAt: null,
                createdAt: '',
                updatedAt: '',
            };
        } catch {
            return null;
        }
    }

    private restoreSession(): void {
        const token = this.getToken();
        if (token) {
            const user = this.decodeToken(token);
            if (user) {
                this.userSignal.set(user);
            } else {
                this.logout();
            }
        }
    }

    hasRole(role: string): boolean {
        const user = this.userSignal();
        return user?.roles?.includes(role) ?? false;
    }

    hasAnyRole(roles: string[]): boolean {
        const user = this.userSignal();
        if (!user) return false;
        return roles.some((role) => user.roles.includes(role));
    }

    getCurrentUser(): User | null {
        return this.userSignal();
    }
}
