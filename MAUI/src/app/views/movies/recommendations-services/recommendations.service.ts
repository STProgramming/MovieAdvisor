import { Injectable } from '@angular/core';
import { RestApiService } from '../../../shared/services/rest-api.service';
import { Observable, map } from 'rxjs';
import { MovieResultRecommendationDto } from '../../../shared/models/movie-result-recommendation-dto';

@Injectable({
  providedIn: 'root'
})
export class RecommendationsService {

  constructor(private apiService: RestApiService) { }

  getRecommendations(email: string): Observable<MovieResultRecommendationDto[]>{
    return this.apiService.get('Recommendation?EmailUser='+email)
      .pipe(map((response : any) => {
        if(response){
          return (response);
        }        
        else{
          return null;
        }
      }));
  }
}
