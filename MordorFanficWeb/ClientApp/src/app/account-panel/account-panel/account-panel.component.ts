import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from './../../shared/services/authorization.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-account-panel',
  templateUrl: './account-panel.component.html',
  styleUrls: ['./account-panel.component.css']
})
export class AccountPanelComponent implements OnInit {

  constructor(private authService: AuthorizationService) { }

  ngOnInit(): void {
    this.authService.tokenExpirationCheck().pipe(take(1)).subscribe();
  }

  isAdmin() {
    return localStorage.getItem('role') === 'admin' ? true : false;
  }
}
