import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Gender } from '../models/gender';
import { ActivityLevel } from '../models/activity-level';
import { DietType, FitnessGoal } from '../models/fitness-goal';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})


export class ProfileComponent implements OnInit {
  profileForm = new FormGroup({
    email: new FormControl({ value: '', disabled: true }, [Validators.required, Validators.email]),
    fullName: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]),
    gender: new FormControl(Gender.Male, [Validators.required]),
    age: new FormControl('', [Validators.required, Validators.min(10), Validators.max(120)]),
    height: new FormControl('', [Validators.required, Validators.min(5), Validators.max(300)])
  });

  fitnessGoalForm = new FormGroup({
    weight: new FormControl('', [Validators.required, Validators.min(10), Validators.max(500)]),
    activityLevel: new FormControl(ActivityLevel.LightlyActive, [Validators.required]),
    fitnessGoal: new FormControl(FitnessGoal.MuscleGain, [Validators.required]),
    dietType: new FormControl(DietType.VEG, [Validators.required]),
    dailyCalorieGoal: new FormControl("",)
  });
  user: any
  constructor(private userService: UserService) { }

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



  ngOnInit(): void {
    this.fetchUserProfile()
  }

  fetchUserProfile() {
    this.userService.userInformation().subscribe((res) => {
      console.log(res)
      this.user = res.data;
      this.profileForm.patchValue({
        email: this.user?.email || '',
        fullName: this.user?.fullName || '',
        gender: this.user?.userInformation?.gender || Gender.Male,
        age: this.user?.userInformation.age || '',

        height: this.user?.userInformation.height || ''
      });
      this.fitnessGoalForm.patchValue({
        weight: this.user?.userInformation.weight || '',
        activityLevel: this.user?.userInformation?.activityLevel || ActivityLevel.LightlyActive,
        fitnessGoal: this.user?.userInformation.fitnessGoal || FitnessGoal.MuscleGain,
        dietType: this.user?.userInformation.dietType || DietType.VEG,
        dailyCalorieGoal: this.user?.macroTargets?.calories || ''
      })

    });
    console.log(this.user)
  }


}
