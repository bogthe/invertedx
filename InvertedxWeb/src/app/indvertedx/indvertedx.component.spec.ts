import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IndvertedxComponent } from './indvertedx.component';

describe('IndvertedxComponent', () => {
  let component: IndvertedxComponent;
  let fixture: ComponentFixture<IndvertedxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IndvertedxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IndvertedxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
