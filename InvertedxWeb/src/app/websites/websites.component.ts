import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Website } from '../models/website';
import { WebsiteService } from '../website.service';

@Component({
  selector: 'app-websites',
  templateUrl: './websites.component.html',
  styleUrls: ['./websites.component.css']
})
export class WebsitesComponent implements OnInit {
  isAddingWebsite: boolean;
  websites: Array<Website>;
  url: string;

  constructor(private router: Router, private webService: WebsiteService) { }

  ngOnInit() {
    this.websites = this.webService.getWebsites();
  }

  navigateTo(id: string) {
    this.router.navigate(['/websites', id]);
  }

  toggleWebModal() {
    this.isAddingWebsite = !this.isAddingWebsite;
  }

  saveWebsite() {
    this.webService.addWebsite(this.url);
    this.toggleWebModal();
  }
}
