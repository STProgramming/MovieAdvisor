import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewMovieComponent } from './views/movies/new-movie/new-movie.component';
import { MoviesComponent } from './views/movies/movies/movies.component';
import { ReviewsComponent } from './views/reviews/reviews/reviews.component';
import { NewReviewComponent } from './views/reviews/new-review/new-review.component';
import { LoginComponent } from './views/identity/login/login.component';

const routes: Routes = [
  {path: 'movie', component: MoviesComponent},
  {path: 'movie/new-movie', component: NewMovieComponent},
  {path: 'review', component: ReviewsComponent},
  {path: 'review/new-review', component: NewReviewComponent},
  {path: 'identity/login', component: LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
