import {Component, OnInit} from '@angular/core';
import {SignalRService} from './services/signal-r.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {UserMessage} from './models/user-message';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private signalRService: SignalRService) {
    // this.userMessages = signalRService.userMessages;
  }

  title = 'chat-client';
  messageForm: FormGroup;

  sendMessage(): void {
    const userMessage: UserMessage = {
      userName: this.messageForm.get('name').value,
      text: this.messageForm.get('message').value
    };
    console.log(userMessage);
    this.signalRService.sendMessage(userMessage);
  }

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.messageForm = new FormGroup({
      name: new FormControl('', [Validators.minLength(1), Validators.maxLength(20)]),
      message: new FormControl('', [Validators.minLength(1), Validators.maxLength(100)])
    });
  }
}
