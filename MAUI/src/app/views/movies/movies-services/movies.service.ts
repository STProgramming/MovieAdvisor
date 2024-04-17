import { Injectable } from '@angular/core';
import { RestApiService } from '../../../shared/services/rest-api.service';
import { Observable, map } from 'rxjs';
import { TagDto } from '../../../shared/models/tag-dto';
import { MovieDto } from '../../../shared/models/movie-dto';
import { NewMovieDto } from '../../../shared/models/new-movie-dto';
import { MovieSearchResultsDto } from '../../../shared/models/movie-search-results-dto';

@Injectable({
  providedIn: 'root'
})
export class MoviesService {
  
  constructor(private apiService: RestApiService) { }

  getTagsMovie(): Observable<TagDto[]>{
    return this.apiService.get('Movie/Tag')
      .pipe(map((response : TagDto[]) => {
        if(response){
          return (response);
        }        
        else{
          return null;
        }
      }));
  }

  getMovies(query: string | null, pageSelected: number, elementsViewed: number): Observable<MovieSearchResultsDto>{
    return this.apiService.get('Movie?Search='+query+'&Page='+pageSelected+'&Elements='+elementsViewed)
      .pipe(map((response : any) => {
        if(response){
          return (response);
        }        
        else{
          return null;
        }
      }));
  }

  postMovie(newMovie: NewMovieDto): Observable<any>{
    return this.apiService.post('Movie', newMovie)
      .pipe(map((response : any) => {
        return response;    
      }));
  }

  postMovieImage(Files: FormData, movieId: number): Observable<any>{
    return this.apiService.upload('Movie/Multimedia?MovieId='+movieId, Files)
      .pipe(map((response: any)=>{
        return response;
      }));
  }
 
  getMovieImage(movieId: number, counter: number): Observable<Blob>{
    return this.apiService.downlaod('Movie/Multimedia?MovieId='+movieId+'&Counter='+counter)
      .pipe(map((response: Blob)=>{
        if(response){
          return response;
        }
        else return null;
      }));
  }
}
