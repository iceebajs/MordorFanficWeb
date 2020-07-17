import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RouteGuard } from './../shared/common/route.guard';
import { AdminRouteGuard } from './../shared/common/admin-route.guard';

import { ProfileComponent } from './profile/profile.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { AccountPanelComponent } from './account-panel/account-panel.component';

const childRoutes: Routes = [
  { path: 'profile', component: ProfileComponent },
  { path: 'change-password', component: ChangePasswordComponent },
  { path: 'admin-dashboard', component: AdminDashboardComponent, canActivate: [AdminRouteGuard] }
];

const routes: Routes = [
  {path: 'account', component: AccountPanelComponent, children: childRoutes, canActivate: [RouteGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [RouteGuard, AdminRouteGuard]
})
export class AccountPanelRoutingModule { }
