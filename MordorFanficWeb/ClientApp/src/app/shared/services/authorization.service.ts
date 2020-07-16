import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Router } from '@angular/router'

import { Credentials } from './../interfaces/credentials.interface';
import { AuthResponse } from '../interfaces/auth-responce.interface';
import { ConfigService } from './../common/config.service';
import { HttpErrorHandler, HandleError } from './../helpers/http-error-hander.service';

@Injectable()
export class AuthorizationService {
  baseURL = '';
  handleError: HandleError;

  authStatusSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  private isLoggedIn: boolean = false;

  constructor(private httpClient: HttpClient, private configService: ConfigService,
    private httpErrorHandler: HttpErrorHandler, private router: Router) {
    this.isLoggedIn = !!this.getAuthorizationToken();
    this.authStatusSubject.next(this.isLoggedIn);
    this.baseURL = this.configService.getApiURI();
    this.handleError = this.httpErrorHandler.createHandleError('AuthenticationService');
  }

  login(user: Credentials) {
    return this.httpClient.post<AuthResponse>(`${this.baseURL}/authentication`, user, { observe: 'response' })
      .pipe(map(response => {
        localStorage.setItem('auth_token', response.body.auth_token);
        localStorage.setItem('id', response.body.id);
        this.isLoggedIn = true;
        this.authStatusSubject.next(this.isLoggedIn);
        return true;
      }),
      catchError(this.handleError('login', user)));
  }

  logout() {
    this.removeUserData();
    this.isLoggedIn = false;
    this.authStatusSubject.next(this.isLoggedIn);
    this.router.navigate(['/']);
  }

  tokenExpirationCheck() {
    const headers = this.setUserHeaders();

    return this.httpClient.get(`${this.baseURL}/account/expiration`, { headers: headers })
  }

  getAuthorizationToken() {
    return localStorage.getItem('auth_token');
  }

  removeUserData() {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('id');
  }

  isSignedIn() {
    return this.isLoggedIn;
  }

  setUserHeaders() {
    let authToken = localStorage.getItem('auth_token');
    return new HttpHeaders({ 'Authorization': `Bearer ${authToken}` });
  }
}
