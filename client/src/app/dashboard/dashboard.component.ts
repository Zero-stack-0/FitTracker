import { Component } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  constructor(private userService: UserService, private authService: AuthService, private route: Router) { }
  userData: any;

  //popup properties
  isOpen = false;
  isGreen = false;
  errorMessage = '';
  poptitle = '';
  ngOnInit() {
    if (!this.authService.isTokenAvailable() || !this.authService.isTokenExpired()) {
      this.openPopup('Please log in again.', 'Session Expired');
      this.route.navigate(['/login']);
      return;
    }

    this.userService.getUserProfile().subscribe({
      next: (response) => {
        this.userData = response.data;
      },
      error: (error) => {
        console.error('Error fetching user data:', error);
        this.route.navigate(['/login']);
      }
    });
  }

  openPopup(message: string, title: string) {
    this.isOpen = true;
    this.isGreen = true;
    this.errorMessage = message;
    this.poptitle = title;
  }
  closePopup() {
    this.isOpen = false;
  }

}
