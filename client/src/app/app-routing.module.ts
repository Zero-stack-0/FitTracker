import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignUpComponent } from './sign-up/sign-up.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FoodLogComponent } from './food-log/food-log.component';
import { ProfileComponent } from './profile/profile.component';
import { ProgressComponent } from './progress/progress.component';
import { FitnessPlanComponent } from './fitness-plan/fitness-plan.component';
import { FoodLogFormComponent } from './food-log-form/food-log-form.component';
import { DietPlanComponent } from './diet-plan/diet-plan.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'sign-up', component: SignUpComponent },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'food-log', component: FoodLogComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'progress', component: ProgressComponent },
  { path: 'fitness-plan', component: FitnessPlanComponent },
  { path: 'food-log-form', component: FoodLogFormComponent },
  { path: 'diet-plan', component: DietPlanComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
