import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';
import { AuthorizationService } from './../shared/services/authorization.service';
import { Credentials } from './../shared/interfaces/credentials.interface';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {

  hide: boolean = true;
  subscription: Subscription;

  brandNew: boolean;
  hasError = false;

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private authService: AuthorizationService) { }

  ngOnInit(): void {
    this.subscription = this.activatedRoute.queryParams
      .pipe(take(1))
      .subscribe(params => {
        this.brandNew = params['brandNew'];
      });
    if (this.authService.isSignedIn())
      this.router.navigate(['account/profile']);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  login() {
    if (this.email.valid && this.password.valid) {
      const credentials: Credentials = { email: this.email.value, password: this.password.value } as Credentials;
      this.authService.login(credentials)
        .pipe(take(1))
        .subscribe(result => {
          if (result) {
            this.router.navigate(['/']);
          }
        },
          error => {
            console.log(error);
            this.brandNew = false;
            this.hasError = true;
          });
    }
  }

  email = new FormControl('', [Validators.required]);
  errorEmail() {
    if (this.email.hasError('required'))
      return 'You must enter a value';
  }

  password = new FormControl('', [Validators.required]);
  errorPassword() {
    if (this.password.hasError('required'))
      return 'You must enter a value';
  }
}
