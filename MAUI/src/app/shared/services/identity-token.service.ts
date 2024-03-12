import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class IdentityTokenService {

  setMAIdentityToken(value: string){
    localStorage.setItem(environment.nameIdentityToken, value);
  }

  removeMAIdentityToken(){
    localStorage.removeItem(environment.nameIdentityToken);
  }

  decodeIdentityToken(){
    var token = localStorage.getItem(environment.nameIdentityToken);
    try{
      return jwtDecode(token);
    }
    catch(Error){
      return null;     
    }
  }

  readIdentityToken(){
    return localStorage.getItem(environment.nameIdentityToken);
  }

  getUserName(){
    var token = this.decodeIdentityToken();
    return token.unique_name;
  }

  getUserRole():string{
    var token = this.decodeIdentityToken();
    return token.role;
  }
}
