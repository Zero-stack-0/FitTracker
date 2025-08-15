import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private API_URL = 'http://localhost:5074/api/User';
  constructor(private http: HttpClient) { }

  signUp(user: any): Observable<any> {
    return this.http.post<any>(`${this.API_URL}/create`, user);
  }

  login(user: any): Observable<any> {
    return this.http.post<any>(`${this.API_URL}/login`, user);
  }

  getUserProfile(): Observable<any> {
    const token = localStorage.getItem('token');
    if (!token) {
      throw new Error('No token found');
    }
    return this.http.get<any>(`${this.API_URL}/profile`, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  userInformation(): Observable<any> {
    return this.http.get<any>(`${this.API_URL}/user-information`);
  }
  //user-information
}
