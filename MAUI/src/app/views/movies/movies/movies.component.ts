import { Component } from '@angular/core';
import { MovieDto } from '../../../shared/models/movie-dto';
import { MoviesService } from '../movies-services/movies.service';
import { Observable } from 'rxjs';
import { MovieResultRecommendationDto } from '../../../shared/models/movie-result-recommendation-dto';
import { RecommendationsService } from '../recommendations-services/recommendations.service';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrl: './movies.component.scss'
})
export class MoviesComponent {
  MoviesObservable: Observable<MovieDto[]>;
  RecommendationsObservable: Observable<MovieResultRecommendationDto[]>;
  MovieRecomObservable: Observable<MovieDto[]>;
  MoviesData: MovieDto[] = [];
  RecommendationsData: MovieResultRecommendationDto[] = [];
  MoviesRecommendations: MovieDto[] = [];
  query: string | null;
  email: string | null;

  constructor(private movieService: MoviesService,
    private recommendationService: RecommendationsService){}

  ngOnInit(): void{
    this.loadMoviesData();
  }

  loadMoviesData(){
    this.MoviesObservable = this.movieService.getMovies(this.query);
    this.MoviesObservable.subscribe((response: MovieDto[]) => {
      this.MoviesData = response;
    });
  }

  loadRecommendationsData(){
    if(this.email != null){
      this.RecommendationsObservable = this.recommendationService.getRecommendations(this.email);
      this.RecommendationsObservable.subscribe((response: MovieResultRecommendationDto[]) =>{
        this.RecommendationsData = response;
        this.loadMoviesId();
      });
    }
  }

  loadMoviesId(){
    this.RecommendationsData.forEach(recom => {
      this.MovieRecomObservable = this.movieService.getMovies(recom.movieId.toString());
      this.MovieRecomObservable.subscribe((response: MovieDto[]) =>{
        var recomObj = response[0];
        this.MoviesRecommendations.push(recomObj);
      });
    });
  }

  inputSearch(event: any){
    this.query = event.target.value;
    this.loadMoviesData();
  }

  inputEmail(event: any){
    this.email = event.target.value;
    this.loadRecommendationsData();
  }
}
