import { Component, OnInit } from '@angular/core';
import { Message } from 'app/_models/message';
import { Pagination } from 'app/_models/pagination';
import { MessageService } from 'app/_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages?: Message[];
  pagination?: Pagination;
  container = 'Unread'; //Unread //Inbox //Outbox
  pageNumber = 1;
  pageSize = 5;
  loading = false;

  constructor(private messageService: MessageService) {

  }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.loading = true;
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe({
      next: response => {
        this.messages = response.result;
        this.pagination = response.pagination;
        this.loading = false;
      }
    })
  }

  deleteMessage(id: number) {
    this.messageService.deleteMessage(id).subscribe({
      next: () => this.messages?.splice(this.messages.findIndex(x => x.id === id), 1)
    });
    //this.loadMessages();
  }

  pageChanged(event: any) {
    if(this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }

}
