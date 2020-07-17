import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthorizationService } from '../services/authorization.service';

@Injectable()
export class AdminRouteGuard implements CanActivate {
  constructor(private authService: AuthorizationService, private router: Router) { }
  
  canActivate() {
    if (!this.isAdmin()) {
      this.router.navigate(['/account/profile']);
      return false;
    }
    return true;
  }

  isAdmin() {
    return localStorage.getItem('role') === 'admin' ? true : false;
  }
}
