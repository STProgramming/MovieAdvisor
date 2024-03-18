import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewMovieComponent } from './views/movies/new-movie/new-movie.component';
import { MoviesComponent } from './views/movies/movies/movies.component';
import { ReviewsComponent } from './views/reviews/reviews/reviews.component';
import { NewReviewComponent } from './views/reviews/new-review/new-review.component';
import { LoginComponent } from './views/identity/authentication/login/login.component';
import { authenticationGuard } from './shared/guards/authentication.guard';
import { HomeComponent } from './views/home/home.component';
import { roleGuard } from './shared/guards/role.guard';
import { NewUserComponent } from './views/identity/user/new-user/new-user.component';
import { UserDetailsComponent } from './views/identity/user/user-details/user-details.component';
import { AuthComponent } from './shared/components/auth/auth.component';
import { sessionGuard } from './shared/guards/session.guard';
import { RecommendationsComponent } from './views/recommendations/recommendations/recommendations.component';

const routes: Routes = [
  {path: '', redirectTo: 'auth', pathMatch: 'full'},
  {path: 'auth', component: AuthComponent, pathMatch: 'full'},
  {path: 'home', component: HomeComponent, pathMatch: 'full', canActivate: [sessionGuard]},
  {path: 'movies', component: MoviesComponent, pathMatch: 'full', canActivate: [sessionGuard]},
  {path: 'movies/new-movie', component: NewMovieComponent, canActivate: [authenticationGuard, roleGuard, sessionGuard], pathMatch: 'full'},
  {path: 'reviews', component: ReviewsComponent, pathMatch: 'full', canActivate: [sessionGuard]},
  {path: 'reviews/new-review', component: NewReviewComponent, canActivate: [authenticationGuard, sessionGuard], pathMatch: 'full'},
  {path: 'identity/authentication', component: LoginComponent, canActivate: [sessionGuard], pathMatch: 'full'},
  {path: 'identity/new-user', component: NewUserComponent, canActivate: [sessionGuard], pathMatch: 'full'},
  {path: 'identity/user', component: UserDetailsComponent, canActivate: [authenticationGuard, sessionGuard], pathMatch: 'full'},
  {path: 'ai/recommendations', component: RecommendationsComponent, canActivate: [authenticationGuard, sessionGuard], pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
