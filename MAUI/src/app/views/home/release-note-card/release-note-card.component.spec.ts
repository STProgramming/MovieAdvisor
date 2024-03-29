import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReleaseNoteCardComponent } from './release-note-card.component';

describe('ReleaseNoteCardComponent', () => {
  let component: ReleaseNoteCardComponent;
  let fixture: ComponentFixture<ReleaseNoteCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ReleaseNoteCardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ReleaseNoteCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
