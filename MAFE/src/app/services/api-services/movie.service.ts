import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, take } from 'rxjs';
import { IMovieModel } from 'src/app/interfaces/IMovieDTO';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private endPoint: string = 'Movie/';

  constructor(private readonly httpClient: HttpClient) { }

  GetAllMovies(): Observable<any>{
    var action = 'GetAllMovies';
    return this.httpClient.get(environment._apiServer+this.endPoint+action).pipe(
      take(1),
      catchError((error) => {
        throw error;
      })
    );
  }

  PostNewMovie(newMovie: IMovieModel): Observable<any>{
    var action = 'PostNewMovie';
    return this.httpClient.post(environment._apiServer+this.endPoint+action+'?emailUser='+environment._emailAdmin, newMovie).pipe(      
      catchError((error) =>{
        throw error;
      })
    );
  }
}
