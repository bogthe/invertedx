import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Website } from '../models/website';
import { WebsiteService } from '../website.service';

@Component({
  selector: 'app-website-detail',
  templateUrl: './website-detail.component.html',
  styleUrls: ['./website-detail.component.css']
})
export class WebsiteDetailComponent implements OnInit {
  website: Website;

  constructor(private route: ActivatedRoute, private webService:WebsiteService) { }

  ngOnInit() {
    this.route.params.subscribe(
      params => this.website = this.webService.getWebsiteById(params["id"])
    );
  }
}
