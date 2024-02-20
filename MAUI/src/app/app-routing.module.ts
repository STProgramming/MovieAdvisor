import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewMovieComponent } from './views/movies/new-movie/new-movie.component';
import { MoviesComponent } from './views/movies/movies/movies.component';

const routes: Routes = [
  {path: '/movie', component: MoviesComponent},
  {path: '/movie/new-movie', component: NewMovieComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
