import { TestBed } from '@angular/core/testing';

import { IdentityTokenService } from './identity-token.service';

describe('IdentityTokenService', () => {
  let service: IdentityTokenService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IdentityTokenService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
