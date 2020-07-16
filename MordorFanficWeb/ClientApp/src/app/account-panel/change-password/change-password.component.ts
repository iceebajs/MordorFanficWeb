import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from './../../shared/services/account.service';
import { ChangePassword } from './../../shared/interfaces/change-password.interface';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css'],
  providers: [AccountService]
})
export class ChangePasswordComponent implements OnInit {

  hide = true;
  hideConf = true;
  hasError: boolean = false;
  requestStatus: any;
  errorMessage = [];

  constructor(private router: Router, private accService: AccountService) { }

  ngOnInit(): void {
  }

  registerUser() {
    if (this.dataIsValid()) {
      const changePassword: ChangePassword = {
        id: localStorage.getItem('id'),
        oldPassword: this.oldPassword.value,
        newPassword: this.password.value,
        newPasswordConfirm: this.passwordConfirm.value
      } as ChangePassword;

      this.accService.changePassword(changePassword)
        .pipe(take(1))
        .subscribe(
          result => {
            if (result)
              this.router.navigate(['/profile'])
          },
          () => {
            this.setErrors();
          });
    }
  }

  private setErrors() {
    this.hasError = true;
    let errResponse = this.accService.getErrorMessage();
    if (errResponse['errors'] !== undefined)
      errResponse = errResponse['errors'];
    let errors = [];
    for (const [, value] of Object.entries(errResponse)) {
      errors.push(value);
    }
    this.errorMessage = errors;
  }

  dataIsValid() {
    let isValid =  
        (this.oldPassword.valid &&
        this.password.valid &&
        this.passwordConfirm.valid) ? true : false;
    return isValid;
  }

  oldPassword = new FormControl('', [Validators.required]);
  errorOldPassword() {
    if (this.password.hasError('required'))
      return 'You must enter a value';
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
