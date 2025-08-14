import { Component } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Gender } from '../models/gender';
import { ActivityLevel } from '../models/activity-level';
import { DietType, FitnessGoal } from '../models/fitness-goal';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
  constructor(private userService: UserService, private route: Router) { }

  //popup properties
  isOpen = false;
  isGreen = false;
  errorMessage = '';
  poptitle = '';
  selectedGoal: FitnessGoal = FitnessGoal.WeightLoss;

  genderOptions = Object.keys(Gender)
    .filter(key => isNaN(Number(key)))
    .map(key => ({
      label: key,
      value: Gender[key as keyof typeof Gender]
    }));

  activityOptions = Object.keys(ActivityLevel)
    .filter(key => isNaN(Number(key)))
    .map(key => ({
      label: key,
      value: ActivityLevel[key as keyof typeof ActivityLevel]
    }));

  fitnessGoalOptions = Object.keys(FitnessGoal)
    .filter(key => isNaN(Number(key)))
    .map(key => ({
      label: key,
      value: FitnessGoal[key as keyof typeof FitnessGoal]
    }));

  dietOptions = Object.keys(DietType)
    .filter(key => isNaN(Number(key)))
    .map(key => ({
      label: key,
      value: DietType[key as keyof typeof DietType]
    }));

  goalEmoji(value: FitnessGoal): string {
    switch (value) {
      case FitnessGoal.WeightLoss:
        return '🔥';
      case FitnessGoal.MuscleGain:
        return '💪';
      case FitnessGoal.MaintainWeight:
        return '🌟';
      default:
        return '💪';
    }
  }
  goalTitle(value: FitnessGoal): string {
    switch (value) {
      case FitnessGoal.WeightLoss:
        return 'Weight Loss';
      case FitnessGoal.MuscleGain:
        return 'Muscle Gain';
      case FitnessGoal.MaintainWeight:
        return 'Maintain Weight';
      default:
        return 'Gain Weight';
    }
  }

  goalDescription(value: FitnessGoal): string {
    switch (value) {
      case FitnessGoal.WeightLoss:
        return 'Burn calories and shed pounds safely';
      case FitnessGoal.MuscleGain:
        return 'Build muscle mass and increase strength';
      case FitnessGoal.MaintainWeight:
        return 'Maintain your current weight and stay healthy';
      default:
        return 'Stay healthy and gain weight';
    }
  }

  dietaryTitle(value: DietType): string {
    switch (value) {
      case DietType.VEG:
        return 'Vegetarian';
      case DietType.NON_VEG:
        return 'Non-Vegetarian';
      default:
        return 'Both';
    }
  }

  dietaryTitleEmoji(value: DietType): string {
    switch (value) {
      case DietType.VEG:
        return '🥬';
      case DietType.NON_VEG:
        return '🍗';
      default:
        return '🍽️';
    }
  }

  dietaryDescription(value: DietType): string {
    switch (value) {
      case DietType.VEG:
        return 'Plant-based meals only';
      case DietType.NON_VEG:
        return 'Includes meat and dairy';
      default:
        return 'Flexible with all food types';
    }
  }

  signUpForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    fullName: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]),
    password: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]),
    gender: new FormControl(Gender.Male, [Validators.required]),
    age: new FormControl('', [Validators.required, Validators.min(10), Validators.max(120)]),
    weight: new FormControl('', [Validators.required, Validators.min(10), Validators.max(500)]),
    height: new FormControl('', [Validators.required, Validators.min(50), Validators.max(300)]),
    activityLevel: new FormControl(ActivityLevel.LightlyActive, [Validators.required]),
    fitnessGoal: new FormControl(FitnessGoal.MuscleGain, [Validators.required]),
    dietType: new FormControl(DietType.VEG, [Validators.required]),
    location: new FormControl("",),
  });

  ngOnInit() {
    console.log(FitnessGoal.WeightLoss);
  }
  onSubmit() {
    if (this.signUpForm.invalid) {
      this.signUpForm.markAllAsTouched();
      return;
    }



    // const goal = this.signUpForm.value.fitnessGoal;
    // console.log(this.signUpForm.value.fitnessGoal);
    // if (goal === null || goal === undefined) {
    //   this.openPopup('Please select a fitness goal', 'Sign Up Error');
    //   return;
    // }
    // if (goal.toString() === "WeightLoss") {
    //   this.signUpForm.value.fitnessGoal = FitnessGoal.WeightLoss;
    // } else if (goal.toString() === "MuscleGain") {
    //   this.signUpForm.value.fitnessGoal = FitnessGoal.MuscleGain;
    // } else if (goal.toString() === "MaintainWeight") {
    //   this.signUpForm.value.fitnessGoal = FitnessGoal.MaintainWeight;
    // }
    // else if (goal.toString() === "WeightGain") {
    //   this.signUpForm.value.fitnessGoal = FitnessGoal.WeightGain;
    // }
    // else {
    //   this.openPopup('Please select a valid fitness goal', 'Sign Up Error');
    //   return;
    // }
    // this.userService.signUp(this.signUpForm.value).subscribe({
    //   next: (response) => {
    //     if (response.statusCodes === 201) {
    //       localStorage.setItem('token', response.data);
    //       this.route.navigate(['/dashboard']);
    //     }
    //     else {
    //       this.openPopup(response.message || 'Sign up failed. Please try again', 'Sign Up Error');
    //     }
    //   },
    //   error: (error) => {
    //     this.openPopup(error.message || 'An error occurred during sign up', 'Sign Up Error');
    //   }
    // });

    console.log(this.signUpForm.value);

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
