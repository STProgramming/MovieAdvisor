import { Injectable, signal } from '@angular/core';
import { IdentityTokenService } from './identity-token.service';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { RouteService } from './route.service';
import { RoleService } from './role.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  public readonly authenticated = new BehaviorSubject<boolean>(false);

  constructor(private readonly tokenService: IdentityTokenService,
    private readonly routeService: RouteService,
    private readonly roleService: RoleService){}

  private isAuthenticated(redirectFail: boolean):boolean{
    if(this.isIdentityTokenExist()){
      var token = this.tokenService.decodeIdentityToken();
      if(this.checkExpirationToken(token)){
        if(this.checkAntiForgeryToken(token)){
          return this.validAuthenticationToken();
        }
        else{
          return this.authFailed(true, redirectFail);
        }
      }
      else{
        return this.authFailed(true, redirectFail);
      }
    }
    else{
      return this.authFailed(false, redirectFail);
    }
  }

  public authSuccessfull(valueToken: string){
    this.tokenService.setMAIdentityToken(valueToken);
    this.validAuthenticationToken();
    this.routeService.loginRouteService(true);
    return this.tokenService.getUserName();
  }
  
  private validAuthenticationToken(): boolean{    
    this.authenticated.next(true);
    return true;
  }

  public authFail(){
    this.authFailed(false, true);
  }
  
  private authFailed(remove: boolean, redirectFail: boolean): boolean{
    if(remove){
      this.tokenService.removeMAIdentityToken();
    }
    this.authenticated.next(false);
    this.roleService.isAdmin.next(false);
    if(redirectFail == true){
      this.routeService.loginRouteService(false);
    }
    return false;
  }

  private checkExpirationToken(token: any): boolean{
    var date = Date.now() / 1000 | 0;
    if(token.iat <= date && date < token.exp){
      return true;
    }
    else{
      return false;
    }
  }

  private checkAntiForgeryToken(token: any):boolean{
    if (token.aud == environment.JwtAudience && token.iss == environment.JwtIssuer){
      return true;
    }
    return false;
  }

  private isIdentityTokenExist():boolean{
    if(this.tokenService.readIdentityToken() == null){
      return false
    }
    else{
      return true;
    }
  }

  public logOutUser(){
    if(this.isAuthenticated(false)){
      this.authFailed(true, false);
    }
    else{
      this.routeService.goLogin();
    }
  }

  triggerAuthGuard(redirect: boolean):boolean{
    var outcome = this.isAuthenticated(true);
    if(redirect){
      this.routeService.loginRouteService(outcome);
    }
    return outcome;
  }

  triggerAuthComponent(): string | null{
    var outcome = this.isAuthenticated(false);
    if (outcome == true){
      return this.tokenService.getUserName();
    }
    return null;
  }
}
