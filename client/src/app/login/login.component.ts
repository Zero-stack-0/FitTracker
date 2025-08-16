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
  isPasswordOpen = false
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
      this.userService.login(this.userForm.value).subscribe({
        next: (response: any) => {
          if (response.statusCodes !== 200) {
            this.isOpen = true;
            this.isGreen = false;
            this.errorMessage = response.message || 'Login failed. Please try again.';
            this.poptitle = 'Login Error';
            return;
          }
          localStorage.setItem('token', response.data);
          this.authService.setToken(response.data);
          this.router.navigate(['/dashboard']);
        },
        error: (error) => {
          this.isOpen = true;
          this.isGreen = false;
          this.errorMessage = error.error.message || 'Login failed. Please try again.';
          this.poptitle = 'Login Error';
        }
      });
    } else {
      console.error('Form is invalid');

    }
  }
  closePopup() {
    this.isOpen = false;
  }
}
