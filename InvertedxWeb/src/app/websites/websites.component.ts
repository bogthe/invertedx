import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-websites',
  templateUrl: './websites.component.html',
  styleUrls: ['./websites.component.css']
})
export class WebsitesComponent implements OnInit {

  constructor(private router:Router) { }

  ngOnInit() {
  }

  navigateTo() {
    this.router.navigate(['/websites', 'loveangular']);
  }

}
