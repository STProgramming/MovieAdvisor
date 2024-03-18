import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SessionsViewerComponent } from './sessions-viewer.component';

describe('SessionsViewerComponent', () => {
  let component: SessionsViewerComponent;
  let fixture: ComponentFixture<SessionsViewerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SessionsViewerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SessionsViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
