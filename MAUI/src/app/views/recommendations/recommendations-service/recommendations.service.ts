import { Injectable } from '@angular/core';
import { RestApiService } from '../../../shared/services/rest-api.service';
import { Observable, map } from 'rxjs';
import { SessionDto } from '../../../shared/models/session-dto';
import { RecommendationDto } from '../../../shared/models/recommendation-dto';
import { NewRequestDto } from '../../../shared/models/new-request-dto';

@Injectable({
  providedIn: 'root'
})
export class RecommendationsService {

  constructor(private apiService: RestApiService) { }

  getSessionByUser(): Observable<SessionDto[]> {
    return this.apiService.get('AI/Session')
      .pipe(map((response : any) => {
        if(response){
          return (response);
        }        
        else{
          return null;
        }
      }));
  }

  getRecommendationsByUser(): Observable<RecommendationDto[]>{
    return this.apiService.get('AI/Recommendation')
      .pipe(map((response : any) => {
        if(response){
          return (response);
        }        
        else{
          return null;
        }
      }));
  }

  getRecommendationsByRequest(newRequest: NewRequestDto): Observable<RecommendationDto[]>{
    return this.apiService.post('AI/Recommendation', newRequest)
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
