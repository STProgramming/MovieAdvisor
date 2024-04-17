import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
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
import { LoginComponent } from './views/identity/authentication/login/login.component';
import { NewUserComponent } from './views/identity/user/new-user/new-user.component';
import { UserDetailsComponent } from './views/identity/user/user-details/user-details.component';
import { HomeComponent } from './views/home/home.component';
import { HeaderComponent } from './shared/components/header/header.component';
import { MovieDetailsModalComponent } from './views/movies/movie-details-modal/movie-details-modal.component';
import { AuthComponent } from './shared/components/auth/auth.component';
import { LoaderComponent } from './shared/components/loader/loader.component';
import { ToastrModule } from 'ngx-toastr';
import { ReviewCardComponent } from './views/reviews/review-card/review-card.component';
import { NewRequestComponent } from './views/recommendations/new-request/new-request.component';
import { RecommendationsComponent } from './views/recommendations/recommendations/recommendations.component';
import { RecommendationsViewerComponent } from './views/recommendations/recommendations-viewer/recommendations-viewer.component';
import { SessionsViewerComponent } from './views/recommendations/sessions-viewer/sessions-viewer.component';
import { FooterComponent } from './shared/components/footer/footer.component';
import { DialogModule } from '@angular/cdk/dialog';
import { ArticleCardComponent } from './views/home/article-card/article-card.component';
import { ReleaseNoteCardComponent } from './views/home/release-note-card/release-note-card.component';
import { PaginatorComponent } from './shared/components/paginator/paginator.component';

@NgModule({
  declarations: [
    AppComponent,
    MoviesComponent,
    MovieCardComponent,
    NewMovieComponent,
    ReviewsComponent,
    NewReviewComponent,
    LoginComponent,
    NewUserComponent,
    UserDetailsComponent,
    HomeComponent,
    HeaderComponent,
    MovieDetailsModalComponent,
    AuthComponent,
    LoaderComponent,
    ReviewCardComponent,
    NewRequestComponent,
    RecommendationsComponent,
    RecommendationsViewerComponent,
    SessionsViewerComponent,
    FooterComponent,
    ArticleCardComponent,
    ReleaseNoteCardComponent,
    PaginatorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgbModule,
    ToastrModule.forRoot(),
    DialogModule
  ],
  providers: [
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
