import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  hide: boolean = true;

  brandNew: boolean;
  hasError = false;
  submitted: boolean = false;

  constructor(private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {

  }

  ngOnDestroy() {

  }

  login() {
    this.submitted = true;
  }

  userName = new FormControl('', [Validators.required]);
  errorUserName() {
    if (this.userName.hasError('required'))
      return 'You must enter a value';
  }

  password = new FormControl('', [Validators.required]);
  errorPassword() {
    if (this.password.hasError('required'))
      return 'You must enter a value';
  }
}
