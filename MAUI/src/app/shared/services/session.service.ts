import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { RouteService } from './route.service';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  constructor(private routerService: RouteService){}

  createSession(){
    let date = new Date();
    let dateBegin = date.getDate();
    let dateEnd = date.getDate()+1;
    //In futuro provvederemo a memorizzare anche la lingua scelta dall'utente
    const paramsSessionMemorize = {
      "dateBegin": dateBegin,
      "dateEnd": dateEnd,
    };
    let stringParamsSession = JSON.stringify(paramsSessionMemorize);
    sessionStorage.setItem(environment.SessionMA, stringParamsSession);
  }

  getSession(){
    return sessionStorage.getItem(environment.SessionMA);
  }

  checkSession():boolean{
    var token = this.getSession();
    var outcome = token == null ? false : true;
    if (outcome == false){
      this.routerService.goAuthComponent();
    }
    return outcome;
  }

}
