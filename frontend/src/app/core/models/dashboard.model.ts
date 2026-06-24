export interface DashboardStats {
    totalPatients: number;
    activePatients: number;
    totalSessions: number;
    sessionsThisMonth: number;
    completedExercises: number;
    pendingReviews: number;
    upComingAppointments: number;
    averageSessionRating: number;
}

export interface TrendData {
    date: string;
    value: number;
    metric: string;
}
