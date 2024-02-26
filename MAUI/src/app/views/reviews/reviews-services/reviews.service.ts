import { Injectable } from '@angular/core';
import { RestApiService } from '../../../shared/services/rest-api.service';
import { Observable, map } from 'rxjs';
import { ReviewDto } from '../../../shared/models/review-dto';

@Injectable({
  providedIn: 'root'
})
export class ReviewsService {

  constructor(private apiService: RestApiService) { }

  getReviews(email: string): Observable<ReviewDto[]>{
    return this.apiService.get('Review')
      .pipe(map((response : ReviewDto[])=>{
        if (response){
          return response;
        }
        else return null;
      }))
  }
}
