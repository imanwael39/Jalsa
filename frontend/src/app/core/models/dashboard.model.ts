export interface DashboardStats {
    totalPatients: number;
    activePatients: number;
    totalSessions: number;
    completedSessions: number;
    pendingExercises: number;
    completedExercises: number;
    pendingReports: number;
}

export interface TrendData {
    date: string;
    value: number;
}
