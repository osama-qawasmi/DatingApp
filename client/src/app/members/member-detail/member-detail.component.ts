import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryModule, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'app/_models/member';
import { find, take } from 'rxjs';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { CommonModule } from '@angular/common';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { MessageService } from 'app/_services/message.service';
import { Message } from 'app/_models/message';
import { PresenceService } from 'app/_services/presence.service';
import { AccountService } from 'app/_services/account.service';
import { User } from 'app/_models/User';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports: [CommonModule, TabsModule, NgxGalleryModule, TimeagoModule, MemberMessagesComponent]
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;
  member: Member = {} as Member;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];
  isMemberOnline: boolean = false;
  user?: User;

  constructor(private accountService: AccountService, private route: ActivatedRoute,
    private messageService: MessageService, public presenceService: PresenceService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: user => {
          if(user) this.user = user;
        }
      })
  }

  ngOnInit(): void {

    this.route.data.subscribe({
      next: data => this.member = data['member']
    })

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })

    this.getImages();

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]

    this.setMmemberStatus();
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  setMmemberStatus() {
    //this.isMemberOnline = (presenceService.onlineUsers$)?.includes(this.member.userName);
    //find((items: string[]) => items.includes(this.member.userName))
    this.presenceService.onlineUsers$.pipe().subscribe(
      {
        next: items => {
          if (items.includes(this.member.userName)) {
            this.isMemberOnline = true;
            console.log("isMemberOnline = " + this.isMemberOnline);
            console.log(this.member.userName + " is Online now");
          }
          else {
            this.isMemberOnline = false;
            console.log("isMemberOnline = " + this.isMemberOnline);
            console.log(this.member.userName + " is offline now");
          }

        }
      });
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      this.memberTabs.tabs.find(x => x.heading === heading)!.active = true;
    }
  }

  onTabActivated(tab: TabDirective) {
    this.activeTab = tab;
    if (this.activeTab.heading === 'Messages' && this.user) {
      this.messageService.createHubConnection(this.user, this.member.userName);
    }
    else {
      this.messageService.stopHubConnection();
    }
  }

  loadMessages() {
    if (this.member) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: response => this.messages = response
      })
    }
  }

  getImages() {
    if (!this.member) return [];
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url
      });
    }

    this.galleryImages = imageUrls;
    return imageUrls;
  }

}
