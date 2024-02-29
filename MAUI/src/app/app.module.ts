import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MoviesComponent } from './views/movies/movies/movies.component';
import { MovieCardComponent } from './views/movies/movie-card/movie-card.component';
import { NewMovieComponent } from './views/movies/new-movie/new-movie.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ReviewsComponent } from './views/reviews/reviews/reviews.component';
import { NewReviewComponent } from './views/reviews/new-review/new-review.component';
import { ReviewAccordionComponent } from './views/reviews/review-accordion/review-accordion.component';
import { LoginComponent } from './views/identity/login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    MoviesComponent,
    MovieCardComponent,
    NewMovieComponent,
    ReviewsComponent,
    NewReviewComponent,
    ReviewAccordionComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatInputModule,
    NgbModule
  ],
  providers: [
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
