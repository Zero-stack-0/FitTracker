import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  menuOpen = false;
  title = 'FitTracker';
  mealType = 1
  constructor(private authService: AuthService, private router: Router) { }
  isUserLoggedWithValidToken: boolean = false;
  ngOnInit() {
    this.authService.isLoggedIn$.subscribe(status => {
      if (status && this.authService.isTokenAvailable() && this.authService.isTokenExpired()) {
        this.isUserLoggedWithValidToken = true;
      }
      else {
        this.isUserLoggedWithValidToken = false;
        this.router.navigate(['/login']);
      }
    });
  }

  logout() {
    this.authService.logout();
    this.isUserLoggedWithValidToken = false;
    this.router.navigate(['/login']);
  }


  toggleMenu() {
    this.menuOpen = !this.menuOpen;
    console.log(this.menuOpen)
  }
}
