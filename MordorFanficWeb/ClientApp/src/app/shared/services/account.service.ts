import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { HandleError, HttpErrorHandler } from './../helpers/http-error-hander.service';
import { ConfigService } from '../common/config.service';
import { UpdateProfile } from '../interfaces/update-profile.interface';
import { User } from '../interfaces/user.interface';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ChangePassword } from '../interfaces/change-password.interface';
import { UpdateUserStatus } from '../interfaces/update-user-status.interface'

@Injectable()
export class AccountService {
  baseURL: string = '';
  handleError: HandleError;

  constructor(private httpClient: HttpClient, private httpErrorHandler: HttpErrorHandler, private configService: ConfigService) {
    this.handleError = this.httpErrorHandler.createHandleError('AccountService');
    this.baseURL = this.configService.getApiURI();
  }

  updateProfileInfo(user: UpdateProfile) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<UpdateProfile>(`${this.baseURL}/account/update-user-informtaion`, user, { headers: headers })
      .pipe(catchError(this.handleError('updateProfileInfo', user)));
  }

  getUserByEmail(email: string): Observable<User> {
    const headers = this.setUserHeaders();
    return this.httpClient.get<User>(`${this.baseURL}/account/user/${email}`, { headers: headers })
      .pipe(catchError(this.handleError<User>('getUserByEmail')));
  }

  getUserById(id: string): Observable<User> {
    const headers = this.setUserHeaders();
    return this.httpClient.get<User>(`${this.baseURL}/account/${id}`, { headers: headers })
      .pipe(catchError(this.handleError<User>('getUserById')));
  }

  getUsersList(): Observable<User[]> {
    const headers = this.setUserHeaders();
    return this.httpClient.get<User[]>(`${this.baseURL}/account/get-users-list`, { headers: headers })
      .pipe(catchError(this.handleError<User[]>('getUsersList')));
  }

  changePassword(userData: ChangePassword) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<ChangePassword>(`${this.baseURL}/account/change-password`, userData, { headers: headers })
      .pipe(catchError(this.handleError<ChangePassword>('changePassword', userData)));
  }

  updateUserStatus(user: UpdateUserStatus) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<UpdateUserStatus>(`${this.baseURL}/account/update-user-status`, user, { headers: headers })
      .pipe(catchError(this.handleError<UpdateUserStatus>('updateUserStatus', user)));
  }

  deleteUser(id: string) {
    const headers = this.setUserHeaders();
    return this.httpClient.delete(`${this.baseURL}/account/${id}`, { headers: headers })
      .pipe(catchError(this.handleError('deleteUser')));
  }

  getUserRoles(id: string){
    const headers = this.setUserHeaders();
    return this.httpClient.get(`${this.baseURL}/account/get-user-roles/${id}`, { headers: headers })
      .pipe(catchError(this.handleError('getUserRoles')));
  }

  setUserAsAdmin(id: string) {
    const headers = this.setUserHeaders();
    return this.httpClient.post(`${this.baseURL}/account/set-role/${id}`, { headers: headers })
      .pipe(catchError(this.handleError('setUserAsAdmin')));
  }

  unsetUserAsAdmin(id: string) {
    const headers = this.setUserHeaders();
    return this.httpClient.post(`${this.baseURL}/account/unset-role/${id}`, { headers: headers })
      .pipe(catchError(this.handleError('unsetUserAsAdmin')));
  }

  getErrorMessage() {
    return this.httpErrorHandler.getError();
  }

  setUserHeaders() {
    let authToken = localStorage.getItem('auth_token');
    return new HttpHeaders({ 'Authorization': `Bearer ${authToken}` });
  }
}
