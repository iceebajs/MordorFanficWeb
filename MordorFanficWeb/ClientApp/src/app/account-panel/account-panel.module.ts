import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';

import { AccountPanelRoutingModule } from './account-panel-routing.module';
import { ProfileComponent } from './profile/profile.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { AccountPanelComponent } from './account-panel/account-panel.component';


@NgModule({
  declarations: [ProfileComponent, ChangePasswordComponent, AdminDashboardComponent, AccountPanelComponent],
  imports: [
    CommonModule,
    AccountPanelRoutingModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatButtonModule,
    MatTableModule,
    MatPaginatorModule,
    NgbModule
  ]
})
export class AccountPanelModule { }
