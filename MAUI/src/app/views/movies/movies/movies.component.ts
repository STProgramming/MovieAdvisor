import { Component } from '@angular/core';
import { MovieDto } from '../../../shared/models/movie-dto';
import { MoviesService } from '../movies-services/movies.service';
import { Observable } from 'rxjs';
import { MovieSearchResultsDto } from '../../../shared/models/movie-search-results-dto';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrl: './movies.component.scss'
})
export class MoviesComponent {
  MoviesObservable: Observable<MovieSearchResultsDto>;
  MoviesData: MovieSearchResultsDto;
  query: string | null = '';
  isLoading: boolean = false;
  elementsViewedList: number[] = [6, 9, 15, 24];
  elementsViewedSelection: number = this.elementsViewedList[0];
  totalPages: number;
  pageSelected: number = 1;

  constructor(private movieService: MoviesService){}

  ngOnInit(): void{
    this.loadMoviesData();
  }

  private resetMovieSearch(){
    if(this.MoviesData != null){
      this.MoviesData.movies = [];
      this.MoviesData.resultsForDescription = [];
      this.MoviesData.resultsForLifeSpan = [];
      this.MoviesData.resultsForMaker = [];
      this.MoviesData.resultsForTag = [];
      this.MoviesData.resultsForTitle = [];
      this.MoviesData.resultsForYear = [];
    }
  }

  loadMoviesData(){
    this.resetMovieSearch();
    this.isLoading = true;
    this.MoviesObservable = this.movieService.getMovies(this.query, this.pageSelected, this.elementsViewedSelection);
    this.MoviesObservable.subscribe((response: MovieSearchResultsDto) => {
      this.MoviesData = response;
      this.isLoading = false;
      this.calculatePages();
    });
  }

  calculatePages(){
    var result = this.MoviesData.moviesCount / this.elementsViewedSelection;
    this.totalPages = Math.floor(result);
  }

  inputElements(event: any){
    this.elementsViewedSelection = parseInt(event.target.value);
    this.loadMoviesData();
  }

  inputPages(page: number){
    this.pageSelected = page;
    this.loadMoviesData();
  }

  inputSearch(event: any){
    this.query = event.target.value;
    this.loadMoviesData();
  }
}
