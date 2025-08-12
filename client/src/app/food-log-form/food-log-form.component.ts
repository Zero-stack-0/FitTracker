import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { FoodMacrosService } from '../services/food-macros.service';
import { debounceTime, distinctUntilChanged, filter, switchMap } from 'rxjs';
import { FoodMacrosInterface } from '../models/food-macros-interface';

@Component({
  selector: 'app-food-log-form',
  templateUrl: './food-log-form.component.html',
  styleUrls: ['./food-log-form.component.css']
})
export class FoodLogFormComponent implements OnInit {
  @ViewChild('container', { static: false }) container!: ElementRef;
  constructor(private foodMacrosService: FoodMacrosService) { }

  searchValue = 'egg';
  values = ''
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

  onAddFoodToLog(food: FoodMacrosInterface) {
    this.foodMacros = [];
    this.values = '';
    console.log(food);
  }
}
