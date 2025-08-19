import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../Constants/api-urls';

@Injectable({
  providedIn: 'root'
})
export class MotivationService {
  constructor(private http: HttpClient) { }

  getMotivation(): Observable<any> {
    return this.http.get<any>(`${API_BASE_URL}/api/Motivation`)
  }
}
