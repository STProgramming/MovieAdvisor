import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { SessionService } from '../../services/session.service';
import { Router } from '@angular/router';
import { RoleService } from '../../services/role.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss'
})
export class AuthComponent implements OnInit, OnDestroy {
  userName: string = null;
  loading: boolean = true;
  intervalId: any;
  pollingStarted: boolean = false;

  constructor(private authService: AuthenticationService,
    private sessionService: SessionService,
    private roleService: RoleService,
    private router: Router){}
  
  ngOnInit(){
    if(!this.pollingStarted){
      this.pollingStarted = true;
      this.userName = this.authService.triggerAuthComponent();
        if(this.userName !==null){
          this.roleService.isAppAdmin(false);
        }
        if(!this.sessionService.checkSession()){
          this.sessionService.createSession();
        }
      this.intervalId = setInterval(() => {
        this.loading = false;        
        this.router.navigateByUrl('home');
        clearInterval(this.intervalId);
      }, 3000);
    }
  }

  ngOnDestroy(){
    clearInterval(this.intervalId);
  }
}
