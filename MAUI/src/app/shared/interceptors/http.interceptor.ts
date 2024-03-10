import { HttpInterceptorFn } from '@angular/common/http';
import { ɵɵinject } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';

export const httpInterceptor: HttpInterceptorFn = (req, next) => {
  var authService = ɵɵinject(AuthenticationService);
  authService.isAuthenticated.subscribe(data => {
    if(data){
      req = req.clone({
        setHeaders: { Authorization: `Bearer ${authService.CheckIdentityAuthentication()}` }
    });
    }
  })
  return next(req); 
}
