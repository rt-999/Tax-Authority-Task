import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Measure, IngestionDataResponse } from '../models/data-ingestion.model';

@Injectable({
  providedIn: 'root'
})
export class DataIngestionService {
  // יש לעדכן את כתובת ה-API בהתאם לשרת ה-.NET שלך
  private apiUrl = 'https://localhost:7001/api';

  constructor(private http: HttpClient) {}

  // שליפת רשימת המדדים לבחירה בטופס
  getMeasures(): Observable<Measure[]> {
    return this.http.get<Measure[]>(`${this.apiUrl}/measures`);
  }

  // העלאת קובץ Excel לשרת
  uploadExcel(measureId: number, year: number, period: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('measureId', measureId.toString());
    formData.append('year', year.toString());
    formData.append('period', period);
    formData.append('file', file);

    return this.http.post(`${this.apiUrl}/ingestion/upload`, formData);
  }

  // שליפת נתונים דינמיים למדד
  getIngestedData(
    measureId: number, 
    page: number = 1, 
    pageSize: number = 10, 
    search: string = ''
  ): Observable<IngestionDataResponse> {
    let params = new HttpParams()
      .set('measureId', measureId.toString())
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (search) {
      params = params.set('search', search);
    }

    return this.http.get<IngestionDataResponse>(`${this.apiUrl}/ingestion/data`, { params });
  }
}