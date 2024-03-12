import { HttpInterceptorFn } from '@angular/common/http';
import { ɵɵinject } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { IdentityTokenService } from '../services/identity-token.service';

export const httpInterceptor: HttpInterceptorFn = (req, next) => {
  var authService = ɵɵinject(AuthenticationService);
  var tokenService = ɵɵinject(IdentityTokenService);
  authService.authenticated.subscribe(data => {
    if(data){
      req = req.clone({
        setHeaders: { Authorization: `Bearer ${tokenService.readIdentityToken()}` }
    });
    }
  })
  return next(req); 
}
