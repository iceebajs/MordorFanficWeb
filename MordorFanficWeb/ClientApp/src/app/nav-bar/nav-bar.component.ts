import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizationService } from './../shared/services/authorization.service';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CompositionService } from '../shared/services/composition.service';
import { UserKeyword } from '../shared/interfaces/composition/keyword.interface';
import { Composition } from '../shared/interfaces/composition/composition.interface';
import { Rating } from '../shared/interfaces/composition/rating.interface';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css'],
  providers: [CompositionService]
})
export class NavBarComponent implements OnInit, OnDestroy {

  isMenuCollapsed = true;
  isLoggedIn: boolean = false;
  subscription: Subscription;

  constructor(private router: Router, private authService: AuthorizationService,
    private modalService: NgbModal, private compositionService: CompositionService) { }

  ngOnInit(): void {
    if (this.authService.isSignedIn())
      this.authService.tokenExpirationCheck().pipe(take(1)).subscribe();
    this.subscription = this.authService.authStatusSubject.
      asObservable().
      subscribe(isLoggedIn => {
        this.isLoggedIn = isLoggedIn;
        this.isAdminUser = this.isAdmin();
        this.isAdminUser ? this.asUserName = localStorage.getItem('asUserEmail') : this.asUserName = undefined;
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


  foundCompositions: Composition[] = [];
  searchValue: string = '';
  public doFilter = (value: string) => {
    this.searchValue = value;
  }

  find(content) {
    this.foundCompositions.length = 0;
    if (this.searchValue.length > 0) {
      const keyword: UserKeyword = { keyword: this.searchValue } as UserKeyword;
      this.searchValue = '';
      this.compositionService.findComposition(keyword)
        .pipe(take(1))
        .subscribe((response: Composition[]) => {
          this.foundCompositions = response;
          this.foundCompositions.length > 0 ? this.notFound = false : this.notFound = true;
          this.calculateRatings();
          this.open(content);
        });
    }
  }

  open(content) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', size: 'lg', scrollable: true, centered: true }).result.then(() => {
    }, () => {
    });
  }

  notFound: boolean = false;
  compositionRating: number[] = [];
  calculateRatings() {
    this.compositionRating.length = 0;
    for (let composition of this.foundCompositions) {
      this.compositionRating.push(calcRating(composition.compositionRatings));
    }


    function calcRating(ratings: Rating[]) {
      let summary = 0;
      if (ratings.length > 0) {
        ratings.forEach(rating => {
          summary = summary + rating.rating;
        });
        return (summary / ratings.length);
      }
      else
        return summary;
    }
  }

  readComposition(id: number) {
    this.router.navigate(['']);
    setTimeout(() => this.router.navigate(['read', id]), 1000);
  }

  isAdminUser: boolean = false;
  asUserName: string;
  asUserLogout() {
    localStorage.removeItem('asUserEmail');
    localStorage.removeItem('asUserId');
    location.reload();
  }
  isAdmin() {
    return localStorage.getItem('role') === 'admin' ? true : false;
  }
}
