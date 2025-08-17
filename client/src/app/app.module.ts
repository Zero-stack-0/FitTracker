import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FoodLogComponent } from './food-log/food-log.component';
import { ProfileComponent } from './profile/profile.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { PopupComponent } from './pop-up/pop-up.component';
import { FitnessPlanComponent } from './fitness-plan/fitness-plan.component';
import { FoodLogFormComponent } from './food-log-form/food-log-form.component';
import { AuthInterceptor } from './services/auth.interceptor';
import { DietPlanComponent } from './diet-plan/diet-plan.component';
import { LoaderComponent } from './loader/loader.component';
import { FoodLogHistoryComponent } from './food-log-history/food-log-history.component';
@NgModule({
  declarations: [
    AppComponent,
    SignUpComponent,
    HomeComponent,
    LoginComponent,
    DashboardComponent,
    FoodLogComponent,
    ProfileComponent,
    PopupComponent,
    FitnessPlanComponent,
    FoodLogFormComponent,
    DietPlanComponent,
    LoaderComponent,
    FoodLogHistoryComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    CommonModule,
    FormsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
