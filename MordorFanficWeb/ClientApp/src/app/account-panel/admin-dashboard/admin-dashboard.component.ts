import { Component, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '../../shared/services/account.service';
import { User } from '../../shared/interfaces/user.interface';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { take } from 'rxjs/operators';
import { UpdateUserStatus } from '../../shared/interfaces/update-user-status.interface';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css'],
  providers: [AccountService]
})
export class AdminDashboardComponent implements OnInit {

  constructor(private accService: AccountService, private modalService: NgbModal) { }

  ngOnInit() {
    this.dataSource.paginator = this.paginator;
    this.accService.getUsersList()
      .pipe(take(1))
      .subscribe((users: User[]) => {
        this.dataSource.data = users;
        this.usersList = users;
        this.dataLoaded = Promise.resolve(true);
      });
  }

  dataLoaded: Promise<boolean>;

  displayedColumns: string[] = ['userName', 'email', 'creationDate', 'lastVisit', 'status', 'asUser', 'changeRole', 'block', 'delete'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  usersList: User[];
  userStatus: UpdateUserStatus = { id: '', accountStatus: true} as UpdateUserStatus;
  dataSource = new MatTableDataSource<User>();
  selection = new SelectionModel<User>(true, []);

  setUserStatusHasError: boolean = false;
  setUserStatus(user: User) {
    this.selectUser(user);
    this.currentUserStatus(user);
    this.accService.updateUserStatus(this.userStatus).pipe(take(1)).subscribe(() => { },
      () => {
        this.setUserStatusHasError = true;
        this.selectUser(user);
        setTimeout(() => this.setUserStatusHasError = false, 3000);
      });
    this.selection = new SelectionModel<User>(true, []);
  }

  private selectUser(user: User) {
    let index: number = this.usersList.findIndex(u => u === user);
    this.usersList[index].accountStatus == true ? this.usersList[index].accountStatus = false : this.usersList[index].accountStatus = true;
    this.dataSource.data = this.usersList;
  }

  private currentUserStatus(user: User) {
    this.setUserStatusHasError = false;
    this.userStatus.id = user.id;
    this.userStatus.accountStatus = user.accountStatus;
  }

  userEmailToDelete: string = '';
  deleteModalHasError: boolean = false;
  deleteUser(deleteUserModal, user: User) {
    this.userEmailToDelete = user.email;
    this.deleteModalHasError = false;
    this.modalService.open(deleteUserModal, { ariaLabelledBy: 'modal-delete' }).result.then((result) => {
      if (result === true) {
        this.selection.select(user);
        this.accService.deleteUser(user.id).pipe(take(1)).subscribe(() => {
          if (!this.deleteModalHasError) {
            this.removeSelectUser(user);
            this.selection = new SelectionModel<User>(true, []);  
          }
        },
          () => {
            this.deleteModalHasError = true;
            setTimeout(() => this.deleteModalHasError = false, 3000);
          });      
      }
    }, () => { });
  }

  private removeSelectUser(user: User) {
    let index: number = this.usersList.findIndex(u => u === user);
    this.usersList.splice(index, 1);
    this.dataSource.data = this.usersList;
  }

  userEmailToChangeRole: string = '';
  currentUserRole: string = '';
  changeUserRoleTo: string = '';
  userIdToChangeRole: string = '';
  roleModalHasError: boolean = false;

  changeRole(changeRoleModal, user: User) {
    this.setChangeRoleVariables(user);
    this.accService.getUserRoles(user.id).pipe(take(1)).subscribe((response: any[]) => {
      for (let item of response)
        if (item === 'admin') {
          this.currentUserRole = item;
          this.changeUserRoleTo = 'user';
        }
    });
    this.modalService.open(changeRoleModal, { ariaLabelledBy: 'modal-change' }).result.then((result) => {
      if (result === true) {
        this.changeUserRoleTo === 'admin' ?
          this.accService.setUserAsAdmin(this.userIdToChangeRole).pipe(take(1)).subscribe() :
          this.accService.unsetUserAsAdmin(this.userIdToChangeRole).pipe(take(1)).subscribe(() => { },
            () => {
              this.roleModalHasError = true;
              setTimeout(() => this.roleModalHasError = false, 3000);
            });
      }
    }, () => { });
  }

  private setChangeRoleVariables(user: User) {
    this.userEmailToChangeRole = user.email;
    this.userIdToChangeRole = user.id;
    this.currentUserRole = 'user';
    this.changeUserRoleTo = 'admin';
    this.roleModalHasError = false;
  }

  loginAsUser(user: User) {
    localStorage.setItem('asUser', user.email);
  }
}
