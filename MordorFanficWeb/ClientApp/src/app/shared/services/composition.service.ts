import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HandleError, HttpErrorHandler } from './../helpers/http-error-hander.service';
import { ConfigService } from '../common/config.service';
import { Composition } from '../interfaces/composition/composition.interface';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class CompositionService {
  baseURL: string = '';
  handleError: HandleError;

  constructor(private httpClient: HttpClient, private configService: ConfigService, private httpErrorHandler: HttpErrorHandler) {
    this.baseURL = this.configService.getApiURI();
    this.handleError = this.httpErrorHandler.createHandleError('CompositionService');
  }

  getCompositions() : Observable<Composition[]> {
    return this.httpClient.get<Composition[]>(`${this.baseURL}/composition`)
      .pipe(catchError(this.handleError<Composition[]>('getCompositions')));
  }

  getCompositionById(id: number): Observable<Composition> {
    return this.httpClient.get<Composition>(`${this.baseURL}/composition/${id}`)
      .pipe(catchError(this.handleError<Composition>('getCompositionById')));
  }

  getAccountCompositions(id: number): Observable<Composition[]> {
    return this.httpClient.get<Composition[]>(`${this.baseURL}/composition/get-account-compositions/${id}`)
      .pipe(catchError(this.handleError<Composition[]>('getAccountCompositions')));
  }

  createComposition(composition: Composition) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<Composition>(`${this.baseURL}/composition`, composition, { headers: headers })
      .pipe(catchError(this.handleError<Composition>('createComposition', composition)));
  }

  updateComposition(composition: Composition) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<Composition>(`${this.baseURL}/composition/update`, composition, { headers: headers })
      .pipe(catchError(this.handleError<Composition>('updateComposition', composition)));
  }

  deleteComposition(id: number) {
    const headers = this.setUserHeaders();
    return this.httpClient.delete(`${this.baseURL}/composition/${id}`, { headers: headers })
      .pipe(catchError(this.handleError('deleteComposition')));
  }

  getErrorMessage() {
    return this.httpErrorHandler.getError();
  }

  setUserHeaders() {
    let authToken = localStorage.getItem('auth_token');
    return new HttpHeaders({ 'Authorization': `Bearer ${authToken}` });
  }
}
