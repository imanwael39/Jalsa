export interface PatientAssessmentSummary {
    id: string;
    templateName: string;
    title: string | null;
    status: string;
    assessmentDate: string | null;
    completedAt: string | null;
    totalScore: number | null;
    severity: string | null;
    questionCount: number;
    answeredCount: number;
}

export interface AssessmentQuestion {
    id: string;
    questionText: string;
    questionType: string | null;
    sortOrder: number;
    answerNumber: number | null;
}

export interface PatientAssessmentDetail {
    id: string;
    templateName: string;
    title: string | null;
    status: string;
    assessmentDate: string | null;
    completedAt: string | null;
    totalScore: number | null;
    severity: string | null;
    questions: AssessmentQuestion[];
}

export interface SaveAnswerRequest {
    answerNumber: number;
}
