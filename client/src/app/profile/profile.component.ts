import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Gender } from '../models/gender';
import { ActivityLevel } from '../models/activity-level';
import { DietType, FitnessGoal } from '../models/fitness-goal';
import { UserDietPlanService } from '../services/user-diet-plan.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})


export class ProfileComponent implements OnInit {
  constructor(private userService: UserService, private userDietPlan: UserDietPlanService) { }
  limitReachedToUpdateDietPlan = false
  //popup
  isOpen = false;
  isGreen = false;
  errorMessage = '';
  poptitle = '';
  user: any
  isLoading = false
  goalUpdatePopUp = false;



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


  profileForm = new FormGroup({
    email: new FormControl({ value: '', disabled: true }, [Validators.required, Validators.email]),
    fullName: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]),
    gender: new FormControl({ value: '', disabled: true }, [Validators.required]),
    age: new FormControl('', [Validators.required, Validators.min(10), Validators.max(120)]),
    height: new FormControl('', [Validators.required, Validators.min(50), Validators.max(300)]),
    weight: new FormControl({ value: '', disabled: this.limitReachedToUpdateDietPlan }, [Validators.required, Validators.min(10), Validators.max(500)]),
    activityLevel: new FormControl({ value: ActivityLevel.LightlyActive, disabled: this.limitReachedToUpdateDietPlan }, [Validators.required]),
    fitnessGoal: new FormControl({ value: '', disabled: this.limitReachedToUpdateDietPlan }, [Validators.required]),
    dietType: new FormControl({ value: DietType.VEG, disabled: this.limitReachedToUpdateDietPlan }, [Validators.required]),
    dailyCalorieGoal: new FormControl({ value: '', disabled: true },),
    canUpdateDietPlan: new FormControl(!!this.limitReachedToUpdateDietPlan)
  });
  ngOnInit(): void {
    this.fetchUserProfile()
    this.fetchCanUserUpdateDietPlan()
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
        height: this.user?.userInformation?.height || '',
        weight: this.user?.userInformation.weight || '',
        activityLevel: this.user?.userInformation?.activityLevel || ActivityLevel.LightlyActive,
        fitnessGoal: this.user?.userInformation.fitnessGoal || FitnessGoal.MuscleGain,
        dietType: this.user?.userInformation.dietType || DietType.VEG,
        dailyCalorieGoal: this.user?.macroTargets?.calories || ''
      });
    });
  }

  saveProfileChanges() {
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }
    if (!this.limitReachedToUpdateDietPlan) {
      if (!(this.profileForm.value['weight'] == this.user?.userInformation?.weight) || !(this.profileForm.value['activityLevel'] == this.user?.userInformation?.activityLevel)
        || !(this.profileForm.value['fitnessGoal'] == this.user?.userInformation?.fitnessGoal) || !(this.profileForm.value['dietType'] == this.user?.userInformation?.dietType)
      ) {
        this.goalUpdatePopUp = true;
        return
      }
    }

    if (this.user?.fullName !== this.profileForm.value['fullName'] || this.user?.userInformation?.age !== this.profileForm.value['age']
      || this.user?.userInformation?.height !== this.profileForm.value['height']) {
      this.updateProfileChanges();
      return;
    }

  }

  closeGoalUpdatePopUp() {
    this.goalUpdatePopUp = false
  }

  updateGoalChanges() {
    this.goalUpdatePopUp = false
  }

  fetchCanUserUpdateDietPlan() {
    this.userDietPlan.canUserUpdateDietPlan().subscribe((res) => {
      this.limitReachedToUpdateDietPlan = !res.data
      if (this.limitReachedToUpdateDietPlan) {
        this.disableRestrictedFields();
      }
    })
  }

  updateProfileChanges() {
    if (this.profileForm.valid) {
      this.closeGoalUpdatePopUp()
      this.isLoading = true;
      console.log(this.profileForm.value)
      this.userDietPlan.updateProfile(this.profileForm.value).subscribe({
        next: (res: any) => {
          if (res.statusCodes === 200) {
            this.isLoading = false;
            console.log("profile updated")
            return
          }
          else {
            this.isLoading = false;
            this.isOpen = true;
            this.isGreen = false;
            this.errorMessage = res?.message;
            this.poptitle = "Cannot update profile"
            return
          }
        },
        error: (res: any) => {
          this.isLoading = false;

        }
      })
    }

  }

  closePopup() {
    this.isOpen = false;
  }
  disableRestrictedFields() {
    console.log("work")
    this.profileForm.get('weight')?.disable();
    this.profileForm.get('activityLevel')?.disable();
    this.profileForm.get('dietType')?.disable();
    this.profileForm.get('fitnessGoal')?.disable();
  }
}
