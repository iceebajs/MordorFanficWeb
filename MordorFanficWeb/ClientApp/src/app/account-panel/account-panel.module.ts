import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { EditorModule } from '@progress/kendo-angular-editor';
import { MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { DragDropModule } from '@angular/cdk/drag-drop';

import { AccountPanelRoutingModule } from './account-panel-routing.module';
import { ProfileComponent } from './profile/profile.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { AccountPanelComponent } from './account-panel/account-panel.component';
import { CreateCompositionComponent } from './create-composition/create-composition.component';
import { ManageCompositionComponent } from './manage-composition/manage-composition.component';


@NgModule({
  declarations: [ProfileComponent, ChangePasswordComponent, AdminDashboardComponent, AccountPanelComponent, CreateCompositionComponent, ManageCompositionComponent],
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
    NgbModule,
    EditorModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatSortModule,
    MatSelectModule,
    DragDropModule
  ]
})
export class AccountPanelModule { }
