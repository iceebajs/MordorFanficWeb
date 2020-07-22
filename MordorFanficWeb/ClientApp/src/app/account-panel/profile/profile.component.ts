import { Component, OnInit } from '@angular/core';
import { AccountService } from './../../shared/services/account.service';
import { User } from './../../shared/interfaces/user.interface';
import { take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { CompositionService } from './../../shared/services/composition.service';
import { Composition } from '../../shared/interfaces/composition/composition.interface';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  providers: [AccountService, CompositionService]
})
export class ProfileComponent implements OnInit {

  constructor(private accountService: AccountService, private compositionService: CompositionService, private router: Router) { }

  currentUser: User;
  accountId: number;
  dataLoaded: Promise<boolean>;
  accountCompositions: Composition[];

  ngOnInit(): void {
    this.accountService.getUserById(localStorage.getItem('id'))
      .pipe(take(1))
      .subscribe((response: User) => {
        this.currentUser = response;
        this.dataLoaded = Promise.resolve(true);
      },
        error => console.log(error));
  }

  goTo(route) {
    this.router.navigate([route]);
  }

  getUserById() {
    this.accountService.getUserAccountId(this.currentUser.id).pipe(take(1)).subscribe(response => {
      this.accountId = response;
      this.getAccountCompositions();
    });
  }

  getAccountCompositions() {
    this.compositionService.getAccountCompositions(this.accountId)
      .pipe(take(1))
      .subscribe((response: Composition[]) => {
        this.accountCompositions = response;
        console.log(this.accountCompositions);
      });
    this.compositionService.getCompositionById(14)
      .pipe(take(1))
      .subscribe((response: Composition) => {
        console.log(response);
      });
  }
}
