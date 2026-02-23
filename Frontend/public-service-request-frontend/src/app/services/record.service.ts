import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Record, RecordDTO, recordFromDTO, recordToDTO } from '../models/records';

@Injectable({ providedIn: 'root' })
export class RecordService {
  private readonly apiUrl = `${environment.apiUrl}/records`;
  private readonly platformId = inject(PLATFORM_ID);

  constructor(private http: HttpClient) {}

  getAllRecords(): Observable<Record[]> {
    return this.http.get<Record[]>(this.apiUrl).pipe(
      map((dtos: any[]) => dtos.map((dto) => recordFromDTO(dto))),
      catchError(this.handleError.bind(this)),
    );
  }

  createRecord(record: Record): Observable<Record> {
    const dto = recordToDTO(record);
    return this.http.post<RecordDTO>(this.apiUrl, dto).pipe(
      map(recordFromDTO),
      catchError((err) => this.handleError(err)),
    );
  }

  getRecordById(id: number): Observable<Record> {
    return this.http.get<Record>(`${this.apiUrl}/${id}`).pipe(
      map((dto: any) => recordFromDTO(dto)),
      catchError(this.handleError.bind(this)),
    );
  }

  updateRecord(id: number, record: Record): Observable<Record> {
    const dto = recordToDTO(record);
    return this.http
      .patch<Record>(`${this.apiUrl}/${id}`, dto)
      .pipe(catchError((err) => this.handleError(err)));
  }

  deleteRecord(id: number): Observable<void> {
    return this.http
      .delete<void>(`${this.apiUrl}/${id}`)
      .pipe(catchError((err) => this.handleError(err)));
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred';

    if (isPlatformBrowser(this.platformId) && error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status ?? 'unknown'}\nMessage: ${error.message}`;

      if (error.status === 404) {
        errorMessage = 'Task not found';
      } else if (error.status === 400) {
        errorMessage = error.error?.message || 'Bad request';
      } else if (error.status === 401) {
        errorMessage = 'Unauthorized: Please log in';
      }
    }
    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
