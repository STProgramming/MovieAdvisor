import { Component } from '@angular/core';
import { MovieDto } from '../../../shared/models/movie-dto';
import { MoviesService } from '../movies-services/movies.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrl: './movies.component.scss'
})
export class MoviesComponent {
  MoviesObservable: Observable<any>;
  MoviesData: MovieDto[] = [];
  query: string | null;

  constructor(private movieService: MoviesService){}

  ngOnInit(): void{
    this.loadMoviesData();
  }

  loadMoviesData(){
    this.MoviesObservable = this.movieService.getMovies(this.query);
    this.MoviesObservable.subscribe((response: any) => {
      this.MoviesData = response;
    });
  }

  inputSearch(event: any){
    this.query = event.target.value;
    this.loadMoviesData();
  }
}
