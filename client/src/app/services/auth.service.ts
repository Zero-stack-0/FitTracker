// auth.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.isTokenAvailable());
  constructor(private userService: UserService) { }
  public isLoggedIn$: Observable<boolean> = this.isLoggedInSubject.asObservable();

  isTokenAvailable(): boolean {
    return localStorage.getItem('token') === null ? false : true;
  }

  logout() {
    localStorage.removeItem('userData');
    localStorage.removeItem('token');
    this.isLoggedInSubject.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  setToken(token: string) {
    localStorage.setItem('token', token);
    this.isLoggedInSubject.next(true);
  }

  isTokenExpired(): boolean {
    var token = localStorage.getItem('token');
    if (!token) return true;
    const payload = JSON.parse(atob(token.split('.')[1]));
    const exp = payload.exp;
    if (!exp) return true;
    const now = Math.floor(Date.now() / 1000);
    return exp > now;
  }

  setUserData(userData: any) {
    localStorage.removeItem('userData');
    localStorage.setItem('userData', userData);
  }


  getUserData() {
    const userDataString = localStorage.getItem('userData');
    if (userDataString) {
      return JSON.parse(userDataString);
    } else {
      return null
    }
  }
}
