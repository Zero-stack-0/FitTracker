import { Component, OnInit } from '@angular/core';
import { UserFoodLodService } from '../services/user-food-log.service';


@Component({
  selector: 'app-food-log-history',
  templateUrl: './food-log-history.component.html',
  styleUrls: ['./food-log-history.component.css']
})
export class FoodLogHistoryComponent implements OnInit {
  constructor(private userFoodLog: UserFoodLodService) { }
  //popup
  isOpen = false;
  isGreen = false;
  errorMessage = '';
  poptitle = '';
  foodLog: any
  weekOffset = 0
  ngOnInit(): void {
    this.fetchFoodLogHistory();

  }

  fetchFoodLogHistory() {
    this.userFoodLog.foodLogHistory(this.weekOffset).subscribe((res) => {
      if (res.statusCodes === 200) {
        this.foodLog = res.data
      } else {
        this.openPopup(res?.message, res?.message, false);
      }

    })
  }

  toggleDay(index: number): void {
    this.foodLog.userFoodLogDayHistory[index].expanded = !this.foodLog.userFoodLogDayHistory[index].expanded;
  }

  previousWeek() {
    this.weekOffset -= 1;
    this.foodLog = []
    this.fetchFoodLogHistory()
  }
  nextWeek() {
    this.weekOffset += 1;
    this.foodLog = []
    this.fetchFoodLogHistory()
  }
  getMealTotalCalories(meal: any): number {
    return meal?.userFoodLogResponseList?.reduce((sum: number, item: any) => sum + item.caloriesLogged, 0) || 0;
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
