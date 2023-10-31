import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovieSubmitComponent } from './movie-submit.component';

describe('MovieSubmitComponent', () => {
  let component: MovieSubmitComponent;
  let fixture: ComponentFixture<MovieSubmitComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MovieSubmitComponent]
    });
    fixture = TestBed.createComponent(MovieSubmitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
