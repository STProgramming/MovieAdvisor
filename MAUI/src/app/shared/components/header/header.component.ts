import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../../services/authentication.service';
import { RoleService } from '../../services/role.service';
import { RouteService } from '../../services/route.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
authFlag: Observable<boolean> = this.authService.authenticated;
adminFlag: Observable<boolean> = this.roleService.isAdmin;

constructor(private authService: AuthenticationService,
  private roleService: RoleService,
  private routeService: RouteService,
  private toastr: ToastrService){}

  goHome(){
    this.routeService.goHome();
  }

  goMovies(){
    this.routeService.goMovies();
  }

  goNewMovie(){
    this.routeService.goNewMovie();
  }

  goReviews(){
    this.routeService.goReviews();
  }

  goNewReview(){
    this.routeService.goNewReview();
  }

  logOutUser(){
    this.authService.logOutUser();
    this.toastr.info('Alla prossima!', 'Logout effettuata');
  }

  goLogin(){
    this.routeService.goLogin();
  }

  goNewUser(){
    this.routeService.goNewUser();
  }

}
