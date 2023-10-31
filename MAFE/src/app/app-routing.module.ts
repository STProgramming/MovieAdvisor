import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MovieSubmitComponent } from './pages/movie/movie-submit/movie-submit.component';
import { IndexComponent } from './pages/index/index.component';

const routes: Routes = [
  { path: 'index', component: IndexComponent },
  { path: '', redirectTo: '/index', pathMatch: 'full'},
  { path: 'movie-submit', component: MovieSubmitComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
