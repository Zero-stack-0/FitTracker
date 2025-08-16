import { Component } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserFoodLodService } from '../services/user-food-log.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  constructor(private userService: UserService, private authService: AuthService, private route: Router, private userFoodLog: UserFoodLodService) { }
  userData: any;
  userMacrosKPI: any
  isLoading = false
  mealType = 1
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
        this.isLoading = true;
        if (response.statusCodes === 200) {
          this.userData = response.data;
          this.isLoading = false;
          return
        }
        else {
          this.isLoading = false;
        }
      },
      error: (error) => {
        console.error('Error fetching user data:', error);
        this.route.navigate(['/login']);
      }
    });

    this.userFoodLog.dashboardForUser().subscribe((res) => {
      this.isLoading = true
      if (res.statusCodes === 200) {
        this.userMacrosKPI = res.data;
        this.isLoading = false;
        return;
      }
      this.isLoading = false;
    })
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

  getMacrosProgressPercentage(macros_total: any, macros_taken: any): number {
    const total = macros_taken || 0;
    const goal = macros_total || 1;

    const percentage = (total / goal) * 100;
    return Math.min(percentage, 100);
  }
}
