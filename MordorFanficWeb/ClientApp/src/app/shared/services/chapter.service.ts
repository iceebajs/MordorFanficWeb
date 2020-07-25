import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HandleError, HttpErrorHandler } from './../helpers/http-error-hander.service';
import { ConfigService } from '../common/config.service';
import { Chapter } from '../interfaces/chapter/chapter.interface';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Like } from '../interfaces/chapter/like.interface';
import { ChapterNumeration } from '../interfaces/chapter/chapter-numeration.interface';
import { UploadImage } from '../interfaces/chapter/upload-image.interface';

@Injectable()
export class ChapterService {
  baseURL: string = '';
  handleError: HandleError;

  constructor(private httpClient: HttpClient, private configService: ConfigService, private httpErrorHandler: HttpErrorHandler) {
    this.baseURL = this.configService.getApiURI();
    this.handleError = this.httpErrorHandler.createHandleError('ChapterService');
  }


  createChapter(chapter: Chapter) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<Chapter>(`${this.baseURL}/chapter`, chapter, { headers: headers })
      .pipe(catchError(this.handleError<Chapter>('createChapter', chapter)));
  }

  updateChapter(chapter: Chapter) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<Chapter>(`${this.baseURL}/chapter/update`, chapter, { headers: headers })
      .pipe(catchError(this.handleError<Chapter>('updateChapter', chapter)));
  }

  updateNumeration(numeration: ChapterNumeration[]) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<ChapterNumeration[]>(`${this.baseURL}/chapter/update-numeration`, numeration, { headers: headers })
      .pipe(catchError(this.handleError<ChapterNumeration[]>('updateNumeration', numeration)));
  }

  deleteChapter(id: number) {
    const headers = this.setUserHeaders();
    return this.httpClient.delete(`${this.baseURL}/chapter/${id}`, { headers: headers })
      .pipe(catchError(this.handleError('deleteChapter')));
  }

  addLike(like: Like) {
    const headers = this.setUserHeaders();
    return this.httpClient.post<Like>(`${this.baseURL}/chapter/add-like`, like, { headers: headers })
      .pipe(catchError(this.handleError<Like>('addLike', like)));
  }

  uploadImage(file: File): Observable<UploadImage> {
    const headers = this.setUserHeaders();

    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.httpClient.post<UploadImage>(`${this.baseURL}/chapter/upload-image`, formData, { headers: headers })
      .pipe(catchError(this.handleError<UploadImage>('uploadImage')));
  }

  deleteImage(image: UploadImage) {
    const headers = this.setUserHeaders();
    return this.httpClient.post(`${this.baseURL}/chapter/delete-image`, image, { headers: headers })
      .pipe(catchError(this.handleError<File>('deleteImage')));
  }

  getErrorMessage() {
    return this.httpErrorHandler.getError();
  }

  setUserHeaders() {
    let authToken = localStorage.getItem('auth_token');
    return new HttpHeaders({ 'Authorization': `Bearer ${authToken}` });
  }
}
