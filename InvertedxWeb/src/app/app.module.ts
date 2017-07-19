import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { IndvertedxComponent } from './indvertedx/indvertedx.component';
import { WebsitesComponent } from './websites/websites.component';
import { WebsiteDetailComponent } from './website-detail/website-detail.component';

import { WebsiteService } from './website.service';

const routes: Routes = [
  { path: 'invertedx', component: IndvertedxComponent },
  { path: 'websites', component: WebsitesComponent },
  { path: 'websites/:id', component: WebsiteDetailComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    IndvertedxComponent,
    WebsitesComponent,
    WebsiteDetailComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    FormsModule
  ],
  providers: [WebsiteService],
  bootstrap: [AppComponent]
})
export class AppModule { }
