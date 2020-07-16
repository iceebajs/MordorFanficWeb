import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RouteGuard } from './../shared/common/route.guard';
import { ProfileComponent } from './profile/profile.component';
import { ChangePasswordComponent } from './change-password/change-password.component';

const routes: Routes = [
  { path: 'profile', component: ProfileComponent, canActivate: [RouteGuard] },
  { path: 'change-password', component: ChangePasswordComponent, canActivate: [RouteGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [RouteGuard]
})
export class AccountPanelRoutingModule { }
