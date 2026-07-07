export interface AdminUser {
    id: string;
    email: string;
    fullName: string;
    roles: string[];
    isActive: boolean;
    isDeleted: boolean;
    isLockedOut: boolean;
    lastLoginAt: string | null;
    createdAt: string;
}

export interface UserFilter {
    searchTerm?: string;
    role?: string;
    isActive?: boolean;
    includeDeleted?: boolean;
    page: number;
    pageSize: number;
}

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
}

export type TherapistApprovalStatus = 'Pending' | 'Approved' | 'Rejected' | 'Suspended';

export interface TherapistAdmin {
    id: string;
    userId: string;
    fullName: string;
    email: string;
    licenseNumber: string;
    specialization: string | null;
    approvalStatus: TherapistApprovalStatus;
    approvalStatusUpdatedAt: string | null;
    patientCount: number;
    isActive: boolean;
    createdAt: string;
    lastLoginAt: string | null;
}

export interface TherapistAdminDetail extends TherapistAdmin {
    phone: string | null;
    bio: string | null;
    profileImageUrl: string | null;
    recentAuditLogs: AuditLogEntry[];
}

export interface TherapistFilter {
    searchTerm?: string;
    approvalStatus?: string;
    page: number;
    pageSize: number;
}

export interface PatientAccountAdmin {
    id: string;
    userId: string | null;
    fullName: string;
    email: string | null;
    therapistName: string;
    status: string;
    isActive: boolean | null;
    isDeleted: boolean | null;
    createdAt: string;
    lastLoginAt: string | null;
}

export interface PatientAccountFilter {
    searchTerm?: string;
    status?: string;
    page: number;
    pageSize: number;
}

export interface AuditLogEntry {
    id: string;
    userId: string | null;
    userEmail: string | null;
    entityName: string;
    entityId: string | null;
    action: string;
    oldValues: string | null;
    newValues: string | null;
    ipAddress: string | null;
    userAgent: string | null;
    occurredAt: string;
}

export interface AuditLogFilter {
    userId?: string;
    entityName?: string;
    entityId?: string;
    action?: string;
    dateFrom?: string;
    dateTo?: string;
    page: number;
    pageSize: number;
}

export interface SystemSettings {
    siteName: string;
    defaultLanguage: string;
    passwordMinLength: number;
    sessionTimeoutMinutes: number;
    maintenanceMode: boolean;
}

export interface AdminDashboardSummary {
    totalUsers: number;
    totalTherapists: number;
    totalPatients: number;
    totalAdmins: number;
    activeUsers: number;
    disabledUsers: number;
    newRegistrations7Days: number;
    newRegistrations30Days: number;
    pendingDoctorApprovals: number;
    recentAuditLogs: AuditLogEntry[];
    latestRegisteredUsers: AdminUser[];
}

export interface SystemHealth {
    processMemoryMb: number;
    gen0Collections: number;
    gen1Collections: number;
    gen2Collections: number;
    databaseSizeMb: number | null;
    hangfireEnqueued: number | null;
    hangfireProcessing: number | null;
    hangfireSucceeded: number | null;
    hangfireFailed: number | null;
    aiChatCallsToday: number;
    aiChatCallsThisMonth: number;
    aiReportCallsToday: number;
    aiReportCallsThisMonth: number;
    aiTokensUsedThisMonth: number;
}
