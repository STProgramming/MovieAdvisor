import { Injectable } from '@angular/core';
import { RestApiService } from '../../../shared/services/rest-api.service';
import { Observable, map } from 'rxjs';
import { ReviewDto } from '../../../shared/models/review-dto';
import { NewReviewDto } from '../../../shared/models/new-review-dto';

@Injectable({
  providedIn: 'root'
})
export class ReviewsService {

  constructor(private apiService: RestApiService) { }

  getReviews(movieTitle: string | null): Observable<ReviewDto[]>{
    return this.apiService.get('Review?Search='+movieTitle)
      .pipe(map((response : ReviewDto[])=>{
        if (response){
          return response;
        }
        else return null;
      }))
  }

  postReview(newReview: NewReviewDto): Observable<any>{
    return this.apiService.post('Review', newReview)
      .pipe(map((response: ReviewDto[])=>{
        return response;
      }));
  }
}
