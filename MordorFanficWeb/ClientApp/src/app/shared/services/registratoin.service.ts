import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HandleError, HttpErrorHandler } from './../helpers/http-error-hander.service';
import { ConfigService } from '../common/config.service';
import { UserRegistrationInterface } from '../interfaces/registration.interface';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class RegistrationService {
  baseURL: string = '';
  handleError: HandleError;

  constructor(private httpClient: HttpClient,
    private configService: ConfigService,
    private httpErrorHandler: HttpErrorHandler,
  ) {
    this.baseURL = this.configService.getApiURI();
    this.handleError = this.httpErrorHandler.createHandleError('RegistrationService');
  }

  register(user: UserRegistrationInterface) : Observable<UserRegistrationInterface> {
    return this.httpClient.post<UserRegistrationInterface>(`${this.baseURL}/account/register-user`, user)
      .pipe(
        catchError(this.handleError('register', user))
      );

  }
}
