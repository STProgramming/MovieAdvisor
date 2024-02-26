import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewAccordionComponent } from './review-accordion.component';

describe('ReviewAccordionComponent', () => {
  let component: ReviewAccordionComponent;
  let fixture: ComponentFixture<ReviewAccordionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ReviewAccordionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ReviewAccordionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
