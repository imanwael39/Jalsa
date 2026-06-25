import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class HttpClientService {
    private baseUrl = environment.apiUrl;

    constructor(private http: HttpClient) {}

    get<T>(endpoint: string, params?: HttpParams | { [param: string]: string | number | boolean | readonly (string | number | boolean)[] }): Observable<T> {
        return this.http.get<T>(`${this.baseUrl}${endpoint}`, { params })
            .pipe(catchError(this.handleError));
    }

    post<T>(endpoint: string, body: unknown | null, options?: { headers?: HttpHeaders }): Observable<T> {
        return this.http.post<T>(`${this.baseUrl}${endpoint}`, body, options)
            .pipe(catchError(this.handleError));
    }

    put<T>(endpoint: string, body: unknown): Observable<T> {
        return this.http.put<T>(`${this.baseUrl}${endpoint}`, body)
            .pipe(catchError(this.handleError));
    }

    patch<T>(endpoint: string, body: unknown): Observable<T> {
        return this.http.patch<T>(`${this.baseUrl}${endpoint}`, body)
            .pipe(catchError(this.handleError));
    }

    delete<T>(endpoint: string): Observable<T> {
        return this.http.delete<T>(`${this.baseUrl}${endpoint}`)
            .pipe(catchError(this.handleError));
    }

    upload<T>(endpoint: string, formData: FormData): Observable<T> {
        return this.http.post<T>(`${this.baseUrl}${endpoint}`, formData)
            .pipe(catchError(this.handleError));
    }

    private handleError(error: HttpErrorResponse): Observable<never> {
        console.error('HTTP Error:', error);
        return throwError(() => error);
    }
}
