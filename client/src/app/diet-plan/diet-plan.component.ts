import { Component, OnInit } from '@angular/core';
import { UserDietPlanService } from '../services/user-diet-plan.service';

@Component({
  selector: 'app-diet-plan',
  templateUrl: './diet-plan.component.html',
  styleUrls: ['./diet-plan.component.css']
})
export class DietPlanComponent implements OnInit {
  userDietPlan: any
  isLoading = false
  constructor(private userDietPlanService: UserDietPlanService) { }

  ngOnInit(): void {
    this.fetchUserDietPlan()
  }

  fetchUserDietPlan() {
    this.isLoading = true;
    this.userDietPlanService.getUserDietPlan().subscribe((response) => {
      this.userDietPlan = response.data;
      this.isLoading = false;
      return
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
}
