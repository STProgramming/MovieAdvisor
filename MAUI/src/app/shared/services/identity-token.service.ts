import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class IdentityTokenService {

  constructor() { }

  setMAIdentityToken(value: string){
    localStorage.setItem(environment.nameIdentityToken, value);
  }

  removeMAIdentityToken(){
    localStorage.removeItem(environment.nameIdentityToken);
  }
}
