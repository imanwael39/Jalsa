import { TrendDto } from './dashboard.model';

export interface UpcomingSession {
    id: string;
    sessionDate: string;
    sessionType: string | null;
    durationMinutes: number | null;
    status: string;
}

export interface TodayReminder {
    type: 'Session' | 'Exercise';
    title: string;
    referenceId: string;
}

export interface AssignedExerciseSummary {
    id: string;
    description: string | null;
    frequency: string | null;
    dueDate: string | null;
    status: string;
    isOverdue: boolean;
}

export interface PendingAssessment {
    id: string;
    templateName: string;
    assignedDate: string | null;
}

export interface RecentConversation {
    conversationId: string;
    lastActivityAt: string | null;
    status: string;
}

export interface ProgressOverview {
    exerciseCompletionRate: number;
    latestAssessmentScore: number | null;
    previousAssessmentScore: number | null;
    assessmentTrend: TrendDto[];
    completedSessionsCount: number;
}

export interface TherapistInfo {
    id: string;
    fullName: string;
    specialization: string | null;
    phone: string | null;
    profileImageUrl: string | null;
}

export interface CrisisSupportInfo {
    hotlineNumber: string;
    hotlineLabel: string;
}

export interface PatientDashboard {
    patientFirstName: string;
    upcomingSessions: UpcomingSession[];
    todayReminders: TodayReminder[];
    assignedExercises: AssignedExerciseSummary[];
    pendingAssessments: PendingAssessment[];
    recentConversations: RecentConversation[];
    progressOverview: ProgressOverview;
    therapist: TherapistInfo | null;
    crisisSupport: CrisisSupportInfo;
}
