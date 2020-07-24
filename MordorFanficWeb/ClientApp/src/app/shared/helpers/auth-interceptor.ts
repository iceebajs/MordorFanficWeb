import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest
} from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { AuthorizationService } from './../services/authorization.service';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthorizationService, private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${this.authService.getAuthorizationToken()}`,
      }
    });

    return next.handle(request).pipe(
      catchError((error) => {
        if (error.status === 401) {
          this.handleAuthError();
          return of(error);
        }
        throw error;
      }));
  }

  private handleAuthError() {
    this.authService.logout();
    this.router.navigate(['login']);
  }
}
