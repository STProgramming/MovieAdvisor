import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecommendationsViewerComponent } from './recommendations-viewer.component';

describe('RecommendationsViewerComponent', () => {
  let component: RecommendationsViewerComponent;
  let fixture: ComponentFixture<RecommendationsViewerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RecommendationsViewerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RecommendationsViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
