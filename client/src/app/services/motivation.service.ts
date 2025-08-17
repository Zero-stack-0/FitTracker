import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MotivationService {
  private API_URL = "https://fittracker-kx3r.onrender.com/api/Motivation"
  constructor(private http: HttpClient) { }

  getMotivation(): Observable<any> {
    return this.http.get<any>(`${this.API_URL}`)
  }
}
