import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../Constants/api-urls';

@Injectable({
  providedIn: 'root'
})
export class UserDietPlanService {
  private API_URL = API_BASE_URL + "/api/UserDietPlan"
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
