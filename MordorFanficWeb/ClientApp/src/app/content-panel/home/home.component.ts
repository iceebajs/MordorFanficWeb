import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from './../../shared/services/authorization.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private authService: AuthorizationService) { }

  ngOnInit(): void {
    if (this.authService.isSignedIn())
      this.authService.tokenExpirationCheck().pipe(take(1)).subscribe();
  }

}
