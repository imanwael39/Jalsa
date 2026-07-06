import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { of, throwError } from 'rxjs';
import { ProfileComponent } from './profile.component';
import { AuthService } from '../../../../core/services/auth.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { provideRouter } from '@angular/router';

interface SetupOptions {
    getProfileReturn?: ReturnType<typeof of> | ReturnType<typeof throwError>;
    updateProfileReturn?: ReturnType<typeof of> | ReturnType<typeof throwError>;
    changePasswordReturn?: ReturnType<typeof of> | ReturnType<typeof throwError>;
    uploadProfilePhotoReturn?: ReturnType<typeof of> | ReturnType<typeof throwError>;
}

function createFakeImageFile(type: string, sizeBytes: number): File {
    return new File([new Uint8Array(sizeBytes)], 'avatar.jpg', { type });
}

const mockUser = {
    id: '1',
    email: 't@t.com',
    firstName: 'Test',
    lastName: 'User',
    profileImageUrl: null,
    roles: ['Therapist'],
    isActive: true,
};

describe('ProfileComponent', () => {
    let component: ProfileComponent;
    let fixture: ComponentFixture<ProfileComponent>;
    let authServiceSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { getProfileReturn, updateProfileReturn, changePasswordReturn, uploadProfilePhotoReturn } = opts;

        authServiceSpy = {
            getProfile: vi.fn().mockReturnValue(getProfileReturn ?? of(mockUser)),
            updateProfile: vi.fn().mockReturnValue(updateProfileReturn ?? of(mockUser)),
            changePassword: vi.fn().mockReturnValue(changePasswordReturn ?? of({})),
            uploadProfilePhoto: vi.fn().mockReturnValue(uploadProfilePhotoReturn ?? of(mockUser)),
            resolveAvatarUrl: vi.fn((url: string | null) => (url ? `http://localhost:5014${url}` : null)),
            currentUser: signal(mockUser),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };

        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule, ProfileComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: AuthService, useValue: authServiceSpy },
                { provide: NotificationService, useValue: notificationSpy },
            ],
        });

        fixture = TestBed.createComponent(ProfileComponent);
        component = fixture.componentInstance;
    };

    it('should create', () => {
        setup();
        expect(component).toBeTruthy();
    });
    it('should load profile on init', () => {
        setup();
        fixture.detectChanges();
        expect(authServiceSpy['getProfile']).toHaveBeenCalled();
        expect(component.profileForm.get('firstName')!.value).toBe('Test');
    });
    it('should handle profile load error', () => {
        setup({ getProfileReturn: throwError(() => new Error('fail')) });
        fixture.detectChanges();
        expect(component.error()).toBe('فشل تحميل الملف الشخصي. يرجى المحاولة مرة أخرى.');
    });
    it('should set loading false after profile loaded', () => {
        setup();
        fixture.detectChanges();
        expect(component.loading()).toBe(false);
    });
    it('should return required firstName error', () => {
        setup();
        component.profileForm.get('firstName')!.markAsTouched();
        expect(component.getFirstNameError()).toBe('الاسم الأول مطلوب');
    });
    it('should return minlength lastName error', () => {
        setup();
        component.profileForm.patchValue({ lastName: 'A' });
        component.profileForm.get('lastName')!.markAsTouched();
        expect(component.getLastNameError()).toBe('اسم العائلة يجب أن يكون حرفين على الأقل');
    });
    it('should return required email error', () => {
        setup();
        component.profileForm.get('email')!.markAsTouched();
        expect(component.getEmailError()).toBe('البريد الإلكتروني مطلوب');
    });
    it('should call updateProfile on valid profile submit', () => {
        setup();
        component.profileForm.patchValue({ firstName: 'Te', lastName: 'Us', email: 't@t.com' });
        component.updateProfile();
        expect(authServiceSpy['updateProfile']).toHaveBeenCalledWith({
            firstName: 'Te',
            lastName: 'Us',
            email: 't@t.com',
        });
    });
    it('should not updateProfile when form invalid', () => {
        setup();
        component.updateProfile();
        expect(authServiceSpy['updateProfile']).not.toHaveBeenCalled();
    });
    it('should set success on profile update success', () => {
        setup();
        component.profileForm.patchValue({ firstName: 'Te', lastName: 'Us', email: 't@t.com' });
        component.updateProfile();
        expect(component.success()).toBe(true);
    });
    it('should show error on profile update failure', () => {
        setup({ updateProfileReturn: throwError(() => ({ error: { message: 'فشل التحديث' } })) });
        component.profileForm.patchValue({ firstName: 'Te', lastName: 'Us', email: 't@t.com' });
        component.updateProfile();
        expect(component.error()).toBe('فشل التحديث');
    });
    it('should return required currentPassword error', () => {
        setup();
        component.passwordForm.get('currentPassword')!.markAsTouched();
        expect(component.getCurrentPasswordError()).toBe('كلمة المرور الحالية مطلوبة');
    });
    it('should return required newPassword error', () => {
        setup();
        component.passwordForm.get('newPassword')!.markAsTouched();
        expect(component.getNewPasswordError()).toBe('كلمة المرور الجديدة مطلوبة');
    });
    it('should return required confirmPassword error', () => {
        setup();
        component.passwordForm.get('confirmPassword')!.markAsTouched();
        expect(component.getConfirmPasswordError()).toBe('يرجى تأكيد كلمة المرور');
    });
    it('should call changePassword on valid password submit', () => {
        setup();
        component.passwordForm.patchValue({
            currentPassword: 'old',
            newPassword: 'NewPassw0rd',
            confirmPassword: 'NewPassw0rd',
        });
        component.changePassword();
        expect(authServiceSpy['changePassword']).toHaveBeenCalledWith({
            currentPassword: 'old',
            newPassword: 'NewPassw0rd',
        });
    });
    it('should reset password form on changePassword success', () => {
        setup();
        component.passwordForm.patchValue({
            currentPassword: 'old',
            newPassword: 'NewPassw0rd',
            confirmPassword: 'NewPassw0rd',
        });
        component.changePassword();
        expect(component.passwordForm.get('currentPassword')!.value).toBe('');
    });
    it('should show error on changePassword failure', () => {
        setup({ changePasswordReturn: throwError(() => ({ error: { message: 'كلمة مرور حالية غير صحيحة' } })) });
        component.passwordForm.patchValue({
            currentPassword: 'wrong',
            newPassword: 'NewPassw0rd',
            confirmPassword: 'NewPassw0rd',
        });
        component.changePassword();
        expect(component.passwordError()).toBe('كلمة مرور حالية غير صحيحة');
    });
    it('should not changePassword when form invalid', () => {
        setup();
        component.changePassword();
        expect(authServiceSpy['changePassword']).not.toHaveBeenCalled();
    });

    it('should show avatar initials when no profile image is set', () => {
        setup();
        expect(component.avatarUrl()).toBeNull();
        expect(component.initials()).toBe('TU');
    });
    it('should resolve avatar url when a profile image is set', () => {
        authServiceSpy = {
            getProfile: vi.fn().mockReturnValue(of(mockUser)),
            updateProfile: vi.fn().mockReturnValue(of(mockUser)),
            changePassword: vi.fn().mockReturnValue(of({})),
            uploadProfilePhoto: vi.fn().mockReturnValue(of(mockUser)),
            resolveAvatarUrl: vi.fn((url: string | null) => (url ? `http://localhost:5014${url}` : null)),
            currentUser: signal({ ...mockUser, profileImageUrl: '/uploads/avatars/1.jpg' }),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };
        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule, ProfileComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: AuthService, useValue: authServiceSpy },
                { provide: NotificationService, useValue: notificationSpy },
            ],
        });
        fixture = TestBed.createComponent(ProfileComponent);
        component = fixture.componentInstance;

        expect(component.avatarUrl()).toBe('http://localhost:5014/uploads/avatars/1.jpg');
    });
    it('should reject an unsupported image type without calling uploadProfilePhoto', () => {
        setup();
        const file = createFakeImageFile('image/gif', 1000);
        const event = { target: { files: [file], value: '' } } as unknown as Event;
        component.onPhotoSelected(event);
        expect(component.photoError()).toBe('صيغة الصورة غير مدعومة. الصيغ المسموحة: JPG, PNG, WEBP.');
        expect(authServiceSpy['uploadProfilePhoto']).not.toHaveBeenCalled();
    });
    it('should reject an oversized image without calling uploadProfilePhoto', () => {
        setup();
        const file = createFakeImageFile('image/jpeg', 6 * 1024 * 1024);
        const event = { target: { files: [file], value: '' } } as unknown as Event;
        component.onPhotoSelected(event);
        expect(component.photoError()).toBe('حجم الصورة يجب ألا يتجاوز 5 ميجابايت.');
        expect(authServiceSpy['uploadProfilePhoto']).not.toHaveBeenCalled();
    });
    it('should upload a valid image and clear uploading state', () => {
        setup();
        const file = createFakeImageFile('image/png', 1000);
        const event = { target: { files: [file], value: '' } } as unknown as Event;
        component.onPhotoSelected(event);
        expect(authServiceSpy['uploadProfilePhoto']).toHaveBeenCalledWith(file);
        expect(component.uploadingPhoto()).toBe(false);
        expect(component.photoError()).toBeNull();
    });
    it('should show an error when photo upload fails', () => {
        setup({ uploadProfilePhotoReturn: throwError(() => ({ error: { message: 'فشل الرفع' } })) });
        const file = createFakeImageFile('image/png', 1000);
        const event = { target: { files: [file], value: '' } } as unknown as Event;
        component.onPhotoSelected(event);
        expect(component.photoError()).toBe('فشل الرفع');
        expect(component.uploadingPhoto()).toBe(false);
    });
});
