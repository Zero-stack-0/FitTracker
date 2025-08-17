import { Component, OnInit } from '@angular/core';
import { UserDietPlanService } from '../services/user-diet-plan.service';

@Component({
  selector: 'app-diet-plan',
  templateUrl: './diet-plan.component.html',
  styleUrls: ['./diet-plan.component.css']
})
export class DietPlanComponent implements OnInit {
  //popup
  isOpen = false;
  isGreen = false;
  errorMessage = '';
  poptitle = '';

  userDietPlan: any
  isLoading = false
  loaderTitle = "Creating your personalized diet plan. Please wait..."
  constructor(private userDietPlanService: UserDietPlanService) { }

  ngOnInit(): void {
    this.fetchUserDietPlan()
  }

  fetchUserDietPlan() {
    this.isLoading = true;
    this.userDietPlanService.getUserDietPlan().subscribe((response) => {
      if (response?.statusCodes === 200) {
        this.userDietPlan = response.data;
        this.isLoading = false;
        return
      } else {
        this.isLoading = false;
        this.openPopup(response?.message, response?.message, false);
        return
      }

    })
  }

  getEmojiForFood(meal: string) {
    switch (meal) {
      case "Breakfast":
        return '🌅'
      case "Lunch":
        return '☀️'
      case "Dinner":
        return '🌙'
      default:
        return '🍎'
    }
  }

  openYouTube(exerciseName: string): void {
    const query = encodeURIComponent(exerciseName.trim());
    const url = `https://www.youtube.com/results?search_query=${query}`;
    window.open(url, '_blank');
  }

  openPopup(message: string, title: string, isGreen: boolean) {
    this.isOpen = true;
    this.isGreen = isGreen;
    this.errorMessage = message;
    this.poptitle = title;
  }
  closePopup() {
    this.isOpen = false;
  }
}
