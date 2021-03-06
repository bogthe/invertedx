import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WebsiteDetailComponent } from './website-detail.component';

describe('WebsiteDetailComponent', () => {
  let component: WebsiteDetailComponent;
  let fixture: ComponentFixture<WebsiteDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WebsiteDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WebsiteDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
