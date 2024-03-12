import { Injectable } from '@angular/core';
import { Route, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RouteService {

  constructor(private router: Router) { }

  loginRouteService(outcome: boolean){
    if(outcome){
      this.goHome();
    }
    else{
      this.goLogin();
    }
  }

  goAuthComponent(){
    this.router.navigateByUrl('/');
  }

  notAuthorizedRouteService(){
    this.goHome();
  }

  goHome(){
    this.router.navigateByUrl('home');
  }

  goLogin(){
    this.router.navigateByUrl('identity/authentication');
  }

  goMovies(){
    this.router.navigateByUrl('movies');
  }

  goNewMovie(){
    this.router.navigateByUrl('movies/new-movie');
  }

  goReviews(){
    this.router.navigateByUrl('reviews');
  }

  goNewReview(){
    this.router.navigateByUrl('reviews/new-review');
  }

  goNewUser(){
    this.router.navigateByUrl('identity/new-user');
  }
}
