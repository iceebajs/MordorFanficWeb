import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit, OnDestroy {

  isMenuCollapsed = true;
  isLoggedIn: boolean = false;

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  signout() {
  }

  ngOnDestroy() {
  }

  goTo(route: string): void {
    this.router.navigate([route]);
  }
}
