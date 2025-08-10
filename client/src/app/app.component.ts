import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'FitTracker';
  constructor(private authService: AuthService, private router: Router) { }
  isUserLoggedWithValidToken: boolean = false;
  ngOnInit() {
    if (this.authService.isLoggedIn$) {
      this.isUserLoggedWithValidToken = this.authService.isTokenExpired();
      console.log(this.isUserLoggedWithValidToken);
    }
  }

  logout() {
    this.authService.logout();
    this.isUserLoggedWithValidToken = false;
    this.router.navigate(['/login']);
  }
}
