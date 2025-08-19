import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  isLoading = false;
  isPasswordOpen = false;
  loadingTitle = "Fetching your details"
  //popup properties
  isOpen = false;
  isGreen = false;
  errorMessage = '';
  poptitle = '';

  userForm = new FormGroup({
    email: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  });
  constructor(private userService: UserService, private router: Router, private authService: AuthService) { }

  onSubmit() {
    if (this.userForm.valid) {
      this.isLoading = true
      this.userService.login(this.userForm.value).subscribe({
        next: (response: any) => {
          if (response.statusCodes !== 200) {
            this.isOpen = true;
            this.isGreen = false;
            this.errorMessage = response.message || 'Login failed. Please try again.';
            this.poptitle = 'Login Error';
            this.isLoading = false
            return;
          }
          localStorage.setItem('token', response.data);
          this.authService.setToken(response.data);
          this.router.navigate(['/dashboard']);
          this.isLoading = false
        },
        error: (error) => {
          this.isLoading = false
          this.isOpen = true;
          this.isGreen = false;
          this.errorMessage = error.error.message || 'Login failed. Please try again.';
          this.poptitle = 'Login Error';
        }
      });
    } else {
      this.isOpen = true;
      this.isGreen = false;
      this.errorMessage = 'Please enter valid details';
      this.poptitle = 'Please enter valid details';
    }
  }
  closePopup() {
    this.isOpen = false;
  }
}
