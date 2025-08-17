import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserDietPlanService {
  private API_URL = "https://fittracker-kx3r.onrender.com/api/UserDietPlan"
  constructor(private http: HttpClient) { }

  getUserDietPlan(): Observable<any> {
    return this.http.get<any>(`${this.API_URL}/basic-diet-plan`);
  }

  canUserUpdateDietPlan(): Observable<any> {
    return this.http.get<any>(`${this.API_URL}/can-update-diet-plan`);
  }

  updateProfile(userdata: any): Observable<any> {
    return this.http.put<any>(`${this.API_URL}/user-information-diet`, userdata);
  }
}
