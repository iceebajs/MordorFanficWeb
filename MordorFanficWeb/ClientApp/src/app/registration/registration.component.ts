import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegistrationService } from './../shared/services/registratoin.service';
import { UserRegistrationInterface } from '../shared/interfaces/registration.interface';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  providers: [RegistrationService]
})
export class RegistrationComponent implements OnInit {

  hide = true;
  hideConf = true;
  hasError: boolean = false;
  requestStatus: any;
  errorMessage = [];

  constructor(private router: Router, private registrationService: RegistrationService) { }

  ngOnInit(): void {
  }

  registerUser() {
    if (this.userModelIsValid()) {
      const userRegistrationModel: UserRegistrationInterface = {
        firstName: this.firstName.value,
        lastName: this.lastName.value,
        userName: this.userName.value,
        email: this.email.value,
        emailConfirm: this.emailConfirm.value,
        password: this.password.value,
        passwordConfirm: this.passwordConfirm.value
      } as UserRegistrationInterface;

      this.registrationService.register(userRegistrationModel)
        .pipe(take(1))
        .subscribe(
          result => {
            if (result)
              this.router.navigate(['login'], { queryParams: { brandNew: true } })
          },
          () => {
            this.setErrors();
          });
    }
  }

  private setErrors() {
    this.hasError = true;
    let errResponse = this.registrationService.getErrorMessage();
    if (errResponse['errors'] !== undefined)
      errResponse = errResponse['errors'];
    let errors = [];
    for (const [, value] of Object.entries(errResponse)) {
      errors.push(value);
    }
    this.errorMessage = errors;
  }

  userModelIsValid() {
    let isValid =
      (this.firstName.valid &&
        this.lastName.valid &&
        this.userName.valid &&
        this.email.valid &&
        this.emailConfirm.valid &&
        this.password.valid &&
        this.passwordConfirm.valid) ? true : false;
    return isValid;
  }

  firstName = new FormControl('', [Validators.required]);
  errorFirstName() {
    if (this.userName.hasError('required'))
      return 'You must enter a value';
  }

  lastName = new FormControl('', [Validators.required]);
  errorLastName() {
    if (this.userName.hasError('required'))
      return 'You must enter a value';
  }

  userName = new FormControl('', [Validators.required]);
  errorUserName() {
    if (this.userName.hasError('required'))
      return 'You must enter a value';
  }

  email = new FormControl('', [Validators.required, Validators.email]);
  errorEmail() {
    if (this.email.hasError('required'))
      return 'You must enter a value';
    return this.email.hasError('email') ? 'Not a valid email' : '';
  }

  emailConfirm = new FormControl('', [Validators.required, Validators.email]);
  errorEmailConfirm() {
    if (this.emailConfirm.hasError('required'))
      return 'You must enter a value';
    return this.emailConfirm.hasError('email') ? 'Not a valid email' : '';
  }

  password = new FormControl('', [Validators.required]);
  errorPassword() {
    if (this.password.hasError('required'))
      return 'You must enter a value';
  }

  passwordConfirm = new FormControl('', [Validators.required]);
  errorPasswordConfirm() {
    if (this.password.hasError('required'))
      return 'You must enter a value';
  }

}
