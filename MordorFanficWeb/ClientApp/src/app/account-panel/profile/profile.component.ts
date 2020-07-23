import { Component, OnInit, ViewChild } from '@angular/core';
import { AccountService } from './../../shared/services/account.service';
import { User } from './../../shared/interfaces/user.interface';
import { take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { CompositionService } from './../../shared/services/composition.service';
import { Composition } from '../../shared/interfaces/composition/composition.interface';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import { MatSort, Sort } from '@angular/material/sort';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  providers: [AccountService, CompositionService]
})
export class ProfileComponent implements OnInit {

  constructor(private accountService: AccountService, private compositionService: CompositionService, private router: Router) { }

  currentUser: User;
  accountId: number;
  userDataLoaded: Promise<boolean>;
  accountDataLoaded: Promise<boolean>;
  accountCompositions: Composition[];

  ngOnInit(): void {
    this.getUserData();
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  displayedColumns: string[] = ['title', 'genre', 'read', 'manage', 'delete'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  dataSource = new MatTableDataSource<Composition>();
  selection = new SelectionModel<Composition>(true, []);

  getUserData() {
    this.accountService.getUserById(localStorage.getItem('id'))
      .pipe(take(1))
      .subscribe((response: User) => {
        this.currentUser = response;
        this.userDataLoaded = Promise.resolve(true);
        this.getAccountData();
      },
        error => console.log(error));
  }

  goTo(route) {
    this.router.navigate([route]);
  }

  getAccountData() {
    this.accountService.getUserAccountId(this.currentUser.id)
      .pipe(take(1))
      .subscribe(response => {
        this.accountId = response;
        this.getAccountCompositions();
      }, error => console.log(error));
  }

  getAccountCompositions() {
    this.compositionService.getAccountCompositions(this.accountId)
      .pipe(take(1))
      .subscribe((response: Composition[]) => {
        this.dataSource.data = response;
        this.accountCompositions = response;
        this.dataSource.sort = this.sort;
        this.accountDataLoaded = Promise.resolve(true);
      }, error => console.log(error));
  }

  openComposition(composition: Composition) {
    console.log(composition.compositionId);
  }

  manageComposition(composition: Composition) {
    this.router.navigate(['account/manage-composition'],
      { queryParams: { uId: composition.userId, id: composition.compositionId } });
  }

  deleteSuccessfull: boolean = false;
  deleteComposition(composition: Composition) {
    this.compositionService.deleteComposition(composition.compositionId)
      .pipe(take(1))
      .subscribe(() => {
        this.removeSelectedComposition(composition);
        this.selection = new SelectionModel<Composition>(true, []);
        this.deleteSuccessfull = true;
        setTimeout(() => this.deleteSuccessfull = false, 3000);
      });
  }

  private removeSelectedComposition(composition: Composition) {
    let index: number = this.accountCompositions.findIndex(c => c === composition);
    this.accountCompositions.splice(index, 1);
    this.dataSource.data = this.accountCompositions;
  }

  sortedData: Composition[];
  sortData(sort: Sort) {
    const data = this.accountCompositions.slice();
    if (!sort.active) {
      this.sortedData = data;
      return;
    }

    this.sortedData = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'title': return compare(a.title, b.title, isAsc);
        case 'genre': return compare(a.genre, b.genre, isAsc);
        default: return 0;
      }
    });

    function compare(a: number | string, b: number | string, isAsc: boolean) {
      return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
    }
    this.paginator.firstPage();
    this.dataSource.data = this.sortedData;
  }
}


