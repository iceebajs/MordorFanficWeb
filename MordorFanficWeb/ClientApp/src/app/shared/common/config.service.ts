import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {
  _apiURI: string;
  _appURL: string;
  _socketURL: string;

  constructor() {
    this._apiURI = 'api';
    this._appURL = 'https://localhost:44396/'
    this._socketURL = 'wss://localhost:44396/ws';
  }

  getApiURI() {
    return this._apiURI;
  }

  getAppURL() {
    return this._appURL;
  }

  getSocketURL() {
    return this._socketURL;
  }
}
