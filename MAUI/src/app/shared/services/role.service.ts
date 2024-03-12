import { Injectable } from '@angular/core';
import { IdentityTokenService } from './identity-token.service';
import { environment } from '../../../environments/environment';
import { RouteService } from './route.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  public readonly isAdmin = new BehaviorSubject<boolean>(false);

  constructor(private readonly tokenService: IdentityTokenService,
    private readonly routeService: RouteService) { }

  isAppAdmin(redirect: boolean):boolean{
    var admin = environment.IdentityRoles[1];
    var outcome = this.getRoleFromUser() === admin ? true : false;
    this.isAdmin.next(outcome);
    if(redirect == false){
      if(outcome == false) this.routeService.notAuthorizedRouteService()
    }
    return outcome;
  }

  getRoleFromUser(): string{
    return this.tokenService.getUserRole();
  }
}
