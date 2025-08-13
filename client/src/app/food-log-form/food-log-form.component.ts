import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FoodMacrosService } from '../services/food-macros.service';
import { debounceTime, distinctUntilChanged, filter, switchMap } from 'rxjs';
import { FoodMacrosInterface } from '../models/food-macros-interface';
import { UserFoodLodService } from '../services/user-food-log.service';

@Component({
  selector: 'app-food-log-form',
  templateUrl: './food-log-form.component.html',
  styleUrls: ['./food-log-form.component.css']
})
export class FoodLogFormComponent implements OnInit {
  @ViewChild('container', { static: false }) container!: ElementRef;
  constructor(private foodMacrosService: FoodMacrosService, private userFoodLogForm: UserFoodLodService) { }

  selectedFood: FoodMacrosInterface | null = null;
  mealType = 1
  searchValue = 'egg';
  values = ''
  quantity = 0
  foodMacros: FoodMacrosInterface[] = [];


  ngOnInit(): void {
    this.fetchFoodMacros();
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
    this.foodMacros = [];
    this.values = food.name;
    console.log(this.selectedFood);
  }

  onSubmitFoodLogForm() {
    this.userFoodLogForm.addFoodToLog(this.userFoodLogForm).subscribe((v) => console.log(v.data))
  }

  mealTypeChanges(type: number) {
    this.mealType = type;
    console.log(this.mealType);
  }

  onQuantityInputChange(quan: number) {
    this.quantity = quan;
    console.log(this.quantity);
  }

  onAddFoodToLog() {
    const foodToLog = {
      FoodId: this.selectedFood?.id,
      TimeOfTheDay: this.mealType,
      Quantity: this.quantity
    }

    this.userFoodLogForm.addFoodToLog(foodToLog).subscribe((response) => {
      if (response && response.data != null && response.statusCodes === 201) {
        this.foodMacros = []
        this.values = ''
        this.quantity = 0
        this.mealType = 1
        this.selectedFood = null;

      } else {
        console.warn('Response or data is null');
      }
    });
  }
}
