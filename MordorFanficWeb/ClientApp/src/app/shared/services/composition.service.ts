import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HandleError, HttpErrorHandler } from './../helpers/http-error-hander.service';
import { ConfigService } from '../common/config.service';
import { Composition } from '../interfaces/composition/composition.interface';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Rating } from '../interfaces/composition/rating.interface';
import { Tag } from '../interfaces/tags/tag.interface';
import { CompositionTag } from '../interfaces/composition-tags/composition-tag.interface';

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

  createComposition(composition: Composition): Observable<number> {
    const headers = this.setUserHeaders();
    return this.httpClient.post<number>(`${this.baseURL}/composition`, composition, { headers: headers })
      .pipe(catchError(this.handleError<number>('createComposition')));
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

  getAllTags(): Observable<Tag[]> {
    return this.httpClient.get<Tag[]>(`${this.baseURL}/tags`)
      .pipe(catchError(this.handleError<Tag[]>('getAllTags')));
  }

  addTags(tags: Tag[]) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<Tag[]>(`${this.baseURL}/tags/add-tags`, tags, { headers: headers })
      .pipe(catchError(this.handleError<Tag[]>('addTags', tags)));

  }

  addCompositionTags(compTags: CompositionTag[]) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<CompositionTag[]>(`${this.baseURL}/tags`, compTags, { headers: headers })
      .pipe(catchError(this.handleError<CompositionTag[]>('addCompositionTags', compTags)));
  }

  addRating(rating: Rating) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<Rating>(`${this.baseURL}/composition/add-rating`, rating, { headers: headers })
      .pipe(catchError(this.handleError<Rating>('addRating', rating)));
  }

  addComment(comment: Comment) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<Comment>(`${this.baseURL}/composition/add-comment`, comment, { headers: headers })
      .pipe(catchError(this.handleError<Comment>('addComment', comment)));
  }

  getErrorMessage() {
    return this.httpErrorHandler.getError();
  }

  setUserHeaders() {
    let authToken = localStorage.getItem('auth_token');
    return new HttpHeaders({ 'Authorization': `Bearer ${authToken}` });
  }
}
