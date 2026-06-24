export interface ApiResponse<T> {
    data: T;
    message: string;
    success: boolean;
    errors: string[] | null;
}

export interface PaginatedApiResponse<T> {
    data: T[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    message: string;
    success: boolean;
    errors: string[] | null;
}
