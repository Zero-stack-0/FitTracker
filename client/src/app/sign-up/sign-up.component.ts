import { Component } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Gender } from '../models/gender';
import { ActivityLevel } from '../models/activity-level';
import { FitnessGoal } from '../models/fitness-goal';

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

  signUpForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    fullName: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]),
    password: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]),
    gender: new FormControl(Gender.Male, [Validators.required]),
    age: new FormControl('', [Validators.required, Validators.min(10), Validators.max(120)]),
    weight: new FormControl('', [Validators.required, Validators.min(10), Validators.max(500)]),
    height: new FormControl('', [Validators.required, Validators.min(50), Validators.max(300)]),
    activityLevel: new FormControl(ActivityLevel.LightlyActive, [Validators.required]),
    fitnessGoal: new FormControl(FitnessGoal.MuscleGain, [Validators.required])
  });

  ngOnInit() {
    console.log(FitnessGoal.WeightLoss);
  }
  onSubmit() {
    if (this.signUpForm.invalid) {
      this.signUpForm.markAllAsTouched();
      return;
    }

    const goal = this.signUpForm.value.fitnessGoal;
    console.log(this.signUpForm.value.fitnessGoal);
    if (goal === null || goal === undefined) {
      this.openPopup('Please select a fitness goal', 'Sign Up Error');
      return;
    }
    if (goal.toString() === "WeightLoss") {
      this.signUpForm.value.fitnessGoal = FitnessGoal.WeightLoss;
    } else if (goal.toString() === "MuscleGain") {
      this.signUpForm.value.fitnessGoal = FitnessGoal.MuscleGain;
    } else if (goal.toString() === "MaintainWeight") {
      this.signUpForm.value.fitnessGoal = FitnessGoal.MaintainWeight;
    }
    else if (goal.toString() === "WeightGain") {
      this.signUpForm.value.fitnessGoal = FitnessGoal.WeightGain;
    }
    else {
      this.openPopup('Please select a valid fitness goal', 'Sign Up Error');
      return;
    }
    this.userService.signUp(this.signUpForm.value).subscribe({
      next: (response) => {
        if (response.statusCodes === 201) {
          localStorage.setItem('token', response.data);
          this.route.navigate(['/dashboard']);
        }
        else {
          this.openPopup(response.message || 'Sign up failed. Please try again', 'Sign Up Error');
        }
      },
      error: (error) => {
        this.openPopup(error.message || 'An error occurred during sign up', 'Sign Up Error');
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
