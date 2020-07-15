import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HandleError ,HttpErrorHandler } from './../helpers/http-error-hander.service';
import { ConfigService } from '../common/config.service';
import { UserRegistration } from '../interfaces/registration.interface';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class RegistrationService {
  baseURL: string = '';
  handleError: HandleError;

  constructor(private httpClient: HttpClient, private configService: ConfigService, private httpErrorHandler: HttpErrorHandler) {
    this.baseURL = this.configService.getApiURI();
    this.handleError = this.httpErrorHandler.createHandleError('RegistrationService');
  }

  register(user: UserRegistration) : Observable<UserRegistration> {
    return this.httpClient.post<UserRegistration>(`${this.baseURL}/account/register-user`, user)
      .pipe(
        catchError(this.handleError('register', user))
      );
  }

  getErrorMessage() {
    return this.httpErrorHandler.getError();
  }
}
