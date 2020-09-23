import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@aspnet/signalr';
import {environment} from '../../environments/environment';
import {BehaviorSubject} from 'rxjs';
import {UserMessage} from '../models/user-message';
import {sendMessage} from '@aspnet/signalr/dist/esm/Utils';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  constructor() {
  }

  private hub: HubConnection;
  public messages: BehaviorSubject<UserMessage[]> = new BehaviorSubject<UserMessage[]>([]);

  startConnection(): void {
    this.hub = new HubConnectionBuilder()
      .withUrl(environment.hubUrl)
      .build();
    this.hub.start()
      .then(() => console.log('Connection opened'))
      .catch(err => console.log('Error while starting connection: ', err));
    this.onMessageSend();
  }

  onMessageSend(): void {
    this.hub.on('ReceiveMessage', (message: UserMessage) => {
      const messagesArr = this.messages.value;
      messagesArr.push(message);
      this.messages.next(messagesArr);
    });
  }

  sendMessage(userMessage: UserMessage): void {
    this.hub.invoke('SendMessage', userMessage);
  }

}
