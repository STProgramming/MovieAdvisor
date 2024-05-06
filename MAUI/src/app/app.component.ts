import { Component } from '@angular/core';
import { SessionService } from './shared/services/session.service';
import { NavigationStart, Router } from '@angular/router';
import { Subscription } from 'rxjs';

export let browserRefresh = false;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {  
  title = 'CMFE';
  subscription: Subscription;

  constructor(private sessionService: SessionService,
    private router: Router){
      this.subscription = router.events.subscribe((event) => {
        if (event instanceof NavigationStart) {
          browserRefresh = !router.navigated;
          if(browserRefresh){
            this.sessionService.removeSession();
          }
        }
    });
    }
  
  ngOnInit(){}


  ngOnDestroy(){
    this.subscription.unsubscribe();
  }
}
