import { TrendDto } from './dashboard.model';

export interface AttendanceBreakdown {
    status: string;
    count: number;
}

export interface ProgressStatistics {
    totalSessions: number;
    completedSessions: number;
    attendanceRate: number;
    totalExercises: number;
    completedExercises: number;
    exerciseCompletionRate: number;
    totalAssessments: number;
    latestAssessmentScore: number | null;
    firstAssessmentScore: number | null;
    improvementPercentage: number | null;
}

export interface PatientProgress {
    assessmentScoreTrend: TrendDto[];
    moodTrend: TrendDto[];
    exerciseCompletionTrend: TrendDto[];
    attendanceBreakdown: AttendanceBreakdown[];
    statistics: ProgressStatistics;
}

export interface PatientProgressDateFilter {
    from?: string;
    to?: string;
}
