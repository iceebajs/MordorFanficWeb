import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';

import { MessageService } from './message.service';

export type HandleError =
  <T> (operation?: string, result?: T) => (error: HttpErrorResponse) => Observable<T>;

@Injectable()
export class HttpErrorHandler {
  constructor(private messageService: MessageService) { }
  errorMessage: any;

  createHandleError = (serviceName = '') => <T>
    (operation = 'operation', result = {} as T) => this.handleError(serviceName, operation, result);

  handleError<T> (serviceName = '', operation = 'operation', result = {} as T) {

    return (error: HttpErrorResponse): Observable<T> => {

      const message = (error.error instanceof ErrorEvent) ?
        error.error.message :
       `server returned code ${error.status}"`;
      this.messageService.add(`${serviceName}: ${operation} failed: ${message}`);

      if (error.status === 200)
        return of(result);
      else {
        this.errorMessage = error.error;
        console.error(error);
        return throwError(message);
      }
    };

  }

  getError() {
    return this.errorMessage;
  }
}
