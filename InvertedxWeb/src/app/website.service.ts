import { Injectable } from '@angular/core';
import { Website } from './models/website';

@Injectable()
export class WebsiteService {
  web: Website = {
    id: "test",
    url: "test-url",
    processed: false
  }

  websites: Array<Website>;

  constructor() { this.websites = [this.web] }

  getWebsites(): Array<Website> {
    return this.websites;
  }

  getWebsiteById(id: string): Website {
    return this.web;
  }

  addWebsite(url:string) {
    this.websites.push({
      id: "new-website",
      url:url,
      processed:false
    });
  }
}
