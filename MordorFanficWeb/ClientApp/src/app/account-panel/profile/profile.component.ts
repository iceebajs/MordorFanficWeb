import { Component, OnInit } from '@angular/core';
import { AccountService } from './../../shared/services/account.service';
import { User } from './../../shared/interfaces/user.interface';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  providers: [AccountService]
})
export class ProfileComponent implements OnInit {

  constructor(private accountService: AccountService) { }

  currentUser: User;
  dataLoaded: Promise<boolean>;

  ngOnInit(): void {
    this.accountService.getUserById(localStorage.getItem('id'))
      .pipe(take(1))
      .subscribe((response: User) => {
        this.currentUser = response;
        this.dataLoaded = Promise.resolve(true);
      },
        error => console.log(error));
  }


}