import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@aspnet/signalr';
import {environment} from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  constructor() {
  }

  private hub: HubConnection;

  public messages: string[];

  startConnection(): void {
    this.hub = new HubConnectionBuilder()
      .withUrl(environment.hubUrl)
      .build();
    this.hub.start()
      .then(() => console.log('Connection opened'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  onMessageSe(): void {
    this.hub.on('messageSend', (message) => {
      this.messages.push(message);
    });
  }
}
