import { CanActivateFn } from '@angular/router';
import { SessionService } from '../services/session.service';
import { ɵɵinject } from '@angular/core';

export const sessionGuard: CanActivateFn = (route, state) => {
  var sessionService = ɵɵinject(SessionService)
  return sessionService.checkSession();
};
