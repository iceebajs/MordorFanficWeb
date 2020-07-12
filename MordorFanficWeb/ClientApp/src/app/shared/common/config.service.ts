import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {
  _apiURI: string;
  _appURL: string;

  constructor() {
    this._apiURI = 'api';
    this._appURL = 'http://localhost:50183'
  }

  getApiURI() {
    return this._apiURI;
  }

  getAppURL() {
    return this._appURL;
  }
}
