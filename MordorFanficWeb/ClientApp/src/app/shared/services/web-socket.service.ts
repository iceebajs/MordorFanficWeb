import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SocketMessage } from '../interfaces/socket-message.interface';
import { SocketComment } from '../interfaces/composition/socket-commentary.interface';
import { UserCommentary } from '../interfaces/composition/comment.interface';
import { ConfigService } from './../common/config.service';

@Injectable()
export class WebSocketService {
  private socket: WebSocket;
  newCommentary: BehaviorSubject<SocketComment[]> = new BehaviorSubject<SocketComment[]>([]);
  socketURL: string = '';

  constructor(private configService: ConfigService) {
    this.socketURL = this.configService.getSocketURL();
  }

  startSocket() {
    this.socket = new WebSocket(this.socketURL);
    this.socket.addEventListener("open", (() => { }));
    this.socket.addEventListener("message", (ev => {
      var messageBox: SocketMessage = JSON.parse(ev.data);
      this.newCommentary.next(messageBox.Payload);
    }));
  }

  sendAddCommentRequest(comment: UserCommentary) {
    var requestAsJson = JSON.stringify(comment);
    this.socket.send(requestAsJson);
  }
}
