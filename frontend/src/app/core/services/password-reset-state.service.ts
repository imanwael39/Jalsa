import { Injectable, signal } from '@angular/core';

/**
 * Carries the email (and, once verified, the OTP) between the forgot-password,
 * verify-otp, and reset-password steps so the user never re-types the email.
 * In-memory only by design: a hard refresh mid-flow forces a restart from
 * forgot-password, which is an acceptable trade-off for an OTP flow.
 */
@Injectable({ providedIn: 'root' })
export class PasswordResetStateService {
    private emailSignal = signal<string | null>(null);
    private otpSignal = signal<string | null>(null);

    getEmail(): string | null {
        return this.emailSignal();
    }

    getOtp(): string | null {
        return this.otpSignal();
    }

    setEmail(email: string): void {
        this.emailSignal.set(email);
    }

    setOtp(otp: string): void {
        this.otpSignal.set(otp);
    }

    clear(): void {
        this.emailSignal.set(null);
        this.otpSignal.set(null);
    }
}
