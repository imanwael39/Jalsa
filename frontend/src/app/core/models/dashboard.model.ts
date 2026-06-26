export interface TrendDto {
    date: string;
    value: number;
    label: string;
}

export interface ExerciseCompletionBreakdown {
    complete: number;
    partial: number;
    skipped: number;
}

export interface Analytics {
    assessmentTrend: TrendDto[];
    exerciseCompletion: ExerciseCompletionBreakdown;
    sessionFrequency: TrendDto[];
}

export interface DashboardSummary {
    totalPatients: number;
    activePatients: number;
    archivedPatients: number;
    totalSessions: number;
    sessionsThisMonth: number;
    exerciseCompletionRate: number;
    averageAssessmentScore: number;
    recentAlerts: string[];
    analytics: Analytics;
}
