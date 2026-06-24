export const API = {
    auth: {
        login: '/api/auth/login',
        register: '/api/auth/register',
        refresh: '/api/auth/refresh',
        profile: '/api/auth/profile',
    },
    patients: {
        base: '/api/patients',
        byId: (id: string) => `/api/patients/${id}`,
        archive: (id: string) => `/api/patients/${id}/archive`,
        restore: (id: string) => `/api/patients/${id}/restore`,
        intake: (id: string) => `/api/patients/${id}/intake`,
        intakeImage: (id: string) => `/api/patients/${id}/intake/image`,
        assessments: (id: string) => `/api/patients/${id}/assessments`,
    },
    sessions: {
        base: '/api/sessions',
        byPatient: (patientId: string) => `/api/sessions/patient/${patientId}`,
        byId: (id: string) => `/api/sessions/${id}`,
        voice: (id: string) => `/api/sessions/${id}/voice`,
        summary: (id: string) => `/api/sessions/${id}/summary`,
    },
    exercises: {
        base: '/api/exercises',
        assign: '/api/exercises/assign',
        byPatient: (patientId: string) => `/api/exercises/patient/${patientId}`,
        status: (id: string) => `/api/exercises/${id}/status`,
    },
    reports: {
        base: '/api/reports',
        generate: '/api/reports/generate',
        byId: (id: string) => `/api/reports/${id}`,
        approve: (id: string) => `/api/reports/${id}/approve`,
        reject: (id: string) => `/api/reports/${id}/reject`,
        exportPdf: (id: string) => `/api/reports/${id}/export`,
    },
    dashboard: {
        stats: '/api/dashboard/stats',
        trends: '/api/dashboard/trends',
    },
    chat: {
        history: (sessionId: string) => `/api/chat/${sessionId}/history`,
        send: '/api/chat/send',
    },
} as const;
