import { Injectable, signal } from '@angular/core';
import { IdentityTokenService } from './identity-token.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  public isAuthenticated = new BehaviorSubject<boolean>(false);

  constructor(private tokenService: IdentityTokenService) {
    this.AuthenticationCheck();
  }

  private AuthenticationCheck(){
    this.isAuthenticated.subscribe(data => {
      if(data){
        this.CheckIdentityAuthentication();
      }
      else{
        this.AuthenticationFail();
      }
    });
  }

  AuthenticationSuccess(token: string){
    this.tokenService.setMAIdentityToken(token);
  }

  AuthenticationFail(){
    this.tokenService.removeMAIdentityToken();
  }

  CheckIdentityAuthentication(){
    return this.tokenService.readIdentityToken();
  }
}
