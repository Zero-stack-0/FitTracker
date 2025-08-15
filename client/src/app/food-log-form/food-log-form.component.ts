import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FoodMacrosService } from '../services/food-macros.service';
import { debounceTime, distinctUntilChanged, filter, switchMap, tap } from 'rxjs';
import { FoodMacrosInterface } from '../models/food-macros-interface';
import { UserFoodLodService } from '../services/user-food-log.service';
import { UserService } from '../services/user.service';
import { AuthService } from '../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-food-log-form',
  templateUrl: './food-log-form.component.html',
  styleUrls: ['./food-log-form.component.css']
})
export class FoodLogFormComponent implements OnInit {
  @ViewChild('container', { static: false }) container!: ElementRef;
  constructor(private foodMacrosService: FoodMacrosService, private userFoodLogForm: UserFoodLodService,
    private authService: AuthService, private route: Router, private routeA: ActivatedRoute) { }


  //popup
  isOpen = false;
  isGreen = false;
  errorMessage = '';
  poptitle = '';
  isFoodSelected = true;
  isQuantityInvalid = false;
  selectedFood: FoodMacrosInterface | null = null;
  mealType = 1
  searchValue = 'egg';
  values = ''
  quantity = 0
  foodMacros: FoodMacrosInterface[] = [];
  recentEntries: any
  searchControl = new FormControl();


  ngOnInit(): void {
    //this.fetchFoodMacros();
    if (!this.authService.isTokenAvailable() || !this.authService.isTokenExpired()) {
      this.openPopup('Please log in again.', 'Session Expired', false);
      this.route.navigate(['/login']);
      return;
    }
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      tap((value: string) => {
        if ((!value || value.length <= 2)) {
          this.foodMacros = [];
        }
      }),
      filter((value: string) => value.length > 2),
      switchMap((value: string) => this.foodMacrosService.getFoodByName(value))
    ).subscribe((foods: FoodMacrosInterface[]) => {
      this.foodMacros = foods;
    });

    this.fetchRecentFoodLogs()
    this.routeA.queryParams.subscribe(params => {
      this.mealType = params['mealType'];
    });
  }

  fetchRecentFoodLogs() {
    this.userFoodLogForm.recentEntries().subscribe((response) => {
      this.recentEntries = response.data;
      console.log(this.recentEntries);
    });
  }

  fetchFoodMacros(): void {
    if (!this.values || this.values.length < 1) {
      this.foodMacros = []
      return;
    }
    this.foodMacrosService.getFoodByName(this.values).subscribe((food) => {
      this.foodMacros = food;
    });
  }

  onInputChange(value: string) {
    if (value.length > 1) {
      this.fetchFoodMacros();

    } else {
      this.foodMacros = []
    }
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.container.nativeElement.contains(event.target)) {
      this.foodMacros = [];
    }
  }

  selectTheFood(food: FoodMacrosInterface) {
    this.selectedFood = food;

    this.searchControl.setValue(food.name, { emitEvent: false });
    this.foodMacros = [];
    this.isFoodSelected = true;
    console.log(this.selectedFood);
  }

  mealTypeChanges(type: number) {
    this.mealType = type;
    console.log(this.mealType);
  }

  onQuantityInputChange(quan: number) {
    if (quan > 0) {
      this.isQuantityInvalid = false;
    }
    this.quantity = quan;
    console.log(this.quantity);
  }

  onAddFoodToLog() {
    const foodToLog = {
      FoodId: this.selectedFood?.id,
      TimeOfTheDay: this.mealType,
      Quantity: this.quantity
    }
    if (!foodToLog.FoodId || !foodToLog.Quantity) {
      this.isFoodSelected = false;
      if (!foodToLog.Quantity) {
        this.isQuantityInvalid = true;
      }
      return;
    }

    this.userFoodLogForm.addFoodToLog(foodToLog).subscribe((response) => {

      if (response && response.data != null && response.statusCodes === 201) {
        this.openPopup(response.message, "Food logged successfully", true);
        this.foodMacros = []
        this.values = ''
        this.quantity = 0
        this.mealType = 1
        this.selectedFood = null;
        this.searchControl.setValue('', { emitEvent: false })
        this.fetchRecentFoodLogs()

      } else {
        this.openPopup(response.message, "Error occured while calling api", false)
      }
    });
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
