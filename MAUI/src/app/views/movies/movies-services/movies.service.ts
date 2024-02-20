import { Injectable } from '@angular/core';
import { RestApiService } from '../../../shared/services/rest-api.service';
import { Observable, map } from 'rxjs';
import { TagDto } from '../../../shared/models/tag-dto';
import { MovieDto } from '../../../shared/models/movie-dto';

@Injectable({
  providedIn: 'root'
})
export class MoviesService {
  
  constructor(private apiService: RestApiService) { }

  getTagsMovie(): Observable<TagDto[]>{
    return this.apiService.get('Tag')
      .pipe(map((response : any) => {
        if(response){
          return response;
        }        
      }));
  }

  getMovies(query: string | null): Observable<MovieDto[]>{
    return this.apiService.get('Movie?Search='+query)
      .pipe(map((response : any) => {
        if(response){
          return response;
        }        
      }));
  }

  postMovie(newMovie: MovieDto): Observable<any>{
    return this.apiService.post('Movie', newMovie)
      .pipe(map((response : any) => {
        if(response){
          return response;
        }        
      }));
  }
}
