import { Component } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserFoodLodService } from '../services/user-food-log.service';
import { MotivationService } from '../services/motivation.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  constructor(private motivationService: MotivationService, private userService: UserService, private authService: AuthService, private route: Router, private userFoodLog: UserFoodLodService) { }
  loadingTitle = "Fetching you data"
  motivation = ''
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
    this.isLoading = true;
    if (!this.authService.isTokenAvailable() || !this.authService.isTokenExpired()) {
      this.openPopup('Please log in again.', 'Session Expired');
      this.route.navigate(['/login']);
      this.isLoading = false
      return;
    }
    this.fetchMotivation()
    this.userService.getUserProfile().subscribe({
      next: (response) => {
        if (response.statusCodes === 200) {
          this.userData = response.data;
          return
        }
        else {
          this.isLoading = false;
        }
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Error fetching user data:', error);
        this.route.navigate(['/login']);
      }
    });

    this.userFoodLog.dashboardForUser().subscribe((res) => {
      if (res.statusCodes === 200) {
        this.userMacrosKPI = res.data;
        this.isLoading = false
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

  fetchMotivation() {
    this.motivationService.getMotivation().subscribe((res) => {
      this.motivation = res?.data.quote
      console.log(res.data)
    })
  }
}
