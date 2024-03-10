import { CanActivateFn } from '@angular/router';
import { environment } from '../../../environments/environment';
import { AuthenticationService } from '../services/authentication.service';
import { ɵɵinject } from '@angular/core';
import { IdentityTokenService } from '../services/identity-token.service';

export const authenticationGuard: CanActivateFn = (route, state) => {
  var tokenService = ɵɵinject(IdentityTokenService);
  var authService = ɵɵinject(AuthenticationService);
  var token = tokenService.decodeIdentityToken();
  if(token !== null){
    try {
      var date = new Date();
      var iat = new Date(token.iat);
      var exp = new Date(token.exp);
      if(iat <= date && date < exp){
        var aud = token.aud;
        var iss = token.iss;
        if (aud == environment.JwtAudience && iss == environment.JwtIssuer){
          authService.isAuthenticated.next(true);
          return true;
        }      
        else{
          authService.isAuthenticated.next(false);
          return false;
        }
      }
      else{
        authService.isAuthenticated.next(false);
        return false;
      }
    }
    catch(Error){
      authService.isAuthenticated.next(false);
      return false;
    }
  }
  else{
    authService.isAuthenticated.next(false);
    return false;
  }
};
