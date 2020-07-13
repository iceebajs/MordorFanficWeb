import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthorizationService } from '../services/authorization.service';

@Injectable()
export class RouteGuard implements CanActivate {
  constructor(private authService: AuthorizationService, private router: Router) {}

  canActivate() {

    if (!this.authService.isSignedIn())
    {
       this.router.navigate(['/']);
       return false;
    }

    return true;
  }
}
