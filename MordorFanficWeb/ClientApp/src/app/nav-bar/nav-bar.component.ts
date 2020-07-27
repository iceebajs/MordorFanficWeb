import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizationService } from './../shared/services/authorization.service';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit, OnDestroy {

  isMenuCollapsed = true;
  isLoggedIn: boolean = false;
  subscription: Subscription;

  constructor(private router: Router, private authService: AuthorizationService, private modalService: NgbModal) { }

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


  searchValue: string = '';
  public doFilter = (value: string) => {
    this.searchValue = value;
  }

  find() {
  }

  open(content) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', size: 'lg', scrollable: true, centered: true }).result.then(() => {
      this.searchValue = '';
    }, () => {
    });
  }

}
