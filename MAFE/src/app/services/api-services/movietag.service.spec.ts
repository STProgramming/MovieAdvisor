import { TestBed } from '@angular/core/testing';

import { MovietagService } from './movietag.service';

describe('MovietagService', () => {
  let service: MovietagService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MovietagService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
