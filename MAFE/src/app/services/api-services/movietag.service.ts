import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, take } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MovietagService {
  private endPoint: string = 'MovieTag/';

  constructor(private readonly httpClient: HttpClient) { }

  GetAllMovieTags(): Observable<any>{
    var action = 'GetAllMovieTags'
    return this.httpClient.get(environment._apiServer+this.endPoint+action).pipe(
      take(1),
      catchError((error) => {
        alert(error);
        throw error;
      })
    );
  }
}
