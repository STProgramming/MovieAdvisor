import { Component } from '@angular/core';
import { IdentityService } from '../identity-services/identity.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginObservable: Observable<any>;

  constructor(private identityServices: IdentityService){}

  ngOnInit(): void{}

  loginWithGoogle(){
    this.loginObservable = this.identityServices.loginWithGoogle()
    this.loginObservable.subscribe((response: any) => {
      console.log(response);
    });
  }
}
