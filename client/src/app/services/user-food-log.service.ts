import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserFoodLodService {
  private API_URL = 'https://fittracker-kx3r.onrender.com/api';
  constructor(private http: HttpClient) { }

  addFoodToLog(food: any): Observable<any> {
    return this.http.post<any>(`${this.API_URL}/UserFoodLog/add`, food);
  }

  recentEntries(): Observable<any> {
    return this.http.get<any>(`${this.API_URL}/UserFoodLog/recent-entries`);
  }

  foodLogHistory(offset: number): Observable<any> {
    return this.http.get<any>(`${this.API_URL}/UserFoodLog/food-log-history?weekOffset=${offset}`);
  }

  dashboardForUser(): Observable<any> {
    return this.http.get<any>(`${this.API_URL}/UserFoodLog/dashboard`);
  }
}
