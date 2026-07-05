import { Injectable, signal, computed } from '@angular/core';
import { Observable, tap, catchError, throwError, switchMap, map, of, firstValueFrom } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { NotificationService } from './notification.service';
import { environment } from '../../../environments/environment';
import type { User } from '../models/user.model';
import type {
    AuthResponse,
    LoginRequest,
    RegisterRequest,
    TherapistOption,
    UpdateProfileRequest,
    ChangePasswordRequest,
    ResetPasswordRequest,
} from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private readonly TOKEN_KEY = 'jalsa_token';
    private readonly REFRESH_TOKEN_KEY = 'jalsa_refresh_token';

    private userSignal = signal<User | null>(null);
    private loadingSignal = signal<boolean>(false);
    private initializedSignal = signal<boolean>(false);

    readonly currentUser = this.userSignal.asReadonly();
    readonly isLoading = this.loadingSignal.asReadonly();
    readonly initialized = this.initializedSignal.asReadonly();

    readonly isAuthenticated = computed(() => this.userSignal() !== null && this.getToken() !== null);

    constructor(
        private http: HttpClientService,
        private notification: NotificationService
    ) {}

    /**
     * Restores the full user profile (including roles) from the backend on app bootstrap.
     * The JWT only carries sub/email/jti/role claims, not a stable "roles" JSON key, so
     * role information must come from GET /api/auth/profile rather than client-side decoding.
     */
    initializeAuth(): Promise<void> {
        const token = this.getToken();
        if (!token) {
            this.initializedSignal.set(true);
            return Promise.resolve();
        }

        return firstValueFrom(
            this.getProfile().pipe(
                tap(user => this.userSignal.set(user)),
                catchError(() => {
                    this.logout();
                    return of(null);
                })
            )
        ).then(() => {
            this.initializedSignal.set(true);
        });
    }

    login(credentials: LoginRequest): Observable<AuthResponse> {
        this.loadingSignal.set(true);
        return this.http.post<AuthResponse>(API.auth.login, credentials).pipe(
            switchMap(response => {
                this.handleAuthentication(response);
                return this.getProfile().pipe(
                    tap(user => this.userSignal.set(user)),
                    catchError(() => of(null)),
                    map(() => response)
                );
            }),
            tap(() => {
                this.loadingSignal.set(false);
                this.notification.success('تم تسجيل الدخول بنجاح');
            }),
            catchError(error => {
                this.loadingSignal.set(false);
                return throwError(() => error);
            })
        );
    }

    getTherapistOptions(): Observable<TherapistOption[]> {
        return this.http.get<TherapistOption[]>(API.auth.therapists);
    }

    register(userData: RegisterRequest): Observable<unknown> {
        this.loadingSignal.set(true);
        return this.http.post(API.auth.register, userData).pipe(
            tap(() => {
                this.loadingSignal.set(false);
                this.notification.success('تم إنشاء الحساب بنجاح');
            }),
            catchError(error => {
                this.loadingSignal.set(false);
                return throwError(() => error);
            })
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
            tap(response => {
                this.handleAuthentication(response);
            })
        );
    }

    getProfile(): Observable<User> {
        return this.http.get<User>(API.auth.profile);
    }

    updateProfile(data: UpdateProfileRequest): Observable<User> {
        return this.http.put<User>(API.auth.profile, data).pipe(
            tap(user => {
                this.userSignal.set(user);
                this.notification.success('تم تحديث الملف الشخصي بنجاح');
            })
        );
    }

    uploadProfilePhoto(file: File): Observable<User> {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.upload<User>(API.auth.profilePhoto, formData).pipe(
            tap(user => {
                this.userSignal.set(user);
                this.notification.success('تم تحديث الصورة الشخصية بنجاح');
            })
        );
    }

    /** Resolves a possibly-relative avatar path (e.g. "/uploads/avatars/x.jpg") to an absolute URL. */
    resolveAvatarUrl(profileImageUrl: string | null | undefined): string | null {
        if (!profileImageUrl) return null;
        if (/^https?:\/\//i.test(profileImageUrl)) return profileImageUrl;
        return `${environment.apiUrl}${profileImageUrl}`;
    }

    changePassword(data: ChangePasswordRequest): Observable<unknown> {
        return this.http.post(API.auth.profile + '/change-password', data).pipe(
            tap(() => {
                this.notification.success('تم تغيير كلمة المرور بنجاح');
            })
        );
    }

    forgotPassword(email: string): Observable<unknown> {
        return this.http.post(API.auth.forgotPassword, { email }).pipe(
            tap(() => {
                this.notification.success('تم إرسال رمز التحقق إلى بريدك الإلكتروني');
            })
        );
    }

    resetPassword(data: ResetPasswordRequest): Observable<unknown> {
        return this.http.post(API.auth.resetPassword, data).pipe(
            tap(() => {
                this.notification.success('تم إعادة تعيين كلمة المرور بنجاح');
            })
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
            profileImageUrl: null,
            roles: response.roles,
            isActive: true,
            lastLoginAt: null,
            createdAt: '',
            updatedAt: '',
        };

        this.userSignal.set(user);
        this.loadingSignal.set(false);
    }

    hasRole(role: string): boolean {
        const user = this.userSignal();
        return user?.roles?.includes(role) ?? false;
    }

    hasAnyRole(roles: string[]): boolean {
        const user = this.userSignal();
        if (!user) return false;
        return roles.some(role => user.roles.includes(role));
    }

    getCurrentUser(): User | null {
        return this.userSignal();
    }
}
