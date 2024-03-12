import { CanActivateFn } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { ɵɵinject } from '@angular/core';

export const authenticationGuard: CanActivateFn = (route, state) => {
  var authService = ɵɵinject(AuthenticationService);
  return authService.triggerAuthGuard(false);
};
