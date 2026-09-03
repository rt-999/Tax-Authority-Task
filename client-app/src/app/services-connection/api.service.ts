import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  // כתובת ה-Backend (C# Web API)
  private baseUrl = 'https://localhost:7xxx/api'; 

  constructor(private http: HttpClient) { }

  // דוגמה לקריאת GET
  getData(): Observable<any> {
    return this.http.get(`${this.baseUrl}/data`);
  }

  // דוגמה לקריאת POST
  sendData(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/data`, data);
  }
}