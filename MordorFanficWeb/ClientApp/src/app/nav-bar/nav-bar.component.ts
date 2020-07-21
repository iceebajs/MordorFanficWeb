import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizationService } from './../shared/services/authorization.service';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit, OnDestroy {

  isMenuCollapsed = true;
  isLoggedIn: boolean = false;
  subscription: Subscription;

  constructor(private router: Router, private authService: AuthorizationService) { }

  ngOnInit(): void {
    if (this.authService.isSignedIn())
      this.authService.tokenExpirationCheck().pipe(take(1)).subscribe();
    this.subscription = this.authService.authStatusSubject.
      asObservable().
      subscribe(isLoggedIn => {
        this.isLoggedIn = isLoggedIn;
      });
  }

  logout() {
    this.authService.logout();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  goTo(route: string): void {
    this.router.navigate([route]);
  }
}
